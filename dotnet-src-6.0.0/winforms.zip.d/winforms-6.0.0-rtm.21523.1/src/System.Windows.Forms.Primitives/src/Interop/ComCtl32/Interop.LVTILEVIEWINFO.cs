// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;

internal static partial class Interop
{
    internal static partial class ComCtl32
    {
        public struct LVTILEVIEWINFO
        {
            public uint cbSize;
            public LVTVIM dwMask;
            public LVTVIF dwFlags;
            public Size sizeTile;
            public int cLines;
            public RECT rcLabelMargin;
        }
    }
}
