﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSG.Security.Authentication.Core;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess.Cfg;

/// <summary>
/// Configuration for the refresh token table
/// </summary>
internal class RefreshTokenCfg : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Content);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}