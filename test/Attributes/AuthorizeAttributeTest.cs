namespace Mobnet.SharedKernel.UnitTest;

public class AuthorizeAttributeTest
{
    [Fact]
    public void Constructor_SetParametersCorrectly() 
    {
        //Arrange
        var permission = "AddUser";
        var group = "Usuários";
        var label = "Adicionar";

        //Act
        var authorizeAttribute = new AuthorizeAttribute(permission, group, label);

        //Assert
        Assert.Equal(authorizeAttribute.Permission, permission);
        Assert.Equal(authorizeAttribute.Group, group);
        Assert.Equal(authorizeAttribute.Label, label);
    }
}