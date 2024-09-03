using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui.Pages.Books;

public partial class AddBookPage : ContentPage
{
    private readonly BooksRepository _booksRepository;
    private readonly BookSearchService _bookSearchService;

    public Book Book { get; set; }

    public bool IsScanning { get; set; }

    public AddBookPage(BooksRepository booksRepository, Book bookDetail, BookSearchService bookSearchService)
    {
        if (bookDetail == null)
        {
            Title = "Add Book";
            Book = new Book();
        }
        else
        {
            Title = "Edit Book";
            Book = bookDetail;
        }

        _booksRepository = booksRepository;
        _bookSearchService = bookSearchService;
        InitializeComponent();
        BindingContext = this;
        ScanView.BarcodeDataRetrieved += ScanView_OnBarcodeDataRetrieved;
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

    private void Scan_OnClicked(object sender, EventArgs env)
    {
        ScanView.StartScanning();
        IsScanning = true;
        OnPropertyChanged(nameof(IsScanning));
    }

    private async void SaveButton_OnClicked(object sender, EventArgs e)
    {
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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ScanView.BarcodeDataRetrieved -= ScanView_OnBarcodeDataRetrieved;
    }
}
