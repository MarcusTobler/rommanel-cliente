using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rommanel.Domain.Clientes;

namespace Rommanel.Infrastructure.Database.Configuration;

public class PessoaFisicaConfiguration : IEntityTypeConfiguration<PessoaFisica>
{
    public void Configure(EntityTypeBuilder<PessoaFisica> builder)
    {
        builder.ToTable("pessoafisica", Schemas.Default);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();
        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasColumnType("varchar(50)")
            .IsRequired();
        builder.Property(x => x.CPF)
            .HasColumnName("cpf")
            .HasColumnType("varchar(11)")
            .IsRequired();
        builder.Property(x => x.DataNascimento)
            .HasColumnName("datanascimento")
            .HasColumnType("date")
            .IsRequired();

        builder.HasOne(p => p.Cliente)
            .WithOne(p => p.PessoaFisica)
            .HasForeignKey<PessoaFisica>(p => p.Id);
    }
}