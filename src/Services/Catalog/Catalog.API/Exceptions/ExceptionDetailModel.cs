using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Exceptions
{
    public class ExceptionDetailModel()
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
    }
}
