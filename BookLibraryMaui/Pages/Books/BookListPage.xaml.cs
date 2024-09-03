using System.Collections.ObjectModel;
using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui.Pages.Books;

public partial class BookListPage : ContentPage
{
    private readonly BooksRepository _booksRepository;
    private string _currentSortDirection = "Title";

    private int _pageIndex = 0;
    private readonly BookSearchService _bookSearchService;
    private const int PageSize = 1000; // Set your page size

    public ObservableCollection<Book> Books { get; set; } = [];

    public BookListPage(BooksRepository booksRepository, BookSearchService bookSearchService)
    {
        _booksRepository = booksRepository;
        _bookSearchService = bookSearchService;
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        IsBusy = true;
        _pageIndex = 0;
        await PopulateBookList();

        IsBusy = false;

        base.OnNavigatedTo(args);
    }

    private async Task PopulateBookList(string searchText = null, bool isNewSearch = true)
    {
        var books = await _booksRepository.GetItemsAsync(searchText, _currentSortDirection, PageSize, _pageIndex);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (isNewSearch)
            {
                Books.Clear();
            }

            foreach (var book in books)
            {
                Books.Add(book);
            }
        });
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddBookPage(_booksRepository, null, _bookSearchService), true);
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
        _pageIndex = 0;
        await PopulateBookList();
    }

    private async void BookSearch_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue;
        _pageIndex = 0;
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
            await Navigation.PushAsync(new DetailBookPage(_booksRepository, currentSelection.Id, _bookSearchService), true);
        }
    }

    private async void OnRemainingItemsThresholdReached(object sender, EventArgs e)
    {
        // Check if there's already an operation running
        if (IsBusy)
            return;

        // Check if there are more items to load
        if (Books.Count < PageSize * _pageIndex)
        {
            return;
        }

        IsBusy = true;
        // Increment the page index
        _pageIndex++;

        // Load more items here and add them to your collection
        await PopulateBookList(isNewSearch: false);

        IsBusy = false;
    }
}