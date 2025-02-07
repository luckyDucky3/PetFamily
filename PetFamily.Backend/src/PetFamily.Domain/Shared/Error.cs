namespace PetFamily.Domain.Shared;

public record Error
{
    private const string SEPARATOR = "||";
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
}