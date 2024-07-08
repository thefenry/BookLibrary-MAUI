using BookLibraryMaui.Constants;
using BookLibraryMaui.Models;
using SQLite;

namespace BookLibraryMaui.DAL;

public class BooksRepository
{
    private SQLiteAsyncConnection _database;

    private async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(BooksDatabaseConstants.DatabasePath, BooksDatabaseConstants.Flags);
        var result = await _database.CreateTableAsync<Book>();
    }

    public async Task<List<Book>> GetItemsAsync(string searchText, string sortOption, int pageSize, int pageIndex)
    {
        await Init();
        AsyncTableQuery<Book> query = _database.Table<Book>();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(book =>
                book.Title.ToLower().Contains(searchText.ToLower()) ||
                book.Author.ToLower().Contains(searchText.ToLower()));
        }

        query = sortOption switch
        {
            "Author" => query.OrderBy(book => book.Author),
            "Series" => query.OrderBy(book => book.SeriesTitle),
            _ => query.OrderBy(book => book.Title)
        };
        
        // Implement pagination
        query = query.Skip(pageSize * pageIndex).Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<Book> GetItemAsync(int id)
    {
        await Init();
        return await _database.Table<Book>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(Book item)
    {
        await Init();
        if (item.Id != 0)
        {
            return await _database.UpdateAsync(item);
        }

        return await _database.InsertAsync(item);
    }

    public async Task<int> SaveItemsAsync(List<Book> items)
    {
        await Init();
        return await _database.InsertAllAsync(items);
    }

    public async Task<int> DeleteItemAsync(Book item)
    {
        await Init();
        return await _database.DeleteAsync(item);
    }
}