namespace PIM.Api.TransferObjects.Requests
{
    public class ProductsFilterModel
    {
        public int Page { get; set; }
        public int GetPage() => Page <= 0 ? 1 : Page;
        public int PageSize { get; set; }
        public int GetPageSize() => PageSize <= 0 ? 10 : PageSize;
        public int Skip => GetPageSize() * (GetPage() - 1);
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }



    }
}
