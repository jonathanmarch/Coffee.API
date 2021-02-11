using System.Collections.Generic;

namespace Coffee.API.Models
{
    public class ValidationErrorsDto
    {
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
    }
}
