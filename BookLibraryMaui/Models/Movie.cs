using SQLite;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BookLibraryMaui.Models
{
    [Table("Movies")]
    public class Movie : INotifyPropertyChanged
    {
        private string _title;
        private string _description;
        private string _genre;
        private string _movieType;
        private int? _year;
        private bool _isSteelBook;
        private double _rating;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value)
                {
                    return;
                }
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                {
                    return;
                }
                _description = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDescriptionVisible));
            }
        }

        public string Genre
        {
            get => _genre;
            set
            {
                if (_genre == value)
                {
                    return;
                }
                _genre = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsGenreVisible));
            }
        }

        public string MovieType
        {
            get => _movieType;
            set
            {
                if (_movieType == value)
                {
                    return;
                }
                _movieType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsMovieTypeVisible));
            }
        }

        public int? Year
        {
            get => _year;
            set
            {
                if (_year == value)
                {
                    return;
                }
                _year = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsYearVisible));
            }
        }

        public bool IsSteelBook
        {
            get => _isSteelBook;
            set
            {
                if (_isSteelBook == value)
                {
                    return;
                }
                _isSteelBook = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSteelBookVisible));
            }
        }

        public double Rating
        {
            get => _rating;
            set
            {
                if (_rating == value)
                {
                    return;
                }
                _rating = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsRatingVisible));
            }
        }

        // Visibility Properties
        public bool IsDescriptionVisible => !string.IsNullOrWhiteSpace(Description);
        public bool IsGenreVisible => !string.IsNullOrWhiteSpace(Genre);
        public bool IsMovieTypeVisible => !string.IsNullOrWhiteSpace(MovieType);
        public bool IsYearVisible => Year.HasValue;
        public bool IsSteelBookVisible => true; // Always visible for boolean
        public bool IsRatingVisible => Rating > 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
