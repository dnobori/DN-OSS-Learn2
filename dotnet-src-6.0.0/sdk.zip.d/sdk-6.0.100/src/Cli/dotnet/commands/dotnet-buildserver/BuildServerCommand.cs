// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Tools.BuildServer.Shutdown;

namespace Microsoft.DotNet.Tools.BuildServer
{
    public class BuildServerCommand : DotNetTopLevelCommandBase
    {
        protected override string CommandName => "build-server";
        protected override string FullCommandNameLocalized => LocalizableStrings.BuildServerCommandName;
        protected override string ArgumentName => "";
        protected override string ArgumentDescriptionLocalized => "";

        internal override Dictionary<string, Func<ParseResult, CommandBase>> SubCommands =>
            new Dictionary<string, Func<ParseResult, CommandBase>>
            {
                ["shutdown"] = appliedOption => new BuildServerShutdownCommand(
                    ParseResult),
            };

        public static int Run(string[] args)
        {
            return new BuildServerCommand().RunCommand(args);
        }
    }
}
