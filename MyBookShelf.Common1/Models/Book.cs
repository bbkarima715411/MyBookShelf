using MyBookShelf.Common1.Enum;
using System.ComponentModel.DataAnnotations;

namespace MyBookShelf.Common1.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Author { get; set; }
        public BookStatus Status { get; set; }
        [Range(1, 5)]
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }
}

