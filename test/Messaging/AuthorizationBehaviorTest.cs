using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Mobnet.SharedKernel.UnitTest;

#nullable disable

public class AuthorizationBehaviorTest
{
    private Mock<IHttpContextAccessor> _httpContextAcessorMock;
    private Mock<RequestHandlerDelegate<Unit>> _requestHandlerDelegateMock;

    public AuthorizationBehaviorTest()
    {
        _httpContextAcessorMock = new Mock<IHttpContextAccessor>();
        _requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Unit>>();
    }
    
    [Fact]
    public void Handler_Unauthenticad_ThrowUnauthenticatedException()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object);
    
        //Act
        Task act() => behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        Assert.ThrowsAsync<UnauthenticatedException>(act);
    }

    [Fact]
    public void Handler_DontHavePermission_ThrowUnauthorizedException()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object);

        _httpContextAcessorMock
            .Setup(s => s.HttpContext.User.Claims)
            .Returns(new List<Claim>{new Claim("resources", "")});
    
        //Act
        Task act() => behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        Assert.ThrowsAsync<UnauthorizedException>(act);
    }

    [Fact]
    public async Task Handler_HavePermission_ShouldContinueExecution()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object);

        _httpContextAcessorMock
            .Setup(s => s.HttpContext.User.Claims)
            .Returns(new List<Claim>{new Claim("resources", nameof(AddUserCommand))});
    
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

class AddUserCommandHandler : IRequestHandler<AddUserCommand>
{
    public Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }
}