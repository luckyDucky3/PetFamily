using Microsoft.AspNetCore.Http.HttpResults;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);
public record Envelope
{
    public object? Result { get; }
   
    public List<ResponseError> Errors { get; }
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result)
        => new (result, []);
    
    public static Envelope Error(IEnumerable<ResponseError> errors) 
        => new(null, errors);
}