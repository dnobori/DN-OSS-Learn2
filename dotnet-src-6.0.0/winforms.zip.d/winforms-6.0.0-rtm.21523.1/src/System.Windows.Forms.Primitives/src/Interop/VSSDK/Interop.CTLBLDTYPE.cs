// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

internal partial class Interop
{
    internal static partial class VSSDK
    {
        [Flags]
        public enum CTLBLDTYPE : uint
        {
            FSTDPROPBUILDER = 0x00000001,
            FINTERNALBUILDER = 0x00000002,
            FEDITSOBJIDRECTLY = 0x00000004,
        }
    }
}
