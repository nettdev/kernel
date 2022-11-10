using System.Diagnostics.CodeAnalysis;

namespace Nett.SharedKernel;

[ExcludeFromCodeCoverage]
public class InvalidTokenException : DomainException
{
    public InvalidTokenException() : base("O Token informado é inválido")
    { }
}