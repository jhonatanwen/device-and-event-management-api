using DeviceManagement.Domain.Common;
using DeviceManagement.Domain.ValueObjects;

namespace DeviceManagement.Domain.Entities;

public sealed class Dispositivo : BaseEntity
{
    private readonly List<Evento> _eventos = [];    public string Serial { get; private set; }
    public Imei Imei { get; private set; }
    public DateTime? DataAtivacao { get; private set; }
    public Guid ClienteId { get; private set; }

    public Cliente Cliente { get; private set; }
    public IReadOnlyCollection<Evento> Eventos => _eventos.AsReadOnly();

    public bool EstaAtivo => DataAtivacao.HasValue;

    private Dispositivo()
    {
        Serial = null!;
        Imei = null!;
        Cliente = null!;
    }    private Dispositivo(string serial, Imei imei, Guid clienteId, DateTime? dataAtivacao = null)
    {
        Serial = serial;
        Imei = imei;
        ClienteId = clienteId;
        DataAtivacao = dataAtivacao;
        Cliente = null!;
    }

    public static Dispositivo Create(string serial, string imei, Guid clienteId, DateTime? dataAtivacao = null)
    {
        if (string.IsNullOrWhiteSpace(serial))
            throw new ArgumentException("Serial é obrigatório", nameof(serial));

        var imeiValueObject = Imei.Create(imei);

        return new Dispositivo(serial.Trim(), imeiValueObject, clienteId, dataAtivacao);
    }    public void Ativar()
    {
        if (EstaAtivo)
            throw new InvalidOperationException("O dispositivo já está ativo. Não é necessário ativá-lo novamente.");

        DataAtivacao = DateTime.UtcNow;
    }

    public void Desativar()
    {
        DataAtivacao = null;
    }

    public void UpdateSerial(string serial)
    {
        if (string.IsNullOrWhiteSpace(serial))
            throw new ArgumentException("Serial é obrigatório", nameof(serial));

        Serial = serial.Trim();
    }

    public void UpdateImei(string imei)
    {
        Imei = Imei.Create(imei);
    }

    public void AdicionarEvento(Evento evento)
    {
        if (!EstaAtivo)
            throw new InvalidOperationException("Não é possível adicionar evento a dispositivo inativo");

        _eventos.Add(evento);
    }

    public void RemoverEvento(Evento evento)
    {
        _eventos.Remove(evento);
    }
}
