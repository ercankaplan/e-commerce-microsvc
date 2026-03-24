using BuildingBlocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    internal class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id) : base($"Order with ID {id} not found.",id)
        {
        }
    }
}
