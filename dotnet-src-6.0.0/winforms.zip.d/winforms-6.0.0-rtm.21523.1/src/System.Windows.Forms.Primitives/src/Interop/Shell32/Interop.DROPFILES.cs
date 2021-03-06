// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;

internal partial class Interop
{
    internal static partial class Shell32
    {
        public struct DROPFILES
        {
            public uint pFiles;
            public Point pt;
            public BOOL fNC;
            public BOOL fWide;
        }
    }
}
