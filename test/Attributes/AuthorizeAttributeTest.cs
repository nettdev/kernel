namespace Mobnet.SharedKernel.UnitTest;

public class AuthorizeAttributeTest
{
    [Fact]
    public void Constructor_SetParametersCorrectly() 
    {
        //Arrange
        var permission = "AddUser";
        var description = "Adicionar Usuários";

        //Act
        var authorizeAttribute = new AuthorizeAttribute(permission, description);

        //Assert
        Assert.Equal(authorizeAttribute.Permission, permission);
        Assert.Equal(authorizeAttribute.Description, description);
    }
}