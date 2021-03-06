// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using System.Text;

internal partial class Interop
{
    internal partial class Kernel32
    {
        [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        public static extern uint FormatMessageW(
            FormatMessageOptions dwFlags,
            IntPtr lpSource,
            uint dwMessageId,
            uint dwLanguageId,
#pragma warning disable CA1838 // Avoid 'StringBuilder' parameters for P/Invokes
            StringBuilder lpBuffer,
#pragma warning restore CA1838 // Avoid 'StringBuilder' parameters for P/Invokes
            int nSize,
            IntPtr arguments);
    }
}
