using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rommanel.Domain.Clientes;

namespace Rommanel.Infrastructure.Database.Configuration;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("cliente", Schemas.Default);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();
        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(100)")
            .IsRequired();
        builder.Property(x => x.Telefone)
            .HasColumnName("telefone")
            .HasColumnType("varchar(20)")
            .IsRequired();
        builder.OwnsOne(p => p.Endereco, x =>
        {
            x.Property(a => a.Cep)
                .HasColumnName("cep")
                .HasColumnType("varchar(10)")
                .IsRequired(false);
            x.Property(a => a.Logradouro)
                .HasColumnName("logradouro")
                .HasColumnType("varchar(100)")
                .IsRequired(false);
            x.Property(a => a.Numero)
                .HasColumnName("numero")
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            x.Property(a => a.Complemento)
                .HasColumnName("complemento")
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            x.Property(a => a.Bairro)
                .HasColumnName("bairro")
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            x.Property(a => a.Cidade)
                .HasColumnName("cidade")
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            x.Property(a => a.Estado)
                .HasColumnName("estado")
                .HasColumnType("varchar(2)")
                .IsRequired(false);
        });
        builder.Property(p => p.Ativo)
            .HasColumnName("ativo")
            .HasColumnType("boolean")
            .IsRequired();
        builder.Property(p => p.Excluido)
            .HasColumnName("excluido")
            .HasColumnType("boolean")
            .IsRequired();
    }
}