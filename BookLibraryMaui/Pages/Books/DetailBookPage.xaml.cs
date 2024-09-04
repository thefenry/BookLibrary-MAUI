using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui.Pages.Books;

public partial class DetailBookPage : ContentPage
{
    private readonly BooksRepository _booksRepository;
    private readonly int _bookId;

    private Book _bookDetail;
    private readonly BookSearchService _bookSearchService;

    public Book BookDetail
    {
        get => _bookDetail;
        set
        {
            _bookDetail = value;
            OnPropertyChanged();
        }
    }

    public DetailBookPage(BooksRepository booksRepository, int bookId, BookSearchService bookSearchService)
    {
        _bookId = bookId;
        _booksRepository = booksRepository;
        _bookSearchService = bookSearchService;
        InitializeComponent();

        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        BookDetail = await _booksRepository.GetItemAsync(_bookId);
    }

    private async void EditBook_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddBookPage(_booksRepository, BookDetail, _bookSearchService), true);
    }

    private async void DeleteBook_OnClicked(object sender, EventArgs e)
    {
        var deleteBook = await DisplayAlert("Warning", "Are you sure you wish to delete this book?", "Yes", "No");

        if (!deleteBook) return;

        await _booksRepository.DeleteItemAsync(BookDetail);
        await Navigation.PopToRootAsync(true);
    }
}