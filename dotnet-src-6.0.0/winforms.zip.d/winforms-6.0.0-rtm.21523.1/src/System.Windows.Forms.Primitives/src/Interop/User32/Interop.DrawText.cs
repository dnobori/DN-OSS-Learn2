// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class User32
    {
        [DllImport(Libraries.User32, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int DrawTextW(Gdi32.HDC hdc, string lpchText, int nCount, ref RECT lprc, DT format);
    }
}
