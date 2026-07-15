using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBookShelf.BLL.Services;
using MyBookShelf.Models;
using MyBookShelf.UI.ViewModels;
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

        public IActionResult Index(BookFilterViewModel filter)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            filter ??= new BookFilterViewModel();

            var books = _service.SearchBooks(userId, filter.Title, filter.Author, filter.Status, filter.FavoritesOnly);

            var model = new BookListViewModel
            {
                Books = books,
                Filter = filter
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new BookCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookCreateViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            if (!ModelState.IsValid)
                return View(model);

            var book = new Book
            {
                UserId = userId,
                Title = model.Title.Trim(),
                Author = model.Author.Trim(),
                Status = model.Status,
                IsFavorite = model.IsFavorite,
                Rating = model.Rating,
                Comment = string.IsNullOrWhiteSpace(model.Comment) ? null : model.Comment.Trim()
            };

            _service.AddBook(book);
            TempData["SuccessMessage"] = "Le livre a bien été ajouté à votre bibliothèque.";
            return RedirectToAction("Index");
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

            var model = new BookEditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Status = book.Status,
                IsFavorite = book.IsFavorite,
                Rating = book.Rating,
                Comment = book.Comment
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookEditViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            if (!ModelState.IsValid)
                return View(model);

            var existing = _service.GetBookById(userId, model.Id);
            if (existing == null)
                return NotFound();

            var book = new Book
            {
                Id = model.Id,
                UserId = userId,
                Title = model.Title.Trim(),
                Author = model.Author.Trim(),
                Status = model.Status,
                IsFavorite = model.IsFavorite,
                Rating = model.Rating,
                Comment = string.IsNullOrWhiteSpace(model.Comment) ? null : model.Comment.Trim()
            };

            _service.UpdateBook(userId, book);
            TempData["SuccessMessage"] = "Les modifications ont été enregistrées.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleFavorite(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var book = _service.GetBookById(userId, id);
            if (book == null)
                return NotFound();

            _service.SetFavorite(userId, id, !book.IsFavorite);
            TempData["SuccessMessage"] = book.IsFavorite
                ? "Le livre a été retiré de vos favoris."
                : "Le livre a été ajouté à vos favoris.";
            return RedirectToAction("Index");
        }
    }
}
