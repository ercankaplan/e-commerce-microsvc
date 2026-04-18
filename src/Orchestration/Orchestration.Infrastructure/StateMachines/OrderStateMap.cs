using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.StateMachines
{
    public class OrderStateMap : SagaClassMap<OrderState>
    {
        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState);

            entity.Property(x => x.OrderDate);
            entity.Property(x => x.OrderId);
            entity.Property(x => x.OrderTotal);
            entity.Property(x => x.UserId);
            entity.Property(x => x.CustomerEmail);
            entity.Property(x => x.PaymentRequestId);
            entity.Property(x => x.PaymentTransactionId);
        }
    }
}
