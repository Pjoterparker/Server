using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace PjoterParker.Core.Validation
{
    public class AppValidationException : ValidationException
    {
        public AppValidationException(string property, string message) : base(new List<ValidationFailure>() { new ValidationFailure(property, message) })
        {
        }
    }
}
