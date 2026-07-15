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

        public List<Book> SearchBooks(string userId, string? title, string? author, BookStatus? status, bool favoritesOnly)
        {
            return _repository.Search(userId, title, author, status, favoritesOnly);
        }

        public Book? GetBookById(string userId, int id)
        {
            return _repository.GetById(userId, id);
        }

        /// <summary>
        /// Valide les regles metier d'un livre. Leve une exception si le livre est invalide.
        /// </summary>
        public static void ValidateBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Le titre du livre est obligatoire.", nameof(book));

            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("L'auteur du livre est obligatoire.", nameof(book));

            if (!System.Enum.IsDefined(typeof(BookStatus), book.Status))
                throw new ArgumentOutOfRangeException(nameof(book), "Le statut de lecture est invalide.");

            if (book.Rating.HasValue && (book.Rating.Value < 1 || book.Rating.Value > 5))
                throw new ArgumentOutOfRangeException(nameof(book), "La note doit etre comprise entre 1 et 5.");
        }

        public void AddBook(Book book)
        {
            ValidateBook(book);
            _repository.add(book);
        }

        public void UpdateBook(string userId, Book book)
        {
            ValidateBook(book);
            _repository.Update(userId, book);
        }

        public void DeleteBook(string userId, int id)
        {
            _repository.Delete(userId, id);
        }


        public void UpdateStatus(string userId, int id, BookStatus status)
        {
            if (!System.Enum.IsDefined(typeof(BookStatus), status))
                throw new ArgumentOutOfRangeException(nameof(status), "Le statut de lecture est invalide.");

            _repository.UpdateStatus(userId, id, status);
        }

        public void SetFavorite(string userId, int id, bool isFavorite)
        {
            _repository.UpdateFavorite(userId, id, isFavorite);
        }
    }
}
