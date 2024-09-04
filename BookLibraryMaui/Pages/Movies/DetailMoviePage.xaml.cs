using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;

namespace BookLibraryMaui.Pages.Movies;

public partial class DetailMoviePage : ContentPage
{
    private readonly MoviesRepository _moviesRepository;
    private readonly int _movieId;

    private Movie _movieDetail;

    public Movie MovieDetail
    {
        get => _movieDetail;
        set
        {
            _movieDetail = value;
            OnPropertyChanged();
        }
    }

    public DetailMoviePage(MoviesRepository moviesRepository, int movieId)
    {
        _movieId = movieId;
        _moviesRepository = moviesRepository;
        InitializeComponent();

        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        MovieDetail = await _moviesRepository.GetItemAsync(_movieId);
    }

    private async void EditBook_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddMoviePage(_moviesRepository, MovieDetail), true);
    }

    private async void DeleteBook_OnClicked(object sender, EventArgs e)
    {
        var deleteMovie = await DisplayAlert("Warning", "Are you sure you wish to delete this book?", "Yes", "No");

        if (!deleteMovie) return;

        await _moviesRepository.DeleteItemAsync(MovieDetail);
        await Navigation.PopToRootAsync(true);
    }
}