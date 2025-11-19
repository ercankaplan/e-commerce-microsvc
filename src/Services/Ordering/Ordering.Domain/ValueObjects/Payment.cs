using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record Payment
    {
        public string? CardName { get; } = default!;

        public string CardNumber { get;} = default!;

        public string Expiration { get; } = default!;

        public string CVV { get; } = default!;    

        public int PaymentMethod { get; } = default!;

        protected Payment()
        {
            
        }

        private Payment(string cardName, string cardNumber, string expiration, string CVV, int paymentMethod)
        { 
            CardName = cardName;
            CardNumber = cardNumber;   
            Expiration = expiration;   
            this.CVV = CVV; 
            PaymentMethod = paymentMethod; 

        }

        public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardName, nameof(cardName));
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber, nameof(cardNumber));
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv, nameof(cvv));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 4);
            ArgumentException.ThrowIfNullOrWhiteSpace(expiration, nameof(expiration));
            //check expiration format MM/YY
            if (!System.Text.RegularExpressions.Regex.IsMatch(expiration, @"^(0[1-9]|1[0-2])\/?([0-9]{2})$"))
            {
                throw new ArgumentException("Expiration date is not in the correct format MM/YY", nameof(expiration));
            }
            //Compare today's date, convert MM/YY to DateTime
            DateTime lastDateofExpiration = new DateTime(2000 + int.Parse(expiration.Split("/").Last()),int.Parse(expiration.Split("/").First()),1).AddMonths(1).AddDays(-1);

            if (DateTime.Today > lastDateofExpiration)
            {
                throw new ArgumentException("Card is expired");
            }



            return new Payment(cardName,cardNumber,expiration, cvv, paymentMethod);

        }
    }
}
