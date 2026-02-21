using MyBookShelf.Common1.Enum;

namespace MyBookShelf.Common1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public BookStatus Status { get; set; }
    }
}

