using FluentValidation;
using MediatR;
using Moq;

namespace Nett.Kernel.UnitTest;

public class ValidationBehaviorTest
{
    private Mock<RequestHandlerDelegate<Unit>> _requestHandlerDelegateMock;

    public ValidationBehaviorTest()
    {
        _requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Unit>>();
    }

    [Fact]
    public async void Handler_ValidParameters_ThrowValidationException()
    {
        //Arrange        
        var command = new AddUserCommand { Name = "Jeziel" };
        var userValidator = new AddUserCommandValidator();
        var behavior = new ValidatorBehavior<AddUserCommand, Unit>(new List<AddUserCommandValidator>{userValidator});
    
        //Act
        var validationResult = await behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        Assert.IsType<Unit>(validationResult);
    }

        [Fact]
    public void Handler_InvalidParameters_ThrowValidationException()
    {
        //Arrange        
        var command = new AddUserCommand();
        var userValidator = new AddUserCommandValidator();
        var behavior = new ValidatorBehavior<AddUserCommand, Unit>(new List<AddUserCommandValidator>{userValidator});
    
        //Act
        Task act() => behavior.Handle(command, _requestHandlerDelegateMock.Object, default(CancellationToken));
    
        //Assert
        Assert.ThrowsAsync<ValidationException>(act);
    }
}

class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Error");
    }
}

class AddUserCommand : IRequest
{
    public string Name { get; set; } = string.Empty;
}