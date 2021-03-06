// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

internal static partial class Interop
{
    internal static partial class User32
    {
        /// <summary>
        ///  Window styles
        /// </summary>
        [Flags]
        public enum WS : uint
        {
            OVERLAPPED      = 0x00000000,
            POPUP           = 0x80000000,
            CHILD           = 0x40000000,
            MINIMIZE        = 0x20000000,
            VISIBLE         = 0x10000000,
            DISABLED        = 0x08000000,
            CLIPSIBLINGS    = 0x04000000,
            CLIPCHILDREN    = 0x02000000,
            MAXIMIZE        = 0x01000000,
            CAPTION         = 0x00C00000,
            BORDER          = 0x00800000,
            DLGFRAME        = 0x00400000,
            VSCROLL         = 0x00200000,
            HSCROLL         = 0x00100000,
            SYSMENU         = 0x00080000,
            THICKFRAME      = 0x00040000,
            TABSTOP         = 0x00010000,
            MINIMIZEBOX     = 0x00020000,
            MAXIMIZEBOX     = 0x00010000
        }
    }
}
