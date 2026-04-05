using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{

    public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("InboxMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.ReceivedOnUtc)
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
            builder.HasIndex(x => new { x.ProcessedOnUtc, x.ReceivedOnUtc });
        }
    }
}
