namespace Rommanel.WebAPI.Models.Requests;

public sealed record CriarClienteRequest(
    int Tipo,
    string Email,
    string Telefone,
    EnderecoRequest Endereco,
    PessoaFisicaRequest? PessoaFisica,
    PessoaJuridicaRequest? PessoaJuridica
    );

public sealed record EnderecoRequest(
    string Cep,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string Estado);

public sealed record PessoaFisicaRequest(
    string Nome,
    string Cpf,
    DateTime DataNascimento);

public sealed record PessoaJuridicaRequest(
    string RazaoSocial,
    string Cnpj,
    string InscricaoEstadual);

    