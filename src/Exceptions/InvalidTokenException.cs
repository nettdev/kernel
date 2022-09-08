namespace Mobnet.SharedKernel;

public class InvalidTokenException : DomainException
{
    public InvalidTokenException() : base("O Token informado é inválido")
    { }
}