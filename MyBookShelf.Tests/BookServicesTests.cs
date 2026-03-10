using MyBookShelf.BLL.Services;
using MyBookShelf.Common1.Enum;
using MyBookShelf.Common1.Models;
using MyBookShelf.DAL.Repositories;

namespace MyBookShelf.Tests;

public class BookServicesTests
{
    private sealed class FakeBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;

        public List<Book> GetAll() => _books.ToList();

        public List<Book> GetByStatus(BookStatus status) => _books.Where(b => b.Status == status).ToList();

        public Book? GetById(int id) => _books.SingleOrDefault(b => b.Id == id);

        public void add(Book book)
        {
            book.Id = _nextId++;
            _books.Add(Clone(book));
        }

        public void Update(Book book)
        {
            var existing = _books.SingleOrDefault(b => b.Id == book.Id);
            if (existing == null) return;

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Status = book.Status;
            existing.Rating = book.Rating;
            existing.Comment = book.Comment;
        }

        public void Delete(int id)
        {
            _books.RemoveAll(b => b.Id == id);
        }

        public void UpdateStatus(int id, BookStatus status)
        {
            var existing = _books.SingleOrDefault(b => b.Id == id);
            if (existing == null) return;
            existing.Status = status;
        }

        private static Book Clone(Book book) => new()
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Status = book.Status,
            Rating = book.Rating,
            Comment = book.Comment
        };
    }

    [Fact]
    public void AddBook_WhenRatingIsOutOfRange_Throws()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        var book = new Book
        {
            Title = "Test",
            Author = "Author",
            Status = BookStatus.Read,
            Rating = 6
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => service.AddBook(book));
    }

    [Fact]
    public void UpdateBook_WhenRatingIsValid_DoesNotThrow()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        var book = new Book
        {
            Title = "Test",
            Author = "Author",
            Status = BookStatus.Read,
            Rating = 5,
            Comment = "Nice"
        };

        service.AddBook(book);

        var saved = repo.GetAll().Single();
        saved.Rating = 4;

        var exception = Record.Exception(() => service.UpdateBook(saved));
        Assert.Null(exception);
    }
}
