// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Versioning;

namespace System.Net.NetworkInformation
{
    internal sealed class LinuxIPv4InterfaceStatistics : IPv4InterfaceStatistics
    {
        private readonly LinuxIPInterfaceStatistics _statistics;

        public LinuxIPv4InterfaceStatistics(string name)
        {
            _statistics = new LinuxIPInterfaceStatistics(name);
        }

        public override long BytesReceived => _statistics.BytesReceived;

        public override long BytesSent => _statistics.BytesSent;

        public override long IncomingPacketsDiscarded => _statistics.IncomingPacketsDiscarded;

        public override long IncomingPacketsWithErrors => _statistics.IncomingPacketsWithErrors;

        [UnsupportedOSPlatform("linux")]
        public override long IncomingUnknownProtocolPackets => _statistics.IncomingUnknownProtocolPackets;

        public override long NonUnicastPacketsReceived => _statistics.NonUnicastPacketsReceived;

        [UnsupportedOSPlatform("linux")]
        public override long NonUnicastPacketsSent => _statistics.NonUnicastPacketsSent;

        public override long OutgoingPacketsDiscarded => _statistics.OutgoingPacketsDiscarded;

        public override long OutgoingPacketsWithErrors => _statistics.OutgoingPacketsWithErrors;

        public override long OutputQueueLength => _statistics.OutputQueueLength;

        public override long UnicastPacketsReceived => _statistics.UnicastPacketsReceived;

        public override long UnicastPacketsSent => _statistics.UnicastPacketsSent;
    }
}
