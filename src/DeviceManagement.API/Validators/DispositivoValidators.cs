using DeviceManagement.API.Contracts;
using FluentValidation;

namespace DeviceManagement.API.Validators;

public sealed class CreateDispositivoValidator : AbstractValidator<CreateDispositivo>
{
    public CreateDispositivoValidator()
    {        RuleFor(dispositivo => dispositivo.Serial)
            .NotEmpty()
            .WithMessage("O número de série é obrigatório e não pode estar vazio")
            .MaximumLength(100)
            .WithMessage("O número de série deve ter no máximo 100 caracteres");

        RuleFor(dispositivo => dispositivo.Imei)
            .NotEmpty()
            .WithMessage("O IMEI é obrigatório e não pode estar vazio")
            .Matches(@"^\d{15}$")
            .WithMessage("O IMEI deve conter exatamente 15 dígitos numéricos");

        RuleFor(dispositivo => dispositivo.ClienteId)
            .NotEmpty()
            .WithMessage("O cliente é obrigatório. Selecione um cliente válido");

        RuleFor(dispositivo => dispositivo.DataAtivacao)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("A data de ativação não pode ser no futuro. Verifique a data informada")
            .When(dispositivo => dispositivo.DataAtivacao.HasValue);
    }
}

public sealed class UpdateDispositivoValidator : AbstractValidator<UpdateDispositivo>
{
    public UpdateDispositivoValidator()
    {        RuleFor(dispositivo => dispositivo.Serial)
            .NotEmpty()
            .WithMessage("O número de série é obrigatório e não pode estar vazio")
            .MaximumLength(100)
            .WithMessage("O número de série deve ter no máximo 100 caracteres");

        RuleFor(dispositivo => dispositivo.Imei)
            .NotEmpty()
            .WithMessage("O IMEI é obrigatório e não pode estar vazio")
            .Matches(@"^\d{15}$")
            .WithMessage("O IMEI deve conter exatamente 15 dígitos numéricos");

        RuleFor(dispositivo => dispositivo.DataAtivacao)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("A data de ativação não pode ser no futuro. Verifique a data informada")
            .When(dispositivo => dispositivo.DataAtivacao.HasValue);
    }
}
