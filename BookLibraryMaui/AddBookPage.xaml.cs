using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui;

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

    private void ScanView_OnBarcodeDataRetrieved(string barcodeValue)
    {
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
        await Navigation.PopAsync();
    }

    private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        // Snap the value to the nearest multiple of 0.25
        var newValue = Math.Round(e.NewValue / 0.25) * 0.25;

        // Ensure it stays within the valid range     
        Book.Rating = Math.Clamp(newValue, 0, 5);
    }
}