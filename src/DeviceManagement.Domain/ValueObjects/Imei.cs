using System.Text.RegularExpressions;

namespace DeviceManagement.Domain.ValueObjects;

public sealed class Imei
{
    private static readonly Regex ImeiRegex = new(@"^\d{15}$", RegexOptions.Compiled);

    public string Value { get; }

    private Imei(string value)
    {
        Value = value;
    }    public static Imei Create(string imei)
    {
        if (string.IsNullOrWhiteSpace(imei))
            throw new ArgumentException("O IMEI é obrigatório e não pode estar vazio.", nameof(imei));

        var cleanImei = imei.Replace(" ", "").Replace("-", "");

        if (!ImeiRegex.IsMatch(cleanImei))
            throw new ArgumentException("O IMEI deve conter exatamente 15 dígitos numéricos.", nameof(imei));

        if (!IsValidLuhnChecksum(cleanImei))
            throw new ArgumentException("O IMEI informado possui dígito verificador inválido. Verifique se o número está correto.", nameof(imei));

        return new Imei(cleanImei);
    }

    private static bool IsValidLuhnChecksum(string imei)
    {
        var sum = 0;
        var isEven = false;

        for (var i = imei.Length - 1; i >= 0; i--)
        {
            var digit = int.Parse(imei[i].ToString());

            if (isEven)
            {
                digit *= 2;
                if (digit > 9)
                    digit = digit / 10 + digit % 10;
            }

            sum += digit;
            isEven = !isEven;
        }

        return sum % 10 == 0;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj)
    {
        return obj is Imei imei && Value == imei.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static implicit operator string(Imei imei) => imei.Value;
}
