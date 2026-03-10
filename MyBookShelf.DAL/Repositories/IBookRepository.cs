using MyBookShelf.Common1.Enum;
using MyBookShelf.Common1.Models;

namespace MyBookShelf.DAL.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAll();
        List<Book> GetByStatus(BookStatus status);
        Book? GetById(int id);
        void add(Book book);
        void Update(Book book);
        void Delete(int id);
        void UpdateStatus(int id, BookStatus status);
    }
}
