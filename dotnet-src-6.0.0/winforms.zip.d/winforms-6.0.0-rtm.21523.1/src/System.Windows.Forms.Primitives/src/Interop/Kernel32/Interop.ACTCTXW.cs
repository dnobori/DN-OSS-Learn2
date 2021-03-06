// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal partial class Interop
{
    internal partial class Kernel32
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public unsafe struct ACTCTXW
        {
            public uint cbSize;
            public ACTCTX_FLAG dwFlags;
            public char* lpSource;
            public ushort wProcessorArchitecture;
            public ushort wLangId;
            public char* lpAssemblyDirectory;
            public IntPtr lpResourceName;
            public char* lpApplicationName;
            public IntPtr hModule;
        }
    }
}
