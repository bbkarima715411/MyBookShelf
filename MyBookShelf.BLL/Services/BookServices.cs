using MyBookShelf.Common1.Enum;
using MyBookShelf.Common1.Models;
using MyBookShelf.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShelf.BLL.Services
{
    public class BookServices
    {
        private readonly IBookRepository _repository;

        public BookServices(IBookRepository repository)
        {
            _repository = repository;
        }

        public List<Book> GetAllBooks()
        {
            return _repository.GetAll();
        }

        public List<Book> GetBooksByStatus(BookStatus status)
        {
            return _repository.GetByStatus(status);
        }

        public Book? GetBookById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddBook(Book book)
        {
            if (book.Rating.HasValue && (book.Rating.Value < 1 || book.Rating.Value > 5))
                throw new ArgumentOutOfRangeException(nameof(book.Rating), "Rating must be between 1 and 5.");
            _repository.add(book);
        }

        public void UpdateBook(Book book)
        {
            if (book.Rating.HasValue && (book.Rating.Value < 1 || book.Rating.Value > 5))
                throw new ArgumentOutOfRangeException(nameof(book.Rating), "Rating must be between 1 and 5.");
            _repository.Update(book);
        }

        public void DeleteBook(int id)
        {
            _repository.Delete(id);
        }


        public void UpdateStatus(int id, BookStatus status)
        {
            _repository.UpdateStatus(id, status);
            
        }
    }
}
