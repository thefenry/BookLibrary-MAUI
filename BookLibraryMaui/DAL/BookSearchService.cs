using BookLibraryMaui.Models;
using System.Text.Json;

namespace BookLibraryMaui.DAL
{
    public class BookSearchService
    {
        private static readonly HttpClient Client = new();

        public async Task<Book> GetBookAsync(string isbn)
        {
            try
            {
                var bookResult = await GetBookByIsbnAsync(isbn);

                if (bookResult.Authors.Count < 1)
                {
                    return new Book
                    {
                        Title = bookResult.Title,
                        Description = bookResult.Description
                    };
                }

                var authorKey = bookResult.Authors.First();
                var author = await GetAuthor(authorKey);

                return new Book
                {
                    Author = author?.Name,
                    Title = bookResult.Title,
                    Description = bookResult.Description
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static async Task<AuthorInfoResult> GetAuthor(AuthorKey authorKey)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://openlibrary.org/{authorKey.Key}");
            request.Headers.Add("User-Agent", "Shelf Buddy/1.0 (twofoxesdigital01@gmail.com)");
            request.Headers.Add("accept", "application/json");

            var response = await Client.SendAsync(request);

            // TODO: Handle error
            if (!response.IsSuccessStatusCode) return null;

            var dataObjects = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthorInfoResult>(dataObjects);
            return result;
        }

        private static async Task<BookApiResult> GetBookByIsbnAsync(string isbn)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://openlibrary.org/isbn/{isbn}");
            request.Headers.Add("User-Agent", "Shelf Buddy/1.0 (twofoxesdigital01@gmail.com)");
            request.Headers.Add("accept", "application/json");

            var response = await Client.SendAsync(request);

            // TODO: Handle error
            if (!response.IsSuccessStatusCode) return null;

            var dataObjects = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BookApiResult>(dataObjects);

            return result;
        }
    }
}
