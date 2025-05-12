using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rommanel.Domain.Clientes;

namespace Rommanel.Infrastructure.Database.Configuration;

public class PessoaJuridicaConfiguration : IEntityTypeConfiguration<PessoaJuridica>
{
    public void Configure(EntityTypeBuilder<PessoaJuridica> builder)
    {
        builder.ToTable("pessoajuridica", Schemas.Default);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();
        builder.Property(x => x.RazaoSocial)
            .HasColumnName("razaosocial")
            .HasColumnType("varchar(50)")
            .IsRequired();
        builder.Property(x => x.CNPJ)
            .HasColumnName("cnpj")
            .HasColumnType("varchar(14)")
            .IsRequired();
        builder.Property(x => x.InscricaoEstadual)
            .HasColumnName("inscricaoestadual")
            .HasColumnType("varchar(10)")
            .IsRequired();
    }
}