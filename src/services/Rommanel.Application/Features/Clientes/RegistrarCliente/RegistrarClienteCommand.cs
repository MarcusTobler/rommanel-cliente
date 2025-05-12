using DevPack.Domain.Messaging.Commands;

namespace Rommanel.Application.Features.Clientes.RegistrarCliente;

public sealed record RegistrarClienteCommand(
    int Tipo,
    PessoaFisicaCommand? PessoaFisica,
    PessoaJuridicaCommand? PessoaJuridica,
    string Email,
    string Telefone,
    EnderecoCommand Endereco) : Command
{
    protected override void OnCommandValidating(ValidatorBuilder builder)
    {
        builder.HasCommandValidator(new RegistrarClienteCommandValidatorConfiguration());
    }
}

public sealed record EnderecoCommand(
    string Cep,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string Estado);
    
public sealed record PessoaFisicaCommand(
    string Nome,
    string Cpf,
    DateTime DataNascimento);

public sealed record PessoaJuridicaCommand(
    string RazaoSocial,
    string Cnpj,
    string InscricaoEstadual);    