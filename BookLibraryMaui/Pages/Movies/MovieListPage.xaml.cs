using BookLibraryMaui.DAL;
using BookLibraryMaui.Models;
using System.Collections.ObjectModel;

namespace BookLibraryMaui.Pages.Movies;

public partial class MovieListPage : ContentPage
{
    private readonly MoviesRepository _moviesRepository;
    private string _currentSortDirection = "Title";

    public ObservableCollection<Movie> Movies { get; set; } = [];

    public MovieListPage(MoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        IsBusy = true;

        await PopulateMovieList();

        IsBusy = false;
        base.OnNavigatedTo(args);
    }

    private async Task PopulateMovieList(string searchText = null)
    {
        var movies = await _moviesRepository.GetItemsAsync(searchText, _currentSortDirection);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Movies.Clear();
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }
        });
    }

    private async void Sort_OnClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Sort by:", "Cancel", null, "Title", "Year", "Genre");

        if (action == "Cancel")
        {
            return;
        }

        _currentSortDirection = action;
        SortLabel.Text = $"Sorted by: {_currentSortDirection}";
        await PopulateMovieList();
    }

    private void Search_OnClicked(object sender, EventArgs e)
    {
        MovieSearch.IsVisible = !MovieSearch.IsVisible;
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddMoviePage(_moviesRepository, null), true);
    }

    private async void MovieSearch_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;

        await PopulateMovieList(searchText);
    }
    
    private async void MovieList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie currentSelection)
        {
            await Navigation.PushAsync(new DetailMoviePage(_moviesRepository, currentSelection.Id), true);
        }
    }
}