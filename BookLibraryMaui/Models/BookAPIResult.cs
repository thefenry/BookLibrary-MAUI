using System.Text.Json.Serialization;

namespace BookLibraryMaui.Models
{
    public class BookApiResult
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("authors")]
        public List<AuthorKey> Authors { get; set; }
    }

    public class AuthorKey
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }

    public class AuthorInfoResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
