using System.ComponentModel.DataAnnotations;
using MyBookShelf.Models;

namespace MyBookShelf.UI.ViewModels
{
    public class BookFilterViewModel
    {
        [Display(Name = "Titre")]
        public string? Title { get; set; }

        [Display(Name = "Auteur")]
        public string? Author { get; set; }

        [Display(Name = "Statut")]
        public BookStatus? Status { get; set; }

        [Display(Name = "Favoris uniquement")]
        public bool FavoritesOnly { get; set; }

        public bool HasActiveFilters =>
            !string.IsNullOrWhiteSpace(Title)
            || !string.IsNullOrWhiteSpace(Author)
            || Status.HasValue
            || FavoritesOnly;
    }
}
