using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui.Pages.Movies;

public partial class AddMoviePage : ContentPage
{
    private readonly MoviesRepository _moviesRepository;

    public Movie MovieDetail { get; set; }

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
        await _moviesRepository.SaveItemAsync(MovieDetail);
        await Navigation.PopAsync(true);
    }
}