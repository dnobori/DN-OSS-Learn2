// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

internal static partial class Interop
{
    internal static partial class ComCtl32
    {
        /// <summary>
        /// Represents MonthCalendar Control Grid Info Flags.
        /// Copied form CommCtrl.h
        /// </summary>
        [Flags]
        public enum MCGIF
        {
            /// <summary>
            /// Represents MCGIF_DATE const.
            /// </summary>
            DATE = 0x00000001,

            /// <summary>
            /// Represents MCGIF_RECT const.
            /// </summary>
            RECT = 0x00000002,

            /// <summary>
            /// Represents MCGIF_NAME const.
            /// </summary>
            NAME = 0x00000004
        }
    }
}
