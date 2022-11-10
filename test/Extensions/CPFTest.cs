namespace Nett.SharedKernel.UnitTest;

public class CPFTest
{
    [Theory]
    [InlineData("07236209031")]
    [InlineData("88519996000")]
    [InlineData("47024042028")]
    public void Constructor_ValidValue_NotThrowException(string cpfValue)
    {
        //Arrange && Act && Assert
        Cpf cpf = cpfValue;
    }

    [Theory]
    [InlineData("32534645656")]
    [InlineData("88519796000")]
    [InlineData("32523523535")]
    [InlineData("00000000000")]
    [InlineData("12345678900")]
    [InlineData("09876543211")]
    [InlineData("11718985411")]
    [InlineData("07152178418")]
    [InlineData("")]
    public void Constructor_InvalidValue_ThrowDomainException(string cpfValue)
    {
        //Arrange
        void act() => new Cnpj(cpfValue);

        Assert.Throws<DomainException>(act);
    }
}