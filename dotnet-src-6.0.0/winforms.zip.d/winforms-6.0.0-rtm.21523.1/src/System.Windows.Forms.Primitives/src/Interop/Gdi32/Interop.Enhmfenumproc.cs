// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

internal static partial class Interop
{
    internal static partial class Gdi32
    {
        public unsafe delegate BOOL Enhmfenumproc(
            HDC hdc,
            HGDIOBJ* lpht,
            ENHMETARECORD* lpmr,
            int nHandles,
            IntPtr data);
    }
}
