using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;
using System.Collections.ObjectModel;

namespace BookLibraryMaui.Pages.Movies;

public partial class AddMoviePage : ContentPage
{
    private readonly MoviesRepository _moviesRepository;

    public Movie MovieDetail { get; set; }
    public ObservableCollection<MediaTypeOption> MediaTypeOptions { get; set; }

    public AddMoviePage(MoviesRepository moviesRepository, Movie movieDetail)
    {
        if (movieDetail == null)
        {
            Title = "Add Movie";
            MovieDetail = new Movie();
        }
        else
        {
            Title = "Edit Movie";
            MovieDetail = movieDetail;
        }

        _moviesRepository = moviesRepository;

        // Initialize media type options
        var availableMediaTypes = new[] { "Blu-ray", "DVD", "4K", "Digital", "VHS" };
        MediaTypeOptions = new ObservableCollection<MediaTypeOption>(
            availableMediaTypes.Select(mt => new MediaTypeOption
            {
                Name = mt,
                IsSelected = MovieDetail.MovieType?.Split(',').Contains(mt) ?? false
            }));

        InitializeComponent();
        BindingContext = this;
    }

    private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        // Snap the value to the nearest multiple of 0.25
        var newValue = Math.Round(e.NewValue / 0.25) * 0.25;

        // Ensure it stays within the valid range
        MovieDetail.Rating = Math.Clamp(newValue, 0, 5);
    }

    private async void SaveButton_OnClicked(object sender, EventArgs e)
    {
        // Validate the title
        if (string.IsNullOrWhiteSpace(MovieDetail.Title))
        {
            await DisplayAlert("Validation Error", "Movie title is required.", "OK");
            return;
        }

        // Update MovieType with selected media types
        MovieDetail.MovieType = string.Join(",", MediaTypeOptions.Where(mt => mt.IsSelected).Select(mt => mt.Name));

        // Save the movie to the repository
        await _moviesRepository.SaveItemAsync(MovieDetail);
        await Navigation.PopAsync(true);
    }

}

public class MediaTypeOption
{
    public string Name { get; set; }
    public bool IsSelected { get; set; }
}
