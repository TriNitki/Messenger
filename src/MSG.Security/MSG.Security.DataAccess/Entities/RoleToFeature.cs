﻿using System.ComponentModel.DataAnnotations;

namespace MSG.Security.DataAccess.Entities;

/// <summary>
/// Role to feature entity
/// </summary>
public class RoleToFeature
{
    /// <summary>
    /// Role id (name)
    /// </summary>
    [MaxLength(64)]
    public string RoleId { get; set; } = string.Empty;

    /// <summary>
    /// Feature id
    /// </summary>
    [MaxLength(64)]
    public string FeatureId { get; set; } = string.Empty;

    /// <summary>
    /// Feature
    /// </summary>
    public Feature Feature { get; set; } = null!;
}