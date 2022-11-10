namespace Nett.SharedKernel.UnitTest;

public class CNPJTest
{
    [Theory]
    [InlineData("30317826000140")]
    [InlineData("10794275000106")]
    [InlineData("36215601000169")]
    public void Constructor_ValidValue_NotThrowException(string cnpjValue)
    {
        //Act && Assert
        new Cnpj(cnpjValue);
    }

    [Theory]
    [InlineData("24324235454354")]
    [InlineData("34268765y4t334")]
    [InlineData("12423432545434")]
    [InlineData("")]
    public void Constructor_InvalidValue_ThrowDomainException(string cnpjValue)
    {
        //Arrange
        void act() => new Cnpj(cnpjValue);

        //Act && Assert
        Assert.Throws<DomainException>(act);
    }
}