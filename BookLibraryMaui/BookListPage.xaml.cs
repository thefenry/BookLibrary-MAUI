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
        var books = await _booksRepository.GetItemsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        });
        
        base.OnNavigatedTo(args);
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddBookPage(_booksRepository));
    }

    private async void BookList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Book selectedItem)
        {
            await Navigation.PushAsync(new DetailBookPage(_booksRepository, selectedItem.Id));
        }
    }
}