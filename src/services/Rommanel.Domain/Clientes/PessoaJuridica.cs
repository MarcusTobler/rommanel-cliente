using DevPack.Domain.Abstractions;
using DevPack.Domain.Core;
using Rommanel.Domain.Clientes.Events;

namespace Rommanel.Domain.Clientes;

public class PessoaJuridica : Entity<Guid>, IAggregateRoot
{
    public Cliente Cliente { get; protected set; }
    public string RazaoSocial { get; protected set; }
    public string CNPJ { get; protected set; }
    public string InscricaoEstadual { get; protected set; }
    
    protected PessoaJuridica() : base(Guid.NewGuid()) { }
    protected PessoaJuridica(Guid id,
        Cliente cliente,
        string razaoSocial,
        string cnpj,
        string inscricaoEstadual,
        string email,
        string telefone,
        Endereco endereco) : base(id) { }

    public static PessoaJuridica Criar(Guid id,
        string razaoSocial,
        string cnpj,
        string inscricaoEstadual,
        string email,
        string telefone,
        Endereco endereco)
    {
        var pessoaJuridica = new PessoaJuridica(
            id,
            Cliente.Criar(id, email, telefone, endereco), 
            razaoSocial,
            cnpj,
            inscricaoEstadual,
            email,
            telefone,
            endereco);
        
        pessoaJuridica.RaiseDomainEvent(new ClienteCriadoDomainEvent(pessoaJuridica.Id));
        
        return pessoaJuridica;
    }
}