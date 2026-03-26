using MyBookShelf.Models;

namespace MyBookShelf.DAL.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAll(string userId);
        List<Book> GetByStatus(string userId, BookStatus status);
        Book? GetById(string userId, int id);
        void add(Book book);
        void Update(string userId, Book book);
        void Delete(string userId, int id);
        void UpdateStatus(string userId, int id, BookStatus status);
    }
}
