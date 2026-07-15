using System.ComponentModel.DataAnnotations;
using MyBookShelf.Models;

namespace MyBookShelf.UI.ViewModels
{
    public class BookCreateViewModel
    {
        [Required(ErrorMessage = "Veuillez indiquer le titre du livre.")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères.")]
        [Display(Name = "Titre")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez indiquer le nom de l'auteur.")]
        [StringLength(200, ErrorMessage = "Le nom de l'auteur ne peut pas dépasser 200 caractères.")]
        [Display(Name = "Auteur")]
        public string Author { get; set; } = string.Empty;

        [Display(Name = "Statut de lecture")]
        public BookStatus Status { get; set; } = BookStatus.ToRead;

        [Display(Name = "Favori")]
        public bool IsFavorite { get; set; }

        [Range(1, 5, ErrorMessage = "La note doit être comprise entre 1 et 5.")]
        [Display(Name = "Note (1 à 5)")]
        public int? Rating { get; set; }

        [StringLength(2000, ErrorMessage = "Votre note personnelle ne peut pas dépasser 2000 caractères.")]
        [Display(Name = "Note personnelle")]
        public string? Comment { get; set; }
    }
}
