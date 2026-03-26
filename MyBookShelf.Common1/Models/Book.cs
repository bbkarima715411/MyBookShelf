using System.ComponentModel.DataAnnotations;


namespace MyBookShelf.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public required string UserId { get; set; }

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

