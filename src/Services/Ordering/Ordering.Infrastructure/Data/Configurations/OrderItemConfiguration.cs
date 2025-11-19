using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Abstractions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                  .HasConversion(
                      orderItemId => orderItemId.Value,
                      dbId => OrderItemId.Of(dbId));

            //builder.Property(e => e.OrderId)
            //      .HasConversion(
            //          v => v.Value,
            //          v => OrderId.Of(v));
            //builder.Property(e => e.ProductId)
            //   .HasConversion(
            //       v => v.Value,
            //       v => ProductId.Of(v));


            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(o => o.ProductId);

            builder.Property(e => e.Quantity)
                .IsRequired();
            builder.Property(e => e.Price)
                .IsRequired();

        }
    }
}
