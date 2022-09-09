namespace Mobnet.SharedKernel.UnitTest;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("fake@gmail.com")]
    [InlineData("fake@outlook.com.br")]
    [InlineData("fake@outlook.com")]
    public void IsEmail_ValidEmailAddress_ReturnTrue(string emailAddress)
    {
        Assert.True(emailAddress.IsEmail());
    }

    [Theory]
    [InlineData("fake@.com")]
    [InlineData("fake@outloo")]
    [InlineData("fakeoutlook.com")]
    public void IsEmail_InvalidEmailAddress_ReturnTrue(string emailAddress)
    {
        Assert.False(emailAddress.IsEmail());
    }

    [Fact]
    public void IsMissing_EmptyString_ReturnTrue()
    {
        //Arrange
        var name = string.Empty;
    
        //Act && Assert
        Assert.True(name.IsMissing());
    }

    [Fact]
    public void IsMissing_StringWithValid_ReturnFalse()
    {
        //Arrange
        var name = "Fake Value";
    
        //Act && Assert
        Assert.False(name.IsMissing());
    }

    [Fact]
    public void IsPresent_StringWithValid_ReturnTrue()
    {
        //Arrange
        var name = "Fake Value";
    
        //Act && Assert
        Assert.True(name.IsPresent());
    }

    [Fact]
    public void IsPresent_EmptyString_ReturnFalse()
    {
        //Arrange
        var name = string.Empty;
    
        //Act && Assert
        Assert.False(name.IsPresent());
    }

    [Fact]
    public void TruncateSensitiveInformation_ReturnModifiedString()
    {
        //Arrange
        var info = "Fake sensitive information";

        //Act
        var truncatedInfo = info.TruncateSensitiveInformation();

        //Assert
        Assert.NotEqual(info, truncatedInfo);
    }

    [Fact]
    public void ToSha256_ReturnHash()
    {
        //Arrange
        var text = "text";
        var hash = "982d9e3eb996f559e633f4d194def3761d909f5a3b647d1a851fead67c32c9d1";

        //Act
        var generatedHash = text.ToSha256();

        //Assert
        Assert.Equal(generatedHash, hash);
    }

    [Fact]
    public void RemoveDiacritics_ReturnTextWithoutDacritics()
    {
        //Arrange
        var input = "Olá, não gosto de açaí";
        var expectedOutput = "Ola, nao gosto de acai";

        //Act
        var output = input.RemoveDiacritics();

        //Assert
        Assert.Equal(output, expectedOutput);
    }

    [Fact]
    public void Urlize_ReturnUrl()
    {
        //Arrange
        var input = "-Olá, não gosto de açaí-";
        var expectedOutput = "ola-nao-gosto-de-acai";

        //Act
        var output = input.Urlize();

        //Assert
        Assert.Equal(output, expectedOutput);
    }

    [Theory]
    [InlineData("12a", "12")]
    [InlineData("b25", "25")]
    [InlineData("1243523sfdg2413432", "12435232413432")]
    public void OnlyNumber_ReturnOnlyNumber(string input, string expectedOutput)
    {
        //Arrange && Act
        var output = input.OnlyNumbers();

        //Assert
        Assert.Equal(output, expectedOutput);
    }

    [Theory]
    [InlineData("olá", "Olá")]
    [InlineData("eu", "Eu")]
    [InlineData("oi", "Oi")]
    public void Captalize_ReturnCaptalizedString(string input, string expectedOutput)
    {
        //Arrange && Act
        var output = input.Capitalize(true);

        //Assert
        Assert.Equal(output, expectedOutput);
    }
}