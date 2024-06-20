using BookLibraryMaui.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BookLibraryMaui.DAL
{
    public class GoogleBookRepository
    {
        public async Task<Book> GetBookAsync(string isbn)
        {
            GoogleAPIBookResults bookResult = await GetBookByIsbnAsync(isbn);

            if (bookResult?.Items?.Count > 0)
            {
                Item book = bookResult.Items.First();

                return new Book
                {
                    Author = SetAuthor(book),

                    Title = SetTitle(book),

                    Description = SetDescription(book),

                    IsEBook = SetIsEbook(book),

                    Category = SetCategory(book)
                };
            }

            return null;
        }

        private async Task<GoogleAPIBookResults> GetBookByIsbnAsync(string isbn)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string url = $"https://www.googleapis.com/books/v1/volumes?q={isbn}+isbn&key=AIzaSyD6ZcI8kLpXhpMckhYWZoVsk0qjcM1PXWY";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<GoogleAPIBookResults>(dataObjects);

                return result;

            }

            // TODO: Handle error

            return null;
        }

        private string SetAuthor(Item book)
        {
            if (book.VolumeInfo == null && book.VolumeInfo.Authors == null)
            {
                return null;
            }

            return string.Join(",", book.VolumeInfo.Authors);
        }

        private string SetTitle(Item book)
        {
            if (book.VolumeInfo == null && book.VolumeInfo.Title == null)
            {
                return null;
            }

            return book.VolumeInfo.Title;
        }

        private string SetDescription(Item book)
        {
            if (book.VolumeInfo == null && book.VolumeInfo.Description == null)
            {
                return null;
            }

            return book.VolumeInfo.Description;
        }

        private bool SetIsEbook(Item book)
        {
            if (book.SaleInfo == null)
            {
                return false;
            }

            return book.SaleInfo.IsEbook;
        }

        private string SetCategory(Item book)
        {
            if (book.VolumeInfo == null && book.VolumeInfo.Categories == null)
            {
                return null;
            }

            return string.Join(",", book.VolumeInfo.Categories);
        }
    }
}
