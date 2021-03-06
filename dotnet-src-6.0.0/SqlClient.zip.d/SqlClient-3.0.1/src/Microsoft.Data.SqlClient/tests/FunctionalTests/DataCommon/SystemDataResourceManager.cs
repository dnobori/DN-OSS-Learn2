// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace Microsoft.Data.SqlClient.Tests
{
    /// <summary>
    /// Allows user to get resource messages from microsoft.data.sqlclient.dll using dynamic properties/methods.
    /// Refer to comments inside AssemblyResourceManager.cs for more details.
    /// </summary>
    public class SystemDataResourceManager : AssemblyResourceManager
    {
        private static SystemDataResourceManager s_instance = new SystemDataResourceManager();
        public static dynamic Instance
        {
            get
            {
                return s_instance;
            }
        }
        private SystemDataResourceManager() : base(typeof(Microsoft.Data.SqlClient.SqlConnection).GetTypeInfo().Assembly)
        {
        }
    }
}
