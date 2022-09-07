namespace Mobnet.SharedKernel;

public class DomainException : Exception
{
    public DomainException()
    { }

    public DomainException(string message) : base(message)
    { }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    { }

    public static void When(bool hasError, string error) 
    {
        if (hasError)
            throw new DomainException(error);
    }
}