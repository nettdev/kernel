namespace Nett.SharedKernel.UnitTest;

public class EntityTest
{
    [Fact]
    public void Constructor_CreateNewGuid()
    {
        //Arrange && Act
        var entity = new UserTest();

        //Assert
        Assert.NotEqual(entity.Id, Guid.Empty);
    }

    private Entity CreateEntity() =>
        new UserTest();
}

class UserTest : Entity { }