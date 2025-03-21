using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;

namespace PetFamily.API.Controllers;

public abstract class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object value)
    {
        var envelope = Envelope.Ok(value);
        return new OkObjectResult(envelope);
    }

    public override CreatedResult Created(string? uri, object? value)
    {
        var envelope = Envelope.Ok(value);
        return base.Created(uri, envelope);
    }
}