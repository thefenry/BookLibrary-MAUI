using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui.Pages.Books;

public partial class AddBookPage : ContentPage
{
    private readonly BooksRepository _booksRepository;
    
    public Book Book { get; set; }

    public bool IsScanning { get; set; }


    public AddBookPage(BooksRepository booksRepository, Book bookDetail)
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
        InitializeComponent();
        BindingContext = this;
        ScanView.BarcodeDataRetrieved += ScanView_OnBarcodeDataRetrieved;
    }

    private async void ScanView_OnBarcodeDataRetrieved(string barcodeValue)
    {
        if (!string.IsNullOrWhiteSpace(barcodeValue))
        {
            var googleBookRepository = new GoogleBookRepository();

            //Task.Run(async () => Book = await googleBookRepository.GetBookAsync(barcodeValue));
            var book = await googleBookRepository.GetBookAsync(barcodeValue);
            Book = book;
            OnPropertyChanged(nameof(Book));
        }

        ScanView.StopScanning();
        IsScanning = false;
        OnPropertyChanged(nameof(IsScanning));
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
}