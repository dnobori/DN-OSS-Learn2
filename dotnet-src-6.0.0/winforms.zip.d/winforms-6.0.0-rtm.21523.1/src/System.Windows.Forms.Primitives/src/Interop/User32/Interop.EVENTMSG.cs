// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

internal static partial class Interop
{
    internal static partial class User32
    {
        public struct EVENTMSG
        {
            public WM message;
            public uint paramL;
            public uint paramH;
            public uint time;
            public IntPtr hwnd;
        }
    }
}
