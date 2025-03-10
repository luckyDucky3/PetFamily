using Microsoft.AspNetCore.Http.HttpResults;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);
public record Envelope
{
    public object? Result { get; }
   
    public ErrorList? Errors { get; }
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result)
        => new (result, null);
    
    public static Envelope Error(ErrorList? errors) 
        => new(null, errors);
}