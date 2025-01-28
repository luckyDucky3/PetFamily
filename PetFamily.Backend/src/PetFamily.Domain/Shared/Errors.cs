namespace PetFamily.Domain.Shared;

public class Errors
{
    public static class General
    {
        public static Error IsInvalid(string? title = null)
        {
            var label = title ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }

        public static Error IsNotFound(Guid? id = null)
        {
            var forId = id is null ? "" : $" for id : {id}";
            return Error.Validation("id.notfound", $"record not found{forId}");
        }

        public static Error IsInvalidLength(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("length.is.invalid", $"{label} is invalid length");
        }
        public static Error IsRequired(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.required", $"{name} is required");
        }

        public static Error IsNullOrWhitespace(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.null.or.whitespace", $"{label} is null or whitespace");
        }
    }
}