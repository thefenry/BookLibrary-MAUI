using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Text;
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
    private readonly IFilePicker _filePicker;
    private readonly BooksRepository _booksRepository;
    private readonly MoviesRepository _moviesRepository;

    public SettingsPage(IFileSaver fileSaver, IFilePicker filePicker, BooksRepository booksRepository, MoviesRepository moviesRepository)
    {
        InitializeComponent();
        _fileSaver = fileSaver;
        _filePicker = filePicker;
        _booksRepository = booksRepository;
        _moviesRepository = moviesRepository;
    }

    private async void ExportButton_OnClicked(object sender, EventArgs e)
    {
        var allBooks = await _booksRepository.GetItemsAsync(null, null, 0, 0);
        //var allMovies = await _moviesRepository.GetItemsAsync(null, null);

        var exportObject = new ExportObject
        {
            Books = allBooks,
            //Movies = allMovies
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

    private async void ImportButton_OnClicked(object sender, EventArgs e)
    {
        try
        {
            await ImportButton_OnClickedAsync(sender, e);
        }
        catch (Exception ex)
        {
            // handle exception
        }
    }

    private async Task ImportButton_OnClickedAsync(object sender, EventArgs e)
    {
        try
        {
            var result = await _filePicker.PickAsync();
            if (result != null && result.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
            {
                await using var stream = await result.OpenReadAsync();
                using var reader = new StreamReader(stream);
                var jsonString = await reader.ReadToEndAsync();

                var importData = JsonSerializer.Deserialize<ExportObject>(jsonString);

                var bookInsertTask = _booksRepository.SaveItemsAsync(importData.Books);
                var movieInsertTask = _moviesRepository.SaveItemsAsync(importData.Movies);

                await Task.WhenAll(bookInsertTask, movieInsertTask);
            }
            else
            {
                //TODO: Handle Exception
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}