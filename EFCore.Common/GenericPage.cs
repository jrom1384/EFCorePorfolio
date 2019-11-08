namespace EFCore.Common
{
    public class GenericPage<T> where T : class
    {
        public string SearchString { get; set; } = string.Empty;

        public string SortField { get; set; } = string.Empty;

        public string PreviousSortField { get; set; } = string.Empty;

        public bool IsHeaderClicked { get; set; } = false;

        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

        public PaginatedList<T> PaginatedList;

        public int CurrentPageIndex { get; set; } = 1;
    }
}
