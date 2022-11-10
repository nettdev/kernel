namespace Nett.SharedKernel.UnitTest;

public class AuthorizeAttributeTest
{
    [Fact]
    public void Constructor_SetParametersCorrectly() 
    {
        //Arrange
        var resource = "AddUserCommand";

        //Act
        var authorizeAttribute = new AuthorizeAttribute(resource);

        //Assert
        Assert.Equal(authorizeAttribute.Resource, resource);
    }
}