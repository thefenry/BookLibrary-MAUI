using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using SQLite;

namespace BookLibraryMaui.Models;

[Table("Books")]
public class Book : INotifyPropertyChanged
{
    private string _title;
    private string _author;
    private string _description;
    private string _seriesTitle;
    private string _genre;
    private string _category;
    private bool _isEBook;
    private double _rating;

    [JsonIgnore]
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Required]
    public string Title
    {
        get => _title;
        set
        {
            if (_title == value) return;
            _title = value;
            // Capitalize each word
            _title = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value?.ToLower() ?? string.Empty);
            OnPropertyChanged();
        }
    }

    [Required]
    public string Author
    {
        get => _author;
        set
        {
            if (_author == value) return;
            _author = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsAuthorVisible));
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (_description == value) return;
            _description = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsDescriptionVisible));
        }
    }

    [JsonPropertyName("Series Title")]
    public string SeriesTitle
    {
        get => _seriesTitle;
        set
        {
            if (_seriesTitle == value) return;
            _seriesTitle = value;
            // Capitalize each word
            _seriesTitle = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value?.ToLower() ?? string.Empty);
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsSeriesTitleVisible));
        }
    }

    public string Genre
    {
        get => _genre;
        set
        {
            if (_genre == value) return;
            _genre = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsGenreVisible));
        }
    }

    public string Category
    {
        get => _category;
        set
        {
            if (_category == value) return;
            _category = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsCategoryVisible));
        }
    }

    public bool IsEBook
    {
        get => _isEBook;
        set
        {
            if (_isEBook == value) return;
            _isEBook = value;
            OnPropertyChanged();
        }
    }

    public double Rating
    {
        get => _rating;
        set
        {
            if (_rating == value) return;
            _rating = value;
            OnPropertyChanged();
        }
    }

    // Visibility Properties
    public bool IsAuthorVisible => !string.IsNullOrWhiteSpace(Author);
    public bool IsDescriptionVisible => !string.IsNullOrWhiteSpace(Description);
    public bool IsSeriesTitleVisible => !string.IsNullOrWhiteSpace(SeriesTitle);
    public bool IsGenreVisible => !string.IsNullOrWhiteSpace(Genre);
    public bool IsCategoryVisible => !string.IsNullOrWhiteSpace(Category);

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
