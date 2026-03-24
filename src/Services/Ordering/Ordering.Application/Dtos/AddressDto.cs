using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Dtos
{
    public record AddressDto(string FirstName, string LastName, string Email, string AddressLine, string Country, string State, string ZipCode);

}
