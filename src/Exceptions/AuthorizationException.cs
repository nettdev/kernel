namespace Nett.Kernel;

[ExcludeFromCodeCoverage]
public class AuthorizationException : ValidationException
{
    public AuthorizationException(string message) 
        : base(new List<ValidationFailure> { new("Authorization", message)})
    { }
}