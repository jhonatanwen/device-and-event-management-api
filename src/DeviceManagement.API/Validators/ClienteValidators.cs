using DeviceManagement.API.DTOs;
using FluentValidation;

namespace DeviceManagement.API.Validators;

public sealed class CreateClienteValidator : AbstractValidator<CreateCliente>
{
    public CreateClienteValidator()
    {
        RuleFor(cliente => cliente.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MaximumLength(200)
            .WithMessage("Nome deve ter no máximo 200 caracteres");

        RuleFor(cliente => cliente.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .EmailAddress()
            .WithMessage("Email deve ter um formato válido")
            .MaximumLength(254)
            .WithMessage("Email deve ter no máximo 254 caracteres");

        RuleFor(cliente => cliente.Telefone)
            .MaximumLength(20)
            .WithMessage("Telefone deve ter no máximo 20 caracteres")
            .When(cliente => !string.IsNullOrEmpty(cliente.Telefone));
    }
}

public sealed class UpdateClienteValidator : AbstractValidator<UpdateCliente>
{
    public UpdateClienteValidator()
    {
        RuleFor(cliente => cliente.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MaximumLength(200)
            .WithMessage("Nome deve ter no máximo 200 caracteres");

        RuleFor(cliente => cliente.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .EmailAddress()
            .WithMessage("Email deve ter um formato válido")
            .MaximumLength(254)
            .WithMessage("Email deve ter no máximo 254 caracteres");

        RuleFor(cliente => cliente.Telefone)
            .MaximumLength(20)
            .WithMessage("Telefone deve ter no máximo 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefone));
    }
}

public sealed class PatchClienteValidator : AbstractValidator<PatchCliente>
{
    public PatchClienteValidator()
    {
        RuleFor(cliente => cliente.Status)
            .NotNull()
            .WithMessage("Status deve ser informado para operação PATCH")
            .When(cliente => cliente.Status.HasValue);
    }
}
