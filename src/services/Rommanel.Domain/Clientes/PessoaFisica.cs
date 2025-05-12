using DevPack.Domain.Abstractions;
using DevPack.Domain.Core;
using Rommanel.Domain.Clientes.Events;

namespace Rommanel.Domain.Clientes;

public class PessoaFisica : Entity<Guid>, IAggregateRoot
{
    public Cliente Cliente { get; protected set; }
    public string Nome { get; protected set; }
    public string CPF { get; protected set; }
    public DateTime DataNascimento { get; protected set; }
    
    protected PessoaFisica() : base(Guid.NewGuid()) { }
    protected PessoaFisica(Guid id,
        Cliente cliente,
        string nome, 
        string cpf, 
        DateTime dataNascimento,
        string email, 
        string telefone, 
        Endereco endereco) : base(id) { }

    public static PessoaFisica Criar(Guid id,
        string nome,
        string cpf,
        DateTime dataNascimento,
        string email,
        string telefone,
        Endereco endereco)
    {
        var pessoaFisica = new PessoaFisica(
            id,
            Cliente.Criar(id, email, telefone, endereco),
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