using Rommanel.Domain.Clientes.Events;

namespace Rommanel.Domain.Clientes;

public class PessoaJuridica : Cliente
{
    public string RazaoSocial { get; protected set; }
    public string CNPJ { get; protected set; }
    public string InscricaoEstadual { get; protected set; }
    
    protected PessoaJuridica(Guid id,
        string razaoSocial,
        string cnpj,
        string inscricaoEstadual,
        string email,
        string telefone,
        Endereco endereco) : base(id, email, telefone, endereco) { }

    public static PessoaJuridica Criar(string razaoSocial,
        string cnpj,
        string inscricaoEstadual,
        string email,
        string telefone,
        Endereco endereco)
    {
        var pessoaJuridica = new PessoaJuridica(
            Guid.NewGuid(),
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