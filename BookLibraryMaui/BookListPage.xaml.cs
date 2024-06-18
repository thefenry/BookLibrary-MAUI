using System.Collections.ObjectModel;
using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui;

public partial class BookListPage : ContentPage
{
    private readonly BooksRepository _booksRepository;

    public ObservableCollection<Book> Books { get; set; } = new();

    public BookListPage(BooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var books = await _booksRepository.GetItemsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        });
    }
}