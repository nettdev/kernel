using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Mobnet.SharedKernel.UnitTest;

public class TokenManagerTests
{
    private Mock<IConfiguration> _configuration;
    private const string SecrectConfiguration = "SecretKey";
    private readonly string _secretKey = "27fe5f6c7a8805b2a38242d069fb914af9d6b749dca72dfe5463c3485dd754b3";

    public TokenManagerTests() =>
        _configuration = new Mock<IConfiguration>();

    [Fact]
    public void CreateToken_EmptyClaims_ThrowArgumentNullException()
    {
        //Arrange
        var tokenManager = new TokenManager(_configuration.Object);
        var claims = new List<Claim>();

        _configuration.Setup(s => s["SecretKey"]).Returns(_secretKey);
    
        //Act
        Task act() => tokenManager.Create(claims);
    
        //Assert
        Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Fact]
    public async Task CreateToken_ReturnToken()
    {
        //Arrange
        var tokenManager = new TokenManager(_configuration.Object);
        var claims = new List<Claim>()
        { 
            new(ClaimTypes.Name, "FakeName"),
            new(ClaimTypes.Email, "email@gmail.com"),  
            new(ClaimTypes.Role, "AddUser;ReadUser"),
        };

        _configuration.Setup(s => s[SecrectConfiguration]).Returns(_secretKey);
    
        //Act
        var token = await tokenManager.Create(claims);
    
        //Assert
        Assert.NotNull(token);
    }

    [Fact]
    public async Task GetClaims_WithInvalidToken_ThrowInvalidTokenException()
    {
        //Arrange
        var tokenManager = new TokenManager(_configuration.Object);
        var invalidToken = "dvberbrhbeabethwtnrsefnhtyehgefdtytrghtrgtrs4re";
        
        _configuration.Setup(s => s[SecrectConfiguration]).Returns(_secretKey);

        //Act
        Task act() => tokenManager.GetClaims(invalidToken);
    
        //Assert
        await Assert.ThrowsAsync<InvalidTokenException>(act);
    }

    [Fact]
    public async Task GetClaims_WithValidToken_ReturnClaims()
    {
        //Arrange
        var tokenManager = new TokenManager(_configuration.Object);
        var claimsValues = new List<Claim>()
        { 
            new(ClaimTypes.GroupSid, "FakeAccountId"),
            new(ClaimTypes.Name, "FakeName"),
            new(ClaimTypes.Email, "email@gmail.com"),  
            new(ClaimTypes.Role, "AddUser;ReadUser"),
        };

        _configuration.Setup(s => s[SecrectConfiguration]).Returns(_secretKey);
        
        var token = await tokenManager.Create(claimsValues);
    
        //Act
        var claims = await tokenManager.GetClaims(token);
    
        //Assert
        Assert.True(claims.Any(c => c.Type.Equals(ClaimTypes.GroupSid)));
        Assert.True(claims.Any(c => c.Type.Equals(ClaimTypes.Name)));
        Assert.True(claims.Any(c => c.Type.Equals(ClaimTypes.Email)));
        Assert.True(claims.Any(c => c.Type.Equals(ClaimTypes.Role)));
    }
}