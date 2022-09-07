using FluentValidation;
using FluentValidation.Results;

namespace Mobnet.SharedKernel;

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