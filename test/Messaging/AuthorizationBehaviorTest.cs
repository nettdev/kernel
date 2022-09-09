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
        Task act() => behavior.Handle(command, default(CancellationToken), _requestHandlerDelegateMock.Object);
    
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
            .Returns(new List<Claim>{new Claim("permissions", "")});
    
        //Act
        Task act() => behavior.Handle(command, default(CancellationToken), _requestHandlerDelegateMock.Object);
    
        //Assert
        Assert.ThrowsAsync<UnauthenticatedException>(act);
    }

    [Fact]
    public async Task Handler_HavePermission_ShouldContinueExecution()
    {
        //Arrange        
        var command = new AddUserCommand();
        var behavior = new AuthorizationBehavior<AddUserCommand, Unit>(_httpContextAcessorMock.Object);

        _httpContextAcessorMock
            .Setup(s => s.HttpContext.User.Claims)
            .Returns(new List<Claim>{new Claim("permissions", "AddUser")});
    
        //Act
        var result = await behavior.Handle(command, default(CancellationToken), _requestHandlerDelegateMock.Object);
    
        //Assert
        Assert.IsType<Unit>(result);
    }
}

[Authorize("AddUser", "Adicionar Usuários")]
class AddUserCommand : IRequest
{

}

class AddUserCommandHandler : IRequestHandler<AddUserCommand>
{
    public Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }
}