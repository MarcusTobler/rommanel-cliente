namespace Rommanel.Domain.Clientes;

public class PessoaFisica : Cliente
{
    public string Nome { get; protected set; }
    public string CPF { get; protected set; }
    public DateTime DataNascimento { get; protected set; }
    
    protected PessoaFisica(Guid id,
        string nome, 
        string cpf, 
        DateTime dataNascimento,
        string email, 
        string telefone, 
        Endereco endereco) : base(id, email, telefone, endereco) { }

    public static PessoaFisica Criar(string nome,
        string cpf,
        DateTime dataNascimento,
        string email,
        string telefone,
        Endereco endereco)
    {
        var pessoaFisica = new PessoaFisica(
            Guid.NewGuid(),
            nome,
            cpf,
            dataNascimento,
            email,
            telefone,
            endereco);
        
        pessoaFisica.RaiseDomainEvent(new ClienteCriadoDomainEvent(pessoaFisica.Id));
        
        return pessoaFisica;
    }
}