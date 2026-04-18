using Payment.Domain.Exceptions;

namespace Payment.Domain.ValueObjects
{


    public record PaymentTransactionId
    {
        public Guid Value { get; }


        private PaymentTransactionId(Guid value) => Value = value;

        public static PaymentTransactionId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value, "value");

            if (value == Guid.Empty)
                throw new DomainException("PaymentId cannot be empty.");

            return new PaymentTransactionId(value);
        }

    }
}
