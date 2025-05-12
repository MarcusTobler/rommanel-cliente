using DevPack.Domain.Abstractions;
using DevPack.Domain.Core;

namespace Rommanel.Domain.Clientes;

public abstract class Cliente : Entity<Guid>, IAggregateRoot
{
    public string Email { get; protected set; }
    public string Telefone { get; protected set; }
    public Endereco Endereco { get; protected set; }
    
    public bool Ativo { get; protected set; }
    public bool Excluido { get; protected set; }
    
    protected Cliente(Guid id, string email, string telefone, Endereco endereco) : base(id) =>
        (Email, Telefone, Endereco) = (email, telefone, endereco);
    
    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;
    public void Excluir() => Excluido = true;

}