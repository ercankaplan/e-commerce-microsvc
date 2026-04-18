using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base($"Domain Excepiton: \"{message}\" throws from Domain Layer.") { }
    }
}
