using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Nett.Kernel;

public class TokenService
{
    public static string Create(IEnumerable<Claim> claims, string key, DateTime expires, string issuer = "", string audience = "")
    {
        var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return  tokenHandler.WriteToken(token);
    }
}