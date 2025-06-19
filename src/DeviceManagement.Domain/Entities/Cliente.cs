using DeviceManagement.Domain.Common;
using DeviceManagement.Domain.ValueObjects;

namespace DeviceManagement.Domain.Entities;

public sealed class Cliente : BaseEntity
{
    private readonly List<Dispositivo> _dispositivos = [];    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public string? Telefone { get; private set; }
    public bool Status { get; private set; }

    public IReadOnlyCollection<Dispositivo> Dispositivos => _dispositivos.AsReadOnly();

    private Cliente()
    {
        Nome = null!;
        Email = null!;
    }

    private Cliente(string nome, Email email, string? telefone, bool status)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Status = status;
    }

    public static Cliente Create(string nome, string email, string? telefone = null, bool status = true)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        var emailValueObject = Email.Create(email);

        return new Cliente(nome.Trim(), emailValueObject, telefone?.Trim(), status);
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        Nome = nome.Trim();
    }

    public void UpdateEmail(string email)
    {
        Email = Email.Create(email);
    }

    public void UpdateTelefone(string? telefone)
    {
        Telefone = telefone?.Trim();
    }

    public void Ativar()
    {
        Status = true;
    }

    public void Desativar()
    {        if (_dispositivos.Any(dispositivo => dispositivo.EstaAtivo))
            throw new InvalidOperationException("Não é possível desativar o cliente pois ele possui dispositivos ativos. Desative todos os dispositivos primeiro.");

        Status = false;
    }

    public void AdicionarDispositivo(Dispositivo dispositivo)
    {
        if (!Status)
            throw new InvalidOperationException("Não é possível adicionar dispositivo a cliente inativo");

        if (_dispositivos.Any(dispositivo => dispositivo.Serial == dispositivo.Serial))
            throw new InvalidOperationException("Dispositivo com este serial já existe para este cliente");

        _dispositivos.Add(dispositivo);
    }

    public void RemoverDispositivo(Dispositivo dispositivo)
    {
        _dispositivos.Remove(dispositivo);
    }
}
