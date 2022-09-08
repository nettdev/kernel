namespace Mobnet.SharedKernel.UnitTest;

public class DomainExceptionTest
{
    [Fact]
    public void Constructor_AddErrorMessage()
    {
        //Arrange && Act
        var message = "Erro ao salvar usuário";
        var exception = new DomainException(message);

        Assert.Equal(exception.Errors?.FirstOrDefault()?.ErrorMessage, message);
    }
}