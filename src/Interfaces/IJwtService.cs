using System.Security.Claims;

namespace Nett.SharedKernel;

public interface ITokenManager
{
    Task<string> Create(IEnumerable<Claim> claims);
    Task<IEnumerable<Claim>> GetClaims(string token);
}