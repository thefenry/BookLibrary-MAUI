using BookLibraryMaui.DAL;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Text;
using BookLibraryMaui.Models;
using System.Text.Json;

namespace BookLibraryMaui.Pages.Settings;

public class ExportObject
{
    public List<Book> Books { get; set; }
    public List<Movie> Movies { get; set; }
}

public partial class SettingsPage : ContentPage
{
    private readonly IFileSaver _fileSaver;
    private readonly BooksRepository _booksRepository;
    private readonly MoviesRepository _moviesRepository;

    public SettingsPage(IFileSaver fileSaver, BooksRepository booksRepository, MoviesRepository moviesRepository)
    {
        InitializeComponent();
        _fileSaver = fileSaver;
        _booksRepository = booksRepository;
        _moviesRepository = moviesRepository;
    }

    private async void ExportButton_OnClicked(object sender, EventArgs e)
    {
        var allBooks = await _booksRepository.GetItemsAsync(null, null);
        var allMovies = await _moviesRepository.GetItemsAsync(null, null);

        var exportObject = new ExportObject
        {
            Books = allBooks,
            Movies = allMovies
        };

        var jsonString = JsonSerializer.Serialize(exportObject);
        using var stream = new MemoryStream(Encoding.Default.GetBytes(jsonString));

        MyProgressBar.IsVisible = true;

        var saverProgress = new Progress<double>(percentage => MyProgressBar.Progress = percentage);
        var fileSaverResult = await _fileSaver.SaveAsync("libraryBackup.json", stream, saverProgress);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show();
        }
        else
        {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show();
        }

        MyProgressBar.IsVisible = false;
    }

    private void ImportButton_OnClicked(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}