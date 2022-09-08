namespace Mobnet.SharedKernel.UnitTest;

public class CNPJTest
{
    [Theory]
    [InlineData("12146377000132")]
    [InlineData("06034513000108")]
    [InlineData("02006030000130")]
    public void Constructor_ValidValue_NotThrowException(string cnpjValue)
    {
        //Act && Assert
        new Cnpj(cnpjValue);
    }

    [Theory]
    [InlineData("02828456000134")]
    [InlineData("72610112000146")]
    [InlineData("01874354002128")]
    public void Constructor_InvalidValue_ThrowDomainException(string cnpjValue)
    {
        //Arrange
        void act() => new Cnpj(cnpjValue);

        //Act && Assert
        Assert.Throws<DomainException>(act);
    }
}