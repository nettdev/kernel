using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Nett.Kernel.UnitTest;

#nullable disable

public class AuthorizationBehaviorTest
{
    private Mock<IHttpContextAccessor> _httpContextAcessorMock;
    private Mock<ITokenManager> _tokenManagerMock;
    private Mock<RequestHandlerDelegate<Unit>> _requestHandlerDelegateMock;

    public AuthorizationBehaviorTest()
    {
        _httpContextAcessorMock = new Mock<IHttpContextAccessor>();
        _tokenManagerMock = new Mock<ITokenManager>();
        _requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Unit>>();
    }
    
    [Fact]
    public async Task Handler_Unauthenticad_ThrowUnauthenticatedException()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object, _tokenManagerMock.Object);
    
        //Act
        Task act() => behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        await Assert.ThrowsAsync<UnauthenticatedException>(act);
    }

    [Fact]
    public async Task Handler_DontHavePermission_ThrowUnauthorizedException()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object, _tokenManagerMock.Object);

        _tokenManagerMock.Setup(s => s.GetClaims(It.IsAny<string>())).ReturnsAsync(new List<Claim>
        { 
            new Claim(ClaimTypes.Role, "ListUserQuery") 
        });
    
        //Act
        Task act() => behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        await Assert.ThrowsAsync<UnauthorizedException>(act);
    }

    [Fact]
    public async Task Handler_AnotherTentant_ThrowUnauthorizedException()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object, _tokenManagerMock.Object);

        _httpContextAcessorMock
            .Setup(s => s.HttpContext.Request.Headers["x-tentant"])
            .Returns(Guid.NewGuid().ToString());

        _tokenManagerMock.Setup(s => s.GetClaims(It.IsAny<string>())).ReturnsAsync(new List<Claim>
        { 
            new Claim(ClaimTypes.Role, "AddUserCommand") 
        });
    
        //Act
        Task act() => behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        await Assert.ThrowsAsync<UnauthorizedException>(act);
    }

    [Fact]
    public async Task Handler_CorrectTentant_ReturnUnit()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object, _tokenManagerMock.Object);
        var tentantId = Guid.NewGuid().ToString();

        _httpContextAcessorMock
            .Setup(s => s.HttpContext.Request.Headers["x-tentant"])
            .Returns(tentantId);

        _tokenManagerMock.Setup(s => s.GetClaims(It.IsAny<string>())).ReturnsAsync(new List<Claim>
        { 
            new Claim(ClaimTypes.Role, "AddUserCommand"),
            new Claim(ClaimTypes.GroupSid, tentantId) 
        });
    
        //Act
        var result = await behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        Assert.IsType<Unit>(result);
    }

    [Fact]
    public async Task Handler_HavePermission_ShouldContinueExecution()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object, _tokenManagerMock.Object);

        var claimsValues = new List<Claim>()
        { 
            new(ClaimTypes.Name, "FakeName"),
            new(ClaimTypes.Email, "email@gmail.com"),  
            new(ClaimTypes.Role, "AddUserCommand"),
        };

        _httpContextAcessorMock
            .Setup(s => s.HttpContext.User.Claims)
            .Returns(new List<Claim>{new Claim(ClaimTypes.Role, nameof(AddUserCommand))});

        _tokenManagerMock.Setup(s => s.GetClaims(It.IsAny<string>())).ReturnsAsync(claimsValues);
    
        //Act
        var result = await behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        Assert.IsType<Unit>(result);
    }
}

[Authorize(nameof(AddUserCommand))]
class AddUserCommand : IRequest
{
    public string Name { get; set; }
}
