using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                      .HasConversion(
                          v => v.Value,
                          v => CustomerId.Of(v));
            
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            
            builder.Property(e => e.Email).HasMaxLength(100);
            builder.HasIndex(e => e.Email).IsUnique();

        }
    }
}
