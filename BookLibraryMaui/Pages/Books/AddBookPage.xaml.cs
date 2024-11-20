using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;
using BookLibraryMaui.Pages.Shared;

namespace BookLibraryMaui.Pages.Books;

public partial class AddBookPage : ContentPage
{
    private readonly BooksRepository _booksRepository;
    private readonly BookSearchService _bookSearchService;

    public Book Book { get; set; }

    public bool IsScanning { get; set; }

    private string _pageTitle;
    public string PageTitle
    {
        get => _pageTitle;
        set
        {
            if (_pageTitle == value) return;
            _pageTitle = value;
            OnPropertyChanged();
        }
    }

    public AddBookPage(BooksRepository booksRepository, Book bookDetail, BookSearchService bookSearchService)
    {
        if (bookDetail == null)
        {
            PageTitle = "Add Book";
            Book = new Book();
        }
        else
        {
            PageTitle = "Edit Book";
            Book = bookDetail;
        }

        _booksRepository = booksRepository;
        _bookSearchService = bookSearchService;
        InitializeComponent();
        BindingContext = this;
    }

    private async void ScanView_OnBarcodeDataRetrieved(string barcodeValue)
    {
        try
        {
            Console.WriteLine($"Scanned {barcodeValue}");

            if (!string.IsNullOrWhiteSpace(barcodeValue))
            {
                var book = await _bookSearchService.GetBookAsync(barcodeValue);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Book = book;
                    OnPropertyChanged(nameof(Book));
                });
            }

            IsScanning = false;
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                OnPropertyChanged(nameof(IsScanning));
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async void Scan_OnClicked(object sender, EventArgs env)
    {
        var scannerPage = new ScannerPage();
        await Navigation.PushAsync(scannerPage);

        var scanResults = await scannerPage.ScanResult;

        if (!string.IsNullOrWhiteSpace(scanResults))
        {
           ScanView_OnBarcodeDataRetrieved(scanResults);
        }
    }

    private async void SaveButton_OnClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Book.Title))
        {
            await DisplayAlert("Validation Error", "The book title cannot be empty.", "OK");
            return;
        }

        await _booksRepository.SaveItemAsync(Book);
        await Navigation.PopAsync(true);
    }

    private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        // Snap the value to the nearest multiple of 0.25
        var newValue = Math.Round(e.NewValue / 0.25) * 0.25;

        // Ensure it stays within the valid range     
        Book.Rating = Math.Clamp(newValue, 0, 5);
    }
}
