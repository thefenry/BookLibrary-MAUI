namespace BookLibraryMaui.Models
{
    public class GoogleAPIBookResults
    {
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public VolumeInfo VolumeInfo { get; set; }

        public SaleInfo SaleInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string Title { get; set; }

        public List<string> Authors { get; set; }

        public string Description { get; set; }

        public List<string> Categories { get; set; }
    }

    public class SaleInfo
    {
        public bool IsEbook { get; set; }
    }
}
