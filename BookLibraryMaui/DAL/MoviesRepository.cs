using BookLibraryMaui.Constants;
using BookLibraryMaui.Models;
using SQLite;

namespace BookLibraryMaui.DAL;

public class MoviesRepository
{
    private SQLiteAsyncConnection _database;

    private async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(MoviesDatabaseConstants.DatabasePath, MoviesDatabaseConstants.Flags);
        var result = await _database.CreateTableAsync<Movie>();
    }

    public async Task<List<Movie>> GetItemsAsync(string searchText, string sortOption)
    {
        await Init();
        AsyncTableQuery<Movie> query = _database.Table<Movie>();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(movie => movie.Title.ToLower().Contains(searchText.ToLower()));
        }

        query = sortOption switch
        {
            "Year" => query.OrderBy(movie => movie.Year),
            "Genre" => query.OrderBy(movie => movie.Genre),
            _ => query.OrderBy(movie => movie.Title)
        };

        return await query.ToListAsync();
    }

    public async Task<Movie> GetItemAsync(int id)
    {
        await Init();
        return await _database.Table<Movie>().Where(movie => movie.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(Movie item)
    {
        await Init();
        if (item.Id != 0)
        {
            return await _database.UpdateAsync(item);
        }

        return await _database.InsertAsync(item);
    }

    public async Task<int> DeleteItemAsync(Movie item)
    {
        await Init();
        return await _database.DeleteAsync(item);
    }
}