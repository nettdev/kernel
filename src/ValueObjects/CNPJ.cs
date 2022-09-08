namespace Mobnet.SharedKernel;

public struct Cnpj
{
    private readonly string _value;
    private static readonly int[] _multiplicator1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] _multiplicator2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

    public Cnpj(string value)
    {
        _value = value;

        if (value == null)
            MakeInvalid();

        var identiqueDigits = true;
        var lastDigit = -1;
        var position = 0;
        var totalDigit1 = 0;
        var totalDigit2 = 0;

        foreach (var c in _value)
        {
            if (char.IsDigit(c))
            {
                var digito = c - '0';
                
                if (position != 0 && lastDigit != digito)
                {
                    identiqueDigits = false;
                }

                lastDigit = digito;

                if (position < 12)
                {
                    totalDigit1 += digito * _multiplicator1[position];
                    totalDigit2 += digito * _multiplicator2[position];
                }
                else if (position == 12)
                {
                    var dv1 = (totalDigit1 % 11);
                    dv1 = dv1 < 2 
                        ? 0 
                        : 11 - dv1;

                    if (digito != dv1)
                        MakeInvalid();

                    totalDigit2 += dv1 * _multiplicator2[12];
                }
                else if (position == 13)
                {
                    var dv2 = (totalDigit2 % 11);

                    dv2 = dv2 < 2 
                        ? 0 
                        : 11 - dv2;

                    if (digito != dv2)
                        MakeInvalid() ;
                }

                position++;
            }
        }

        var isValid = (position == 14) && !identiqueDigits;

        if (!isValid)
            MakeInvalid();
    }

    public static implicit operator Cnpj(string value)
        => new Cnpj(value);

    public override string ToString()
        => _value;

    private void MakeInvalid() =>
        throw new DomainException("O CNPJ informado é inválido.");
}