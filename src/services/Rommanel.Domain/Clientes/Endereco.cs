using DevPack.Domain.Core;

namespace Rommanel.Domain.Clientes;

public class Endereco : ValueObject
{
    public string? Cep { get; protected set; }
    public string? Logradouro { get; protected set; }
    public string? Numero { get; protected set; }
    public string? Complemento { get; protected set; }
    public string? Bairro { get; protected set; }
    public string? Cidade { get; protected set; }
    public string? Estado { get; protected set; }
    
    protected Endereco(string? cep, string? logradouro, string? numero, string? complemento, string? bairro, 
        string? cidade, string? estado) => 
        (Cep, Logradouro, Numero, Complemento, Bairro, Cidade, Estado) =
        (cep, logradouro, numero, complemento, bairro, cidade, estado);

    public static Endereco EnderecoFactory(string? cep, string? logradouro, string? numero, string? complemento, 
        string? bairro, string? cidade, string? estado) =>
        new (cep, logradouro, numero, complemento, bairro, cidade, estado);
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Cep;
        yield return Logradouro;
        yield return Numero;
        yield return Complemento;
        yield return Bairro;
        yield return Cidade;
        yield return Estado;
    }

    public override string ToString()
    {
        return $"{Logradouro}, {Numero} - {Bairro}, {Cep}, {Cidade}/{Estado}";
    }
}