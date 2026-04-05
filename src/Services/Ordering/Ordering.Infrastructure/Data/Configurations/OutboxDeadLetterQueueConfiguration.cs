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
    public class OutboxDeadLetterQueueConfiguration:IEntityTypeConfiguration<OutboxDeadLetterQueue>
    {
        public void Configure(EntityTypeBuilder<OutboxDeadLetterQueue> builder)
        {
            builder.ToTable("OutboxDeadLetterQueue");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.OriginalMessageId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.Error)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(x => x.MovedToDlqOnUtc)
                .IsRequired();
        }
    }
}