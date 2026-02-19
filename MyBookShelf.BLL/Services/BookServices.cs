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
        private readonly BookRepository _repository;

        public BookServices(BookRepository repository)
        {
            _repository = repository;
        }

        public List<Book> GetAllBooks()
        {
            return _repository.GetAll();
        }

        public void AddBook(Book book)
        {
            _repository.Add(book);
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
