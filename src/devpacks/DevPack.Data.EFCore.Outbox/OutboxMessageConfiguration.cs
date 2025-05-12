using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevPack.Data.EFCore.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outboxmessages", "messages");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();
        builder.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired();
        builder.Property(x => x.CreatedOnUtc)
            .HasColumnName("createdonutc")
            .IsRequired();
        builder.Property(x => x.ProcessedOnUtc)
            .HasColumnName("processedonutc")
            .IsRequired(false);
        builder.Property(x => x.Error)
            .HasColumnName("error")
            .IsRequired(false);

    }
}