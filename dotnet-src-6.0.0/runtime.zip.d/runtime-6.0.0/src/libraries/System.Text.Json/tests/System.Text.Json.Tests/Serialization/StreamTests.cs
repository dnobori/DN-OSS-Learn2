// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Text.Json.Serialization.Tests
{
    public sealed class StreamTests_Async : StreamTests
    {
        public StreamTests_Async() : base(JsonSerializerWrapperForStream.AsyncStreamSerializer) { }
    }

    public sealed class StreamTests_Sync : StreamTests
    {
        public StreamTests_Sync() : base(JsonSerializerWrapperForStream.SyncStreamSerializer) { }
    }

    public abstract partial class StreamTests
    {
        /// <summary>
        /// Wrapper for JsonSerializer's Serialize() and Deserialize() methods.
        /// </summary>
        private JsonSerializerWrapperForStream Serializer { get; }

        public StreamTests(JsonSerializerWrapperForStream serializer)
        {
            Serializer = serializer;
        }
    }
}
