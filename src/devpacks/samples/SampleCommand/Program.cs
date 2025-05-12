using Rommanel.Application.Features.Clientes.RegistrarCliente;

var command = new RegistrarClienteCommand(
    Tipo: 1, 
    PessoaFisica: new PessoaFisicaCommand(
        Nome: "Marcus Tobler",
        Cpf: "832.684.059-91",
        DataNascimento: new DateTime(1974, 07, 15)),
    PessoaJuridica: null,
    Email: "marcus.toblerlive.com",
    Telefone: "(11) 99213-8901",
    Endereco: new EnderecoCommand(
        Cep: "04090-013",
        Logradouro: "Alameda dos Nhambiquaras",
        Numero: "1755",
        Complemento: "APT 133",
        Bairro: "Indianópolis",
        Cidade: "São Paulo",
        Estado: "SP")
);

var isValid = command.IsValid();
Console.WriteLine($"command isValid: {isValid}");

foreach (var error in command.Erros)
{
    Console.WriteLine($"Property {error.PropertyName}: {error.ErrorMessage}");
}

Console.ReadKey();