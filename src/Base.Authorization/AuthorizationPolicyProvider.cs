﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Base.Authorization.Permission;

namespace Base.Authorization;

/// <summary>
/// Authorization policy provider
/// </summary>
public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    /// <summary>
    /// All policies
    /// </summary>
    private readonly ConcurrentDictionary<string, AuthorizationPolicy> _allPolicies;

    /// <summary>
    /// Default authorization policy provider
    /// </summary>
    private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _allPolicies = new ConcurrentDictionary<string, AuthorizationPolicy>();
        _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    /// <summary>
    /// Get default policy
    /// </summary>
    /// <returns> Default policy </returns>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _defaultPolicyProvider.GetDefaultPolicyAsync();
    }

    /// <summary>
    /// Get fallback policy
    /// </summary>
    /// <returns> Fallback policy </returns>
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _defaultPolicyProvider.GetDefaultPolicyAsync()!;
    }

    /// <summary>
    /// Get authorization policy
    /// </summary>
    /// <param name="policyName"> Policy name </param>
    /// <returns> Authorization policy  </returns>
    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PermissionAttribute.POLICY_PREFIX))
        {
            return _allPolicies.GetOrAdd(policyName, CreatePermissionPolicy);
        }

        return await _defaultPolicyProvider.GetPolicyAsync(policyName);
    }

    private static AuthorizationPolicy CreatePermissionPolicy(string policyName)
    {
        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName[PermissionAttribute.POLICY_PREFIX.Length..]))
            .Build();
    }
}
