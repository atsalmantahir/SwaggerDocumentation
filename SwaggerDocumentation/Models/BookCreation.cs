using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDocumentation.Models
{
    /// <summary>
    /// A book with Title and Description
    /// </summary>
    public class BookCreation
    {
        /// <summary>
        /// Title of the book
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// Description of the book
        /// </summary>
        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
