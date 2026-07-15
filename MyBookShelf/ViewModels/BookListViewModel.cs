using MyBookShelf.Models;

namespace MyBookShelf.UI.ViewModels
{
    public class BookListViewModel
    {
        public List<Book> Books { get; set; } = new();

        public BookFilterViewModel Filter { get; set; } = new();

        public bool IsEmptyLibrary => Books.Count == 0 && !Filter.HasActiveFilters;

        public bool IsEmptySearchResult => Books.Count == 0 && Filter.HasActiveFilters;
    }
}
