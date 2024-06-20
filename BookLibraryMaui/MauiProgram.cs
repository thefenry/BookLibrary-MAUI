using BookLibraryMaui.DAL;
using BookLibraryMaui.Pages.Books;
using BookLibraryMaui.Pages.Movies;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace BookLibraryMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FaSolids");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            
            builder.Services.AddSingleton<BookListPage>();
            builder.Services.AddSingleton<MovieListPage>();
            
            builder.Services.AddSingleton<BooksRepository>();
            builder.Services.AddSingleton<MoviesRepository>();
         
            return builder.Build();
        }
    }
}
