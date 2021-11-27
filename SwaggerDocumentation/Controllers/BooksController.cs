using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.OData.Query;
using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using SwaggerDocumentation.Data;
using SwaggerDocumentation.Entities;
using SwaggerDocumentation.Models;

namespace SwaggerDocumentation.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly SwaggerDocumentationContext _context;
        private readonly IMapper _mapper;

        public BooksController(SwaggerDocumentationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(Guid id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        /// <summary>
        /// Create a record of book
        /// </summary>
        /// <param name="bookCreation"></param>
        /// <returns>ActionResult of book record</returns>
        /// <remarks>
        /// Sample request (this request creates the record of book) \
        /// POST bookCreation \
        /// [ \
        ///     { \
        ///         "title": "Limitless", \
        ///         "desciption": "Best selling book of all time" \
        ///     } \
        /// ]
        /// </remarks>
        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookCreation bookCreation)
        {
            var book = _mapper.Map<Entities.Book>(bookCreation);
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(Guid id)
        {
            return _context.Book.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("search")]
        [Microsoft.AspNet.OData.EnableQuery]
        [System.Web.Http.Queryable(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.Select)]
        public IActionResult SearchBook()
        {
            return Ok(this._context.Book);
        }
    }
}
