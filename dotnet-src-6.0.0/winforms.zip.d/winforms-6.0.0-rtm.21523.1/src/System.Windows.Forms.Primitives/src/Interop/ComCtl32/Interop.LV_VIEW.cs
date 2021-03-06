// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

internal static partial class Interop
{
    internal static partial class ComCtl32
    {
        public enum LV_VIEW : uint
        {
            ICON = 0x0000,
            DETAILS = 0x0001,
            SMALLICON = 0x0002,
            LIST = 0x0003,
            TILE = 0x0004,
            MAX = 0x0004
        }
    }
}
