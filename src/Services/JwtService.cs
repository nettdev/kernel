using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Mobnet.SharedKernel;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKeyIndetifier = "SecretKey";

    public JwtService(IConfiguration configuration) =>
        _configuration = configuration;

    public string CreateToken(IEnumerable<Tuple<string, string>> values)
    {
        if (!values.Any())
            throw new ArgumentNullException(nameof(values));

        var claims = values.Select(c => new Claim(c.Item1, c.Item2));
        var claimsIdentity = new ClaimsIdentity(claims);
        var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(_configuration[_secretKeyIndetifier]);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
		    Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(180),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
        
		var token = tokenHandler.CreateToken(tokenDescriptor);
		
        return tokenHandler.WriteToken(token);
    }

    public async Task<IEnumerable<Tuple<string, string>>> GetClaims(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration[_secretKeyIndetifier]));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenOptions = new TokenValidationParameters { ValidateIssuerSigningKey = true, ValidateIssuer = false, ValidateAudience = false, IssuerSigningKey = key};
        var validationResult = await tokenHandler.ValidateTokenAsync(token, tokenOptions);

        if(!validationResult.IsValid)
            throw new InvalidTokenException();
        
        return  validationResult.Claims.Select(s => new Tuple<string, string>(s.Key, s.Value?.ToString() ?? string.Empty));
    }
}