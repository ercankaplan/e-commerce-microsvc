using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain.Models;
using Payment.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Data.Configurations
{
    public sealed class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => PaymentTransactionId.Of(value))
                .ValueGeneratedNever();

            builder.Property(x => x.OrderId)
                .HasConversion(
                    id => id.Value,
                    value => OrderId.Of(value))
                .IsRequired();

            builder.HasIndex(x => x.OrderId);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Currency)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(x => x.ExternalTransactionId)
                .HasMaxLength(128)
                .IsRequired(false);

            builder.Property(x => x.FailureReason)
                .HasMaxLength(500)
                .IsRequired(false);
        }
    }
}
