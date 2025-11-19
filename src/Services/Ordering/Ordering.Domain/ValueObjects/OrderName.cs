using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record OrderName
    {
        public const int DefaultLength = 50;
        public string Value { get; }

        private OrderName(string value) => Value = value;

        public static OrderName Of(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, "value");
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length,DefaultLength);

            return new OrderName(value);
        }
    }
}
