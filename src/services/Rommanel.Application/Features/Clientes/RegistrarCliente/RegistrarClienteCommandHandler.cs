using DevPack.Messaging.Commands;
using MediatR;
using Rommanel.Domain.Clientes;

namespace Rommanel.Application.Features.Clientes.RegistrarCliente;

public class RegistrarClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, CommandResult>
{
    public async Task<CommandResult> Handle(RegistrarClienteCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
            return CommandResult.HasFailure(command.ValidationResult);

        return command.Tipo switch
        {
            1 => await CriarPessoaFisica(
                PessoaFisica.Criar(Guid.NewGuid(), 
                    command.PessoaFisica.Nome, 
                    command.PessoaFisica.Cpf,
                    command.PessoaFisica.DataNascimento, 
                    command.Email, 
                    command.Telefone,
                Endereco.EnderecoFactory(
                    command.Endereco.Cep, 
                    command.Endereco.Logradouro, 
                    command.Endereco.Numero,
                    command.Endereco.Complemento, 
                    command.Endereco.Bairro, 
                    command.Endereco.Cidade,
                    command.Endereco.Estado))),
            2 => await CriarPessoaJuridica(
                PessoaJuridica.Criar(Guid.NewGuid(), 
                    command.PessoaJuridica.RazaoSocial,
                    command.PessoaJuridica.Cnpj, 
                    command.PessoaJuridica.InscricaoEstadual, 
                    command.Email, 
                    command.Telefone,
                Endereco.EnderecoFactory(
                    command.Endereco.Cep, 
                    command.Endereco.Logradouro, 
                    command.Endereco.Numero,
                    command.Endereco.Complemento, 
                    command.Endereco.Bairro, 
                    command.Endereco.Cidade,
                    command.Endereco.Estado))),
            _ => throw new ArgumentOutOfRangeException(nameof(command.Tipo), "Tipo deverá ser: 1 - Pessoa Física; 2 - Pessoa Jurídica")
        };
        
        return CommandResult.HasSuccess();
    }
    
    private async Task<CommandResult> CriarPessoaFisica(PessoaFisica pessoaFisica)
    {
        
        return CommandResult.HasSuccess();
    }

    private async Task<CommandResult> CriarPessoaJuridica(PessoaJuridica pessoaJuridica)
    {
        return CommandResult.HasSuccess();
    }
}