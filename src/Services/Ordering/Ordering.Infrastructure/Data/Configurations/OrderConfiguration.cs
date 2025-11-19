using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                  .HasConversion(
                      v => v.Value,
                      v => OrderId.Of(v));

            //builder.Property(e => e.CustomerId)
            //      .HasConversion(
            //          v => v.Value,
            //          v => CustomerId.Of(v));

            builder.HasOne<Customer>()
                   .WithMany()
                   .HasForeignKey(o => o.CustomerId)
                   .IsRequired();

            builder.HasMany(o => o.OrderItems)
                   .WithOne()
                   .HasForeignKey(oi => oi.OrderId);
                   //.IsRequired()
                   //.OnDelete(DeleteBehavior.Cascade);       

            builder.ComplexProperty(o => o.OrderName,nameBuilder => {
                nameBuilder.Property(n => n.Value)
                .HasColumnName(nameof(Order.OrderName))
                .HasMaxLength(100)
                .IsRequired();

            } );

            builder.ComplexProperty(o => o.ShippingAddress, shippingBuilder => {
                shippingBuilder.Property(n => n.FirstName)
                .HasMaxLength(50)
                .IsRequired();
                
                shippingBuilder.Property(n => n.LastName)
                .HasMaxLength(50)
                 .IsRequired();

                shippingBuilder.Property(n => n.Email).HasMaxLength(50);

                shippingBuilder.Property(n => n.AddressLine)
                 .HasMaxLength(180)
                .IsRequired();

                shippingBuilder.Property(n => n.Country).HasMaxLength(50);

                shippingBuilder.Property(n => n.State).HasMaxLength(50);

                shippingBuilder.Property(n => n.ZipCode)
                .HasMaxLength(50)
                .IsRequired(); ;

            });

            builder.ComplexProperty(o => o.BillingAddress, billingBuilder => {
                billingBuilder.Property(n => n.FirstName)
                .HasMaxLength(50)
                .IsRequired();

                billingBuilder.Property(n => n.LastName)
                .HasMaxLength(50)
                 .IsRequired();

                billingBuilder.Property(n => n.Email).HasMaxLength(50);

                billingBuilder.Property(n => n.AddressLine)
                 .HasMaxLength(180)
                .IsRequired();

                billingBuilder.Property(n => n.Country).HasMaxLength(50);

                billingBuilder.Property(n => n.State).HasMaxLength(50);

                billingBuilder.Property(n => n.ZipCode)
                .HasMaxLength(50)
                .IsRequired(); ;

            });

            builder.ComplexProperty(o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(n => n.CardName)
                .HasMaxLength(50);

                paymentBuilder.Property(n => n.CardNumber)
                .HasMaxLength(24)
                 .IsRequired();

                paymentBuilder.Property(n => n.Expiration).HasMaxLength(10);

                paymentBuilder.Property(n => n.CVV)
                 .HasMaxLength(3);


                paymentBuilder.Property(n => n.PaymentMethod);



            });


            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Draft)
                .HasConversion(
                
                 s => s.ToString(),
                 dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus)

                 );


            builder.Property(o => o.TotalPrice);

        }
    }
}
