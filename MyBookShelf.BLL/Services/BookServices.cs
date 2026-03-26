using MyBookShelf.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBookShelf.Models;

namespace MyBookShelf.BLL.Services
{
    public class BookServices
    {
        private readonly IBookRepository _repository;

        public BookServices(IBookRepository repository)
        {
            _repository = repository;
        }

        public List<Book> GetAllBooks(string userId)
        {
            return _repository.GetAll(userId);
        }

        public List<Book> GetBooksByStatus(string userId, BookStatus status)
        {
            return _repository.GetByStatus(userId, status);
        }

        public Book? GetBookById(string userId, int id)
        {
            return _repository.GetById(userId, id);
        }

        public void AddBook(Book book)
        {
            if (book.Rating.HasValue && (book.Rating.Value < 1 || book.Rating.Value > 5))
                throw new ArgumentOutOfRangeException(nameof(book.Rating), "Rating must be between 1 and 5.");
            _repository.add(book);
        }

        public void UpdateBook(string userId, Book book)
        {
            if (book.Rating.HasValue && (book.Rating.Value < 1 || book.Rating.Value > 5))
                throw new ArgumentOutOfRangeException(nameof(book.Rating), "Rating must be between 1 and 5.");
            _repository.Update(userId, book);
        }

        public void DeleteBook(string userId, int id)
        {
            _repository.Delete(userId, id);
        }


        public void UpdateStatus(string userId, int id, BookStatus status)
        {
            _repository.UpdateStatus(userId, id, status);
            
        }
    }
}
