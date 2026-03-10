using Microsoft.AspNetCore.Mvc;
using MyBookShelf.BLL.Services;
using MyBookShelf.Common1.Enum;
using MyBookShelf.Common1.Models; 
namespace MyBookShelf.UI.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookServices _service;

        public BooksController(BookServices service)
        {
            _service = service;
        }

        public IActionResult Index(BookStatus? status)
        {
            var books = status.HasValue ? _service.GetBooksByStatus(status.Value) : _service.GetAllBooks();
            ViewBag.SelectedStatus = status;
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book) 
        {
            if (ModelState.IsValid)
            {
                _service.AddBook(book);     
                return RedirectToAction("Index");
            }

            return View(book);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _service.DeleteBook(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, BookStatus status)
        {
            _service.UpdateStatus(id, status);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var book = _service.GetBookById(id);
            if (book == null)
                return NotFound();
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateBook(book);
                return RedirectToAction("Index");
            }

            return View(book);
        }
    }
}
