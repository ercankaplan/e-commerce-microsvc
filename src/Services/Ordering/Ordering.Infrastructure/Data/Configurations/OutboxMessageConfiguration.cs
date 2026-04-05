using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.OccurredOnUtc)
                .IsRequired();

            builder.Property(x => x.EventName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.EventType)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.EventVersion)
                .IsRequired();

            builder.Property(x => x.EnvelopeVersion)
                .IsRequired();

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Payload)
                .IsRequired();

            builder.Property(x => x.Metadata);

            builder.Property(x => x.ProcessedOnUtc);

            builder.Property(x => x.RetryCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.LastError)
                .HasMaxLength(4000);

            builder.HasIndex(x => x.ProcessedOnUtc);
            builder.HasIndex(x => new { x.ProcessedOnUtc, x.OccurredOnUtc });
        }
    }
}
