using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SwaggerDocumentation.Entities;

namespace SwaggerDocumentation.Data
{
    public class SwaggerDocumentationContext : DbContext
    {
        public SwaggerDocumentationContext (DbContextOptions<SwaggerDocumentationContext> options)
            : base(options)
        {
        }

        public DbSet<SwaggerDocumentation.Entities.Book> Book { get; set; }
    }
}
