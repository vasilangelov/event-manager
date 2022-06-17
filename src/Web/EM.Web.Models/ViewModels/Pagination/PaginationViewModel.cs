namespace EM.Web.Models.ViewModels.Pagination
{
    public class PaginationViewModel<TModel>
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int DisplayPageCount { get; set; }

        public int ItemsPerPage { get; set; }

        public string? SearchQuery { get; set; }

        public IEnumerable<TModel> Items { get; set; }
            = Enumerable.Empty<TModel>();
    }
}
