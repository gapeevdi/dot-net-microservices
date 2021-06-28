using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        private IDictionary<string, string[]> Errors { get; }

        public ValidationException() : base("One or more validation failures occured")
        {
            
        }

        public ValidationException(IReadOnlyCollection<ValidationFailure> failures):this()
        {
            Errors = failures.GroupBy(failure => failure.PropertyName, failure => failure.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
