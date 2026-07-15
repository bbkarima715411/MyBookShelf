using Xunit;
using MyBookShelf.BLL.Services;
using MyBookShelf.DAL.Repositories;
using MyBookShelf.Models;

namespace MyBookShelf.Tests;

public class BookServicesTests
{
    private sealed class FakeBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;

        public List<Book> GetAll(string userId) => _books.Where(b => b.UserId == userId).ToList();

        public List<Book> GetByStatus(string userId, BookStatus status) => _books.Where(b => b.UserId == userId && b.Status == status).ToList();

        public Book? GetById(string userId, int id) => _books.SingleOrDefault(b => b.UserId == userId && b.Id == id);

        public void add(Book book)
        {
            book.Id = _nextId++;
            _books.Add(Clone(book));
        }

        public void Update(string userId, Book book)
        {
            var existing = _books.SingleOrDefault(b => b.UserId == userId && b.Id == book.Id);
            if (existing == null) return;

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Status = book.Status;
            existing.Rating = book.Rating;
            existing.Comment = book.Comment;
        }

        public void Delete(string userId, int id)
        {
            _books.RemoveAll(b => b.UserId == userId && b.Id == id);
        }

        public void UpdateStatus(string userId, int id, BookStatus status)
        {
            var existing = _books.SingleOrDefault(b => b.UserId == userId && b.Id == id);
            if (existing == null) return;
            existing.Status = status;
        }

        private static Book Clone(Book book) => new()
        {
            Id = book.Id,
            UserId = book.UserId,
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
        const string userId = "user-1";

        var book = new Book
        {
            UserId = userId,
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
        const string userId = "user-1";

        var book = new Book
        {
            UserId = userId,
            Title = "Test",
            Author = "Author",
            Status = BookStatus.Read,
            Rating = 5,
            Comment = "Nice"
        };

        service.AddBook(book);

        var saved = repo.GetAll(userId).Single();
        saved.Rating = 4;

        var exception = Record.Exception(() => service.UpdateBook(userId, saved));
        Assert.Null(exception);
    }
}
