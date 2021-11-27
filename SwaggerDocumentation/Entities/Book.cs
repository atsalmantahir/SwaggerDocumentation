using System;
using System.ComponentModel.DataAnnotations;

namespace SwaggerDocumentation.Entities
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        
        [MaxLength(2500)]
        public string Description { get; set; }

        public int? AmountOfPages { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}
