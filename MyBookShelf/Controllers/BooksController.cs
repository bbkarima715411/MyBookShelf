using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBookShelf.BLL.Services;
using MyBookShelf.Models;
namespace MyBookShelf.UI.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly BookServices _service;
        private readonly UserManager<IdentityUser> _userManager;

        public BooksController(BookServices service, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public IActionResult Index(BookStatus? status)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var books = status.HasValue ? _service.GetBooksByStatus(userId, status.Value) : _service.GetAllBooks(userId);
            ViewBag.SelectedStatus = status;
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book) 
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            book.UserId = userId;
            ModelState.Remove(nameof(Book.UserId));

            if (ModelState.IsValid)
            {
                _service.AddBook(book);
                TempData["SuccessMessage"] = "Le livre a bien été ajouté à votre bibliothèque.";
                return RedirectToAction("Index");
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            _service.DeleteBook(userId, id);
            TempData["SuccessMessage"] = "Le livre a été supprimé de votre bibliothèque.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, BookStatus status)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            _service.UpdateStatus(userId, id, status);
            TempData["SuccessMessage"] = "Le statut de lecture a été mis à jour.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var book = _service.GetBookById(userId, id);
            if (book == null)
                return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            book.UserId = userId;
            ModelState.Remove(nameof(Book.UserId));

            if (ModelState.IsValid)
            {
                _service.UpdateBook(userId, book);
                TempData["SuccessMessage"] = "Les modifications ont été enregistrées.";
                return RedirectToAction("Index");
            }

            return View(book);
        }
    }
}
