// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Gdi32
    {
        [DllImport(Libraries.Gdi32, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public unsafe static extern HDC CreateEnhMetaFileW(
            HDC hdc,
            string? lpFilename,
            RECT* lprc,
            string? lpDesc);
    }
}
