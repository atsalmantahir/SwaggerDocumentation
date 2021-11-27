using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwaggerDocumentation.Data;
using SwaggerDocumentation.Entities;
using SwaggerDocumentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDocumentation.Controllers.v2
{
    [Route("api/v2.0/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly SwaggerDocumentationContext _context;

        public BooksController(SwaggerDocumentationContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            return await _context.Book.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(Guid id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet]
        [Route("search")]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(this._context.Book);
        }
    }
}
