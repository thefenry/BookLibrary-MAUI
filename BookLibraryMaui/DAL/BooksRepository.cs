using BookLibraryMaui.Constants;
using BookLibraryMaui.Models;
using SQLite;

namespace BookLibraryMaui.DAL;

public class BooksRepository
{
    private SQLiteAsyncConnection? _database;

    async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(BooksDatabaseConstants.DatabasePath, BooksDatabaseConstants.Flags);
        var result = await _database.CreateTableAsync<Book>();
    }
    
    public async Task<List<Book>> GetItemsAsync()
    {
        await Init();
        return await _database.Table<Book>().ToListAsync();
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

    public async Task<int> DeleteItemAsync(Book item)
    {
        await Init();
        return await _database.DeleteAsync(item);
    }
}