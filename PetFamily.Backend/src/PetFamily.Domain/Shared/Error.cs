namespace PetFamily.Domain.Shared;

public record Error
{
    private const string SEPARATOR = "||";
    public string Code { get; set; }
    public string Message { get; set; }
    public ErrorType Type { get; set; }

    public string? InvalidField { get; set; } = null;
    
    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }
    
    public static Error Validation(string code, string message, string? invalidField = null) => new Error(code, message, ErrorType.Validation, invalidField);
    public static Error NotFound(string code, string message) => new Error(code, message, ErrorType.NotFound);
    public static Error Failure(string code, string message) => new Error(code, message, ErrorType.Failure);
    public static Error Conflict(string code, string message) => new Error(code, message, ErrorType.Conflict);

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }
    
    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split("||");
        if (parts.Length < 3)
            throw new ArgumentException("Invalid serialized format");
        
        if (!Enum.TryParse(parts[2], out ErrorType errorType))
            throw new ArgumentException("Invalid serialized format");
        
        return new Error(parts[0], parts[1], errorType);
    }
    
    public ErrorList? ToErrorList() => new([this]);
}