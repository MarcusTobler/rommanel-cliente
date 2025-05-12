using DevPack.Domain.Messaging.Commands;
using FluentValidation;

namespace Rommanel.Application.Features.Clientes.RegistrarCliente;

public sealed class RegistrarClienteCommandValidatorConfiguration
    : AbstractValidator<RegistrarClienteCommand>,
        ICommandValidatorConfiguration<RegistrarClienteCommand>
{
    public void Validate(CommandValidatorBuilder<RegistrarClienteCommand> builder)
    {
        builder.RuleFor(x => x.Tipo)
            .InclusiveBetween(1, 2)
            .WithMessage("Tipo inválido. Tipo deverá ser: 1 (Pessoa Física) ou 2 (Pessoa Jurídica)");
        
        builder.RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email inválido.");
    }
}