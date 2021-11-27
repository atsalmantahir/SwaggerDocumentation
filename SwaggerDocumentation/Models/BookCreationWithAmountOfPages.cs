using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDocumentation.Models
{
    public class BookCreationWithAmountOfPages : BookCreation
    {
        public int AmountOfPages { get; set; }
    }
}
