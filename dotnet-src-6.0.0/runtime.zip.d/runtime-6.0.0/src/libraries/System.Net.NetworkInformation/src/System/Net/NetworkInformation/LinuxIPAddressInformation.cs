// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Versioning;

namespace System.Net.NetworkInformation
{
    internal sealed class LinuxIPAddressInformation : IPAddressInformation
    {
        private readonly IPAddress _address;

        public LinuxIPAddressInformation(IPAddress address)
        {
            _address = address;
        }

        public override IPAddress Address { get { return _address; } }

        [UnsupportedOSPlatform("linux")]
        public override bool IsDnsEligible { get { throw new PlatformNotSupportedException(SR.net_InformationUnavailableOnPlatform); } }

        [UnsupportedOSPlatform("linux")]
        public override bool IsTransient { get { throw new PlatformNotSupportedException(SR.net_InformationUnavailableOnPlatform); } }
    }
}
