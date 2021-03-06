// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

internal static partial class Interop
{
    internal static partial class Ole32
    {
        public sealed class GPStream : IStream
        {
            private readonly Stream _dataStream;

            // to support seeking ahead of the stream length...
            private long _virtualPosition = -1;

            internal GPStream(Stream stream)
            {
                _dataStream = stream;
            }

            private void ActualizeVirtualPosition()
            {
                if (_virtualPosition == -1)
                    return;

                if (_virtualPosition > _dataStream.Length)
                    _dataStream.SetLength(_virtualPosition);

                _dataStream.Position = _virtualPosition;

                _virtualPosition = -1;
            }

            public IStream Clone()
            {
                // The cloned object should have the same current "position"
                return new GPStream(_dataStream)
                {
                    _virtualPosition = _virtualPosition
                };
            }

            public void Commit(STGC grfCommitFlags)
            {
                _dataStream.Flush();

                // Extend the length of the file if needed.
                ActualizeVirtualPosition();
            }

            public unsafe void CopyTo(IStream pstm, ulong cb, ulong* pcbRead, ulong* pcbWritten)
            {
                byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);

                ulong remaining = cb;
                ulong totalWritten = 0;
                ulong totalRead = 0;

                fixed (byte* b = buffer)
                {
                    while (remaining > 0)
                    {
                        uint read = remaining < (ulong)buffer.Length ? (uint)remaining : (uint)buffer.Length;
                        Read(b, read, &read);
                        remaining -= read;
                        totalRead += read;

                        if (read == 0)
                        {
                            break;
                        }

                        uint written;
                        pstm.Write(b, read, &written);
                        totalWritten += written;
                    }
                }

                ArrayPool<byte>.Shared.Return(buffer);

                if (pcbRead is not null)
                    *pcbRead = totalRead;

                if (pcbWritten is not null)
                    *pcbWritten = totalWritten;
            }

            public unsafe void Read(byte* pv, uint cb, uint* pcbRead)
            {
                ActualizeVirtualPosition();

                Span<byte> buffer = new Span<byte>(pv, checked((int)cb));
                int read = _dataStream.Read(buffer);

                if (pcbRead is not null)
                    *pcbRead = (uint)read;
            }

            public void Revert()
            {
                // We never report ourselves as Transacted, so we can just ignore this.
            }

            public unsafe void Seek(long dlibMove, SeekOrigin dwOrigin, ulong* plibNewPosition)
            {
                long position = _virtualPosition;
                if (_virtualPosition == -1)
                {
                    position = _dataStream.Position;
                }

                long length = _dataStream.Length;
                switch (dwOrigin)
                {
                    case SeekOrigin.Begin:
                        if (dlibMove <= length)
                        {
                            _dataStream.Position = dlibMove;
                            _virtualPosition = -1;
                        }
                        else
                        {
                            _virtualPosition = dlibMove;
                        }

                        break;
                    case SeekOrigin.End:
                        if (dlibMove <= 0)
                        {
                            _dataStream.Position = length + dlibMove;
                            _virtualPosition = -1;
                        }
                        else
                        {
                            _virtualPosition = length + dlibMove;
                        }

                        break;
                    case SeekOrigin.Current:
                        if (dlibMove + position <= length)
                        {
                            _dataStream.Position = position + dlibMove;
                            _virtualPosition = -1;
                        }
                        else
                        {
                            _virtualPosition = dlibMove + position;
                        }

                        break;
                }

                if (plibNewPosition is null)
                    return;

                if (_virtualPosition != -1)
                {
                    *plibNewPosition = (ulong)_virtualPosition;
                }
                else
                {
                    *plibNewPosition = (ulong)_dataStream.Position;
                }
            }

            public void SetSize(ulong value)
            {
                _dataStream.SetLength(checked((long)value));
            }

            public void Stat(out STATSTG pstatstg, STATFLAG grfStatFlag)
            {
                pstatstg = new STATSTG
                {
                    cbSize = (ulong)_dataStream.Length,
                    type = STGTY.STREAM,

                    // Default read/write access is READ, which == 0
                    grfMode = _dataStream.CanWrite
                        ? _dataStream.CanRead
                            ? STGM.READWRITE
                            : STGM.WRITE
                        : STGM.Default
                };

                if (grfStatFlag == STATFLAG.DEFAULT)
                {
                    // Caller wants a name
                    pstatstg.AllocName(_dataStream is FileStream fs ? fs.Name : _dataStream.ToString());
                }
            }

            public unsafe void Write(byte* pv, uint cb, uint* pcbWritten)
            {
                ActualizeVirtualPosition();

                var buffer = new ReadOnlySpan<byte>(pv, checked((int)cb));
                _dataStream.Write(buffer);

                if (pcbWritten is not null)
                    *pcbWritten = cb;
            }

            public HRESULT LockRegion(ulong libOffset, ulong cb, uint dwLockType)
            {
                // Documented way to say we don't support locking
                return HRESULT.STG_E_INVALIDFUNCTION;
            }

            public HRESULT UnlockRegion(ulong libOffset, ulong cb, uint dwLockType)
            {
                // Documented way to say we don't support locking
                return HRESULT.STG_E_INVALIDFUNCTION;
            }

            public Stream GetDataStream() => _dataStream;
        }
    }
}
