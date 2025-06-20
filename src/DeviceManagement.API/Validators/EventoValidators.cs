using DeviceManagement.API.Contracts;
using DeviceManagement.Domain.Enums;
using FluentValidation;

namespace DeviceManagement.API.Validators;

public sealed class CreateEventoValidator : AbstractValidator<CreateEvento>
{
    public CreateEventoValidator()
    {        RuleFor(evento => evento.Tipo)
            .IsInEnum()
            .WithMessage("O tipo de evento é inválido. Use um dos tipos válidos: Ligado, Desligado, Movimento, QuedaSinal");

        RuleFor(evento => evento.DispositivoId)
            .NotEmpty()
            .WithMessage("O dispositivo é obrigatório. Selecione um dispositivo válido");

        RuleFor(evento => evento.DataHora)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("A data e hora do evento não pode ser no futuro. Verifique a informação")
            .When(evento => evento.DataHora.HasValue);
    }
}
