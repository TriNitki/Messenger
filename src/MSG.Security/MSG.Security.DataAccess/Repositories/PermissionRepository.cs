﻿using Microsoft.EntityFrameworkCore;
using MSG.Security.Permission.Core;
using MSG.Security.Permission.UseCases.Abstractions;

namespace MSG.Security.DataAccess.Repositories;

/// <summary>
/// <see cref="IPermissionRepository"/> implementation
/// </summary>
public class PermissionRepository : IPermissionRepository
{
    private readonly DataBaseContext _context;

    public PermissionRepository(DataBaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<FeatureModel?> GetFeature(string featureName)
    {
        var feature = await _context.Features
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Name.Equals(featureName))
            .ConfigureAwait(false);

        return feature is null
            ? null
            : new FeatureModel()
            {
                Name = feature.Name,
                Description = feature.Description,
                WhiteList = feature.Roles.Select(x => x.RoleId).ToHashSet()
            };
    }
}