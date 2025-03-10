using CSharpFunctionalExtensions;
using FluentValidation.Results;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Extensions;

public static class ValidationExtension
{
    public static ErrorList ToErrorList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;
        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select Error.Validation(error.Code, error.Message, validationError.PropertyName);

        return errors.ToList();
    }
}