namespace Mobnet.SharedKernel;

public interface IJwtService
{
    string CreateToken(IEnumerable<Tuple<string, string>> values);
    Task<IEnumerable<Tuple<string, string>>> GetClaims(string token);
}