using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.Results;

namespace Nett.Kernel;

[ExcludeFromCodeCoverage]
public class DomainException : ValidationException
{    
    public DomainException(string message) : base(message, new List<ValidationFailure> { new("DomainException", message) })
    { }

    public static void When(bool hasError, string message) 
    {
        if (hasError)
            throw new DomainException(message);
    }
}