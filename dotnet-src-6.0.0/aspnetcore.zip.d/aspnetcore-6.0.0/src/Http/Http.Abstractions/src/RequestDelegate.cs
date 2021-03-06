// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// A function that can process an HTTP request.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public delegate Task RequestDelegate(HttpContext context);
}