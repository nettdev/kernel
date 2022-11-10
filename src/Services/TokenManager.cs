using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Nett.SharedKernel;

public class TokenManager : ITokenManager
{
    private const string SecrectConfiguration = "SecretKey";
    private readonly IConfiguration _configuration;

    public TokenManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> Create(IEnumerable<Claim> claims)
    {
        if (!claims.Any())
            throw new ArgumentNullException(nameof(claims));

        var claimsIdentity = new ClaimsIdentity(claims);
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKeyString = _configuration[SecrectConfiguration] ?? string.Empty;
		var secretKey = Encoding.UTF8.GetBytes(secretKeyString);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
		    Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(180),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
		};
        
		var token = tokenHandler.CreateToken(tokenDescriptor);
		
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }

    public async Task<IEnumerable<Claim>> GetClaims(string token)
    {
        var secretKeyString = _configuration[SecrectConfiguration] ?? string.Empty;
		var secretKey = Encoding.UTF8.GetBytes(secretKeyString);
        var key = new SymmetricSecurityKey(secretKey);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenOptions = new TokenValidationParameters { ValidateIssuerSigningKey = true, ValidateIssuer = false, ValidateAudience = false, IssuerSigningKey = key};
        var validationResult = await tokenHandler.ValidateTokenAsync(token, tokenOptions);

        if(!validationResult.IsValid)
            throw new InvalidTokenException();
        
        return validationResult?.ClaimsIdentity?.Claims ?? Enumerable.Empty<Claim>();
    }
}