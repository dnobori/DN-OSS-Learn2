// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Ole32
    {
        [ComImport]
        [Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public unsafe interface IConnectionPointContainer
        {
            [PreserveSig]
            HRESULT EnumConnectionPoints(
                IntPtr* ppEnum);

            [PreserveSig]
            HRESULT FindConnectionPoint(
                Guid* riid,
                out IConnectionPoint ppCP);
        }
    }
}
