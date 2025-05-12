using DevPack.Messaging.Commands;
using MediatR;

namespace Rommanel.Application.Features.Clientes.ExcluirCliente;

public class ExcluirClienteCommandHandler : CommandHandler, IRequestHandler<ExcluirClienteCommand, CommandResult>
{
    public async Task<CommandResult> Handle(ExcluirClienteCommand command, CancellationToken cancellationToken)
    {
        if (command.IsValid())
            return CommandResult.HasFailure(command.ValidationResult);
        
        return CommandResult.HasSuccess();
    }
}