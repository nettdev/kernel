namespace Nett.Kernel;

public struct Cpf
{
    private readonly string _value;

    private Cpf(string value)
    {
        _value = value;
            
        if (value is null)
            MakeInvalid();

        var position = 0;
        var totalDigit1 = 0;
        var totalDigit2 = 0;
        var dv1 = 0;
        var dv2 = 0;

        bool identiqueDigits = true;
        var lastDigit = -1;

        foreach (var c in _value)
        {
            if (char.IsDigit(c))
            {
                var digit = c - '0';
                if (position != 0 && lastDigit != digit)
                {
                    identiqueDigits = false;
                }

                lastDigit = digit;
                if (position < 9)
                {
                    totalDigit1 += digit * (10 - position);
                    totalDigit2 += digit * (11 - position);
                }
                else if (position == 9)
                {
                    dv1 = digit;
                }
                else if (position == 10)
                {
                    dv2 = digit;
                }

                position++;
            }
        }

        if (position > 11)
            MakeInvalid();

        if (identiqueDigits)
            MakeInvalid();
            
        var digit1 = totalDigit1 % 11;
        
        digit1 = digit1 < 2 
            ? 0 
            : 11 - digit1;

        if (dv1 != digit1)
            MakeInvalid();

        totalDigit2 += digit1 * 2;

        var digit2 = totalDigit2 % 11;

        digit2 = digit2 < 2 ? 0 : 11 - digit2;

        var isValid = dv2 == digit2;

        if (!isValid)
            MakeInvalid();
    }
        
    public static implicit operator Cpf(string value) => 
        new Cpf(value);

    public override string ToString() => 
        _value;

    private void MakeInvalid() =>
        throw new DomainException("O CPF informado é invalido");
}

        