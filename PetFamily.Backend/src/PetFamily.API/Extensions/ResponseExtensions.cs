using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Domain.Shared;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PetFamily.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var responseError = new ResponseError(error.Code, error.Message, null);
        
        var envelope = Envelope.Error([responseError]);
        return new ObjectResult(envelope) { StatusCode = statusCode };
    }
    
    public static ActionResult ToValidationErrorResponse(this ValidationResult result)
    {
        if (result.IsValid)
        {
            throw new InvalidOperationException("Result can not be succeed");
        }
            
        var validationErrors = result.Errors;

        List<ResponseError> responseErrors = [];
        responseErrors.AddRange(from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select new ResponseError(error.Code, error.Message, validationError.PropertyName));

        var envelope = Envelope.Error(responseErrors);
        
        return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
    }
}