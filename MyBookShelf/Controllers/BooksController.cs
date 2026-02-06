using Microsoft.AspNetCore.Mvc;
using MyBookShelf.Data;
using MyBookShelf.Models;

namespace MyBookShelf.Controllers
{
    public class BooksController : Controller
    {

        private readonly BookRepository _repository;

        public BooksController(BookRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            List<Book> books = _repository.GetAll();
            return View (books);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create (Book book)
        {  if (ModelState.IsValid)
            {
                _repository.Add(book);
                return RedirectToAction("Index");
            }
            return View(book);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id, BookStatus status)
        {
            _repository.UpdateStatus(id, status);
            return RedirectToAction("Index");
        }
    }
}
