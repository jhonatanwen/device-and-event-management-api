using System.Text.RegularExpressions;

namespace DeviceManagement.Domain.ValueObjects;

public sealed class Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O endereço de email é obrigatório e não pode estar vazio.", nameof(email));

        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalizedEmail))
            throw new ArgumentException("O formato do email é inválido. Use um formato válido como exemplo@dominio.com", nameof(email));

        return new Email(normalizedEmail);
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj)
    {
        return obj is Email email && Value == email.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static implicit operator string(Email email) => email.Value;
}
