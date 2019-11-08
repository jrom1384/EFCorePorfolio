using EFCore.Common;

namespace EFCore.DTO
{
    public class PageFilterDTO
    {
        public string SearchString { get; set; } = string.Empty;

        public string SortField { get; set; } = string.Empty;

        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

        public int CurrentPageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
