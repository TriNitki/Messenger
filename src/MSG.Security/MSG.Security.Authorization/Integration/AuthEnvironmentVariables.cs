﻿namespace MSG.Security.Authorization.Integration;

/// <summary>
/// Environment variables
/// </summary>
internal static class AuthEnvironmentVariables
{
    /// <summary>
    /// Fabio Url address
    /// </summary>
    internal static string FabioUrl => nameof(FabioUrl);

    /// <summary>
    /// Authorization service name
    /// </summary>
    internal static string SecurityServiceUri => nameof(SecurityServiceUri);
}