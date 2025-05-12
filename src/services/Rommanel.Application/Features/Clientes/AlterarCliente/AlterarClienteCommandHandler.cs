using DevPack.Messaging.Commands;
using MediatR;

namespace Rommanel.Application.Features.Clientes.AlterarCliente;

public sealed class AlterarClienteCommandHandler : CommandHandler, IRequestHandler<AlterarClienteCommand, CommandResult>
{
    public async Task<CommandResult> Handle(AlterarClienteCommand request, CancellationToken cancellationToken)
    {
        return CommandResult.HasSuccess();
    }
}