using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Mobnet.SharedKernel.UnitTest;

public class JwtServiceTest
{
    private Mock<IConfiguration> _configuration;
    private readonly string _secretKey = "27fe5f6c7a8805b2a38242d069fb914af9d6b749dca72dfe5463c3485dd754b3";

    public JwtServiceTest() =>
        _configuration = new Mock<IConfiguration>();

    [Fact]
    public void CreateToken_ReturnToken()
    {
        //Arrange
        var jwtService = new JwtService(_configuration.Object);
        var values = new List<Tuple<string, string>> 
        { 
            new(ClaimTypes.Name, "FakeName"),
            new(ClaimTypes.Email, "email@gmail.com"),  
            new(ClaimTypes.Role, "AddUser;ReadUser"),
        };

        _configuration.Setup(s => s["SecretKey"]).Returns(_secretKey);
    
        //Act
        var jwt = jwtService.CreateToken(values);
    
        //Assert
        Assert.NotNull(jwt);
    }

    [Fact]
    public void GetClaims_WithInvalidToken_ThrowInvalidTokenException()
    {
        //Arrange
        var jwtService = new JwtService(_configuration.Object);
        var values = new List<Tuple<string, string>> { new("name", "FakeName") };

        _configuration.Setup(s => s["SecretKey"]).Returns(_secretKey);
        
        var jwt = jwtService.CreateToken(values);
    
        _configuration.Setup(s => s["SecretKey"]).Returns("InvalidKey");

        //Act
        Task act() => jwtService.GetClaims(jwt);
    
        //Assert
        Assert.ThrowsAsync<InvalidTokenException>(act);
    }

    [Fact]
    public async Task GetClaims_WithValidToken_ReturnClaims()
    {
        //Arrange
        var jwtService = new JwtService(_configuration.Object);
        var values = new List<Tuple<string, string>> 
        { 
            new(ClaimTypes.Name, "FakeName"),
            new(ClaimTypes.Email, "email@gmail.com"),  
            new(ClaimTypes.Role, "AddUser;ReadUser")
        };

        _configuration.Setup(s => s["SecretKey"]).Returns(_secretKey);
        
        var jwt = jwtService.CreateToken(values);
    
        //Act
        var claims = await jwtService.GetClaims(jwt);
    
        //Assert
        Assert.Contains(claims, c => c.Item1.Equals(ClaimTypes.Name));
        Assert.Contains(claims, c => c.Item1.Equals(ClaimTypes.Email));
        Assert.Contains(claims, c => c.Item1.Equals(ClaimTypes.Role));
    }
}