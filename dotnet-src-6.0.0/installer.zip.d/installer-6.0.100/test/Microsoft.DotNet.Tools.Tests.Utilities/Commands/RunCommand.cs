// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.DotNet.Cli.Utils;
using System.Runtime.InteropServices;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public sealed class RunCommand : DotnetCommand
    {
        public override CommandResult Execute(string args = "")
        {
            args = $"run {args}";
            return base.Execute(args);
        }

        public override CommandResult ExecuteWithCapturedOutput(string args = "")
        {
            args = $"run {args}";
            return base.ExecuteWithCapturedOutput(args);
        }
    }
}
