// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.DotNet.Cli.Utils;
using System.IO;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace Microsoft.NET.TestFramework.Commands
{
    public sealed class CleanCommand : MSBuildCommand
    {
        public CleanCommand(ITestOutputHelper log, string projectPath, string relativePathToProject = null)
            : base(log, "Clean", projectPath, relativePathToProject)
        {
        }

        public CleanCommand(TestAsset testAsset, string relativePathToProject = null)
           : base(testAsset, "Clean", relativePathToProject)
        {
        }

        protected override bool ExecuteWithRestoreByDefault => false;
    }
}
