// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

internal static partial class Interop
{
    internal static partial class Ole32
    {
        [Flags]
        public enum CTRLINFO : uint
        {
            EATS_RETURN = 1,
            EATS_ESCAPE = 2,
        }
    }
}
