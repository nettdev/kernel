namespace Mobnet.SharedKernel.UnitTest;

public class EnumerationTest
{

    [Fact]
    public void FromName_ReturnInstance()
    {
        //Arrange && Act
        var userType = UserType.FromName("Editor");

        //Assert
        Assert.Equal(userType?.Value, 0);
    }

    [Fact]
    public void FromValue_ReturnInstance()
    {
        //Arrange && Act
        var userType = UserType.FromValue(0);

        //Assert
        Assert.Equal(userType?.Name, "Editor");
    }

    [Fact]
    public void Equal_SameValue_ReturnTrue()
    {
        // Arrange && Act
        var editor1 = UserType.FromValue(0);
        var editor2 = UserType.FromValue(0);
    
        // Assert
        Assert.True(editor1?.Equals(editor2));
    }

    [Fact]
    public void ToString_ReturnEnumName()
    {
        // Arrange && Act
        var editor = UserType.FromValue(0);
    
        // Assert
        Assert.Equal(editor?.ToString(), "Editor");
    }

    private Entity CreateEntity() =>
        new UserTest();
}

class UserType : Enumeration<UserType>
{
    public static UserType Editor = new (nameof(Editor), 0);
    public static UserType Admin = new (nameof(Admin), 1);

    public UserType(string name, int value) : base(name, value)
    {
    }
}