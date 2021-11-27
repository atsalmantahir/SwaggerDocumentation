using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using SwaggerDocumentation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDocumentation.Controllers.v2
{

    [Route("api/v2.0/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class SearchController : ODataController
    {
        private readonly SwaggerDocumentationContext swaggerDocumentationContext;
        public SearchController(SwaggerDocumentationContext swaggerDocumentationContext) 
        {
            this.swaggerDocumentationContext = swaggerDocumentationContext
                ?? throw new ArgumentNullException(nameof(swaggerDocumentationContext));
        }

        [HttpGet]
        [ODataRoute("Books")]
        public IActionResult Get() 
        {
            return Ok(this.swaggerDocumentationContext.Book);
        }
    }
}
