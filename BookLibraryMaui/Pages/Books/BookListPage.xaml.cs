using System.Collections.ObjectModel;
using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui.Pages.Books;

public partial class BookListPage : ContentPage
{
    private readonly BooksRepository _booksRepository;
    private string _currentSortDirection = "Title";

    public ObservableCollection<Book> Books { get; set; } = [];

    public BookListPage(BooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        IsBusy = true;

        await PopulateBookList();

        IsBusy = false;

        base.OnNavigatedTo(args);
    }

    private async Task PopulateBookList(string searchText = null)
    {
        var books = await _booksRepository.GetItemsAsync(searchText, _currentSortDirection);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        });
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddBookPage(_booksRepository, null), true);
    }

    private async void Sort_OnClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Sort by:", "Cancel", null, "Author", "Title", "Series");

        if (action == "Cancel")
        {
            return;
        }

        _currentSortDirection = action;
        SortLabel.Text = $"Sorted by: {_currentSortDirection}";
        await PopulateBookList();
    }

    private async void BookSearch_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        await PopulateBookList(searchText);
    }

    private void Search_OnClicked(object sender, EventArgs e)
    {
        BookSearch.IsVisible = !BookSearch.IsVisible;
    }

    private async void BookList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Book currentSelection)
        {
            await Navigation.PushAsync(new DetailBookPage(_booksRepository, currentSelection.Id), true);
        }
    }
}