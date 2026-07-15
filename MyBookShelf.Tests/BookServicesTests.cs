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

        public List<Book> Search(string userId, string? title, string? author, BookStatus? status, bool favoritesOnly) =>
            _books.Where(b => b.UserId == userId)
                  .Where(b => string.IsNullOrWhiteSpace(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                  .Where(b => string.IsNullOrWhiteSpace(author) || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
                  .Where(b => !status.HasValue || b.Status == status.Value)
                  .Where(b => !favoritesOnly || b.IsFavorite)
                  .ToList();

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
            existing.IsFavorite = book.IsFavorite;
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

        public void UpdateFavorite(string userId, int id, bool isFavorite)
        {
            var existing = _books.SingleOrDefault(b => b.UserId == userId && b.Id == id);
            if (existing == null) return;
            existing.IsFavorite = isFavorite;
        }

        private static Book Clone(Book book) => new()
        {
            Id = book.Id,
            UserId = book.UserId,
            Title = book.Title,
            Author = book.Author,
            Status = book.Status,
            IsFavorite = book.IsFavorite,
            Rating = book.Rating,
            Comment = book.Comment
        };
    }

    private static Book NewBook(string userId, string title = "Test", string author = "Author",
        BookStatus status = BookStatus.ToRead, bool isFavorite = false, int? rating = null) => new()
    {
        UserId = userId,
        Title = title,
        Author = author,
        Status = status,
        IsFavorite = isFavorite,
        Rating = rating
    };

    [Fact]
    public void AddBook_WhenRatingIsValid_SavesBook()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        service.AddBook(NewBook("user-1", rating: 3));

        var saved = repo.GetAll("user-1").Single();
        Assert.Equal(3, saved.Rating);
    }

    [Fact]
    public void AddBook_WhenRatingIsBelowOne_Throws()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        Assert.Throws<ArgumentOutOfRangeException>(() => service.AddBook(NewBook("user-1", rating: 0)));
    }

    [Fact]
    public void AddBook_WhenRatingIsAboveFive_Throws()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        Assert.Throws<ArgumentOutOfRangeException>(() => service.AddBook(NewBook("user-1", rating: 6)));
    }

    [Fact]
    public void AddBook_WhenMarkedAsFavorite_SavesFavorite()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        service.AddBook(NewBook("user-1", isFavorite: true));

        var saved = repo.GetAll("user-1").Single();
        Assert.True(saved.IsFavorite);
    }

    [Fact]
    public void SetFavorite_TogglesOnlyForOwner()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        service.AddBook(NewBook("user-1"));
        var bookId = repo.GetAll("user-1").Single().Id;

        service.SetFavorite("user-2", bookId, true);
        Assert.False(repo.GetAll("user-1").Single().IsFavorite);

        service.SetFavorite("user-1", bookId, true);
        Assert.True(repo.GetAll("user-1").Single().IsFavorite);
    }

    [Fact]
    public void UpdateStatus_ChangesStatus()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        service.AddBook(NewBook("user-1", status: BookStatus.ToRead));
        var bookId = repo.GetAll("user-1").Single().Id;

        service.UpdateStatus("user-1", bookId, BookStatus.Abandoned);

        Assert.Equal(BookStatus.Abandoned, repo.GetAll("user-1").Single().Status);
    }

    [Fact]
    public void SearchBooks_ReturnsOnlyBooksOfRequestedUserAndMatchingFilters()
    {
        var repo = new FakeBookRepository();
        var service = new BookServices(repo);

        service.AddBook(NewBook("user-1", title: "Dune", author: "Frank Herbert", status: BookStatus.Read, isFavorite: true));
        service.AddBook(NewBook("user-1", title: "Fondation", author: "Isaac Asimov", status: BookStatus.ToRead));
        service.AddBook(NewBook("user-2", title: "Dune", author: "Frank Herbert", status: BookStatus.Read, isFavorite: true));

        var byTitle = service.SearchBooks("user-1", "dune", null, null, false);
        Assert.Single(byTitle);
        Assert.All(byTitle, b => Assert.Equal("user-1", b.UserId));

        var byAuthor = service.SearchBooks("user-1", null, "asimov", null, false);
        Assert.Single(byAuthor);
        Assert.Equal("Fondation", byAuthor.Single().Title);

        var byStatus = service.SearchBooks("user-1", null, null, BookStatus.Read, false);
        Assert.Single(byStatus);

        var favorites = service.SearchBooks("user-1", null, null, null, true);
        Assert.Single(favorites);
        Assert.True(favorites.Single().IsFavorite);
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
