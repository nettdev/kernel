using System.Security.Claims;

namespace Nett.Kernel.UnitTest;

public class TokenServiceTest
{
    [Fact]
    public void Create_WithInfos_ReturnToken()
    {
        //Arrange
        var key = "superhypermegasecretkey";
        var claims = new List<Claim> 
        { 
            new(ClaimTypes.Email, "nett@nett.dev"),
            new(ClaimTypes.Name, "Nett Dev"),
            new(ClaimTypes.Role, "AddUser")
        };

        //Act
        var token = TokenService.Create(claims, key, DateTime.UtcNow.AddHours(1));

        //Act
        Assert.NotEmpty(token);
    }
}