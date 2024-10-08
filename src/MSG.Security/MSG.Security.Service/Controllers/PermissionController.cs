﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSG.Security.Authorization.Client;
using MSG.Security.Permission.UseCases;
using Packages.Application.UseCases;

namespace MSG.Security.Service.Controllers;

[Route("api/permission")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Check if the feature is accessible for the passed roles
    /// </summary>
    /// <param name="featureId"> Feature id (name) </param>
    /// <param name="roles"> Array of roles </param>
    /// <returns> <see langword="true"/> if access is available, otherwise <see langword="false"/> </returns>
    /// <response code="200"> Success </response>
    /// <response code="400"> Invalid feature was passed </response>
    /// <response code="403"> Insufficient access rights to the resource </response>
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [HttpGet("features/{featureId}/isAvailable")]
    [Client]
    public async Task<IActionResult> FeatureIsAvailable(string featureId, [FromQuery] string[] roles)
    {
        var result = await _mediator.Send(new CheckFeatureAccessQuery(featureId, roles));
        return result.ToActionResult();
    }
}