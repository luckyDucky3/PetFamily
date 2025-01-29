namespace PetFamily.Domain.Shared;

public record Error
{
    public string Code { get; set; }
    public string Message { get; set; }
    public ErrorType Type { get; set; }
    
    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }
    
    public static Error Validation(string code, string message) => new Error(code, message, ErrorType.Validation);
    public static Error NotFound(string code, string message) => new Error(code, message, ErrorType.NotFound);
    public static Error Failure(string code, string message) => new Error(code, message, ErrorType.Failure);
    public static Error Conflict(string code, string message) => new Error(code, message, ErrorType.Conflict);
}