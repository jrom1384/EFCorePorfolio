using System.Collections.Generic;

namespace EFCore.Razor.Components
{
    public class PagingViewComponentModel
    {
        public Dictionary<string, string> RouteData { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageSize { get; set; }

        public int MatchCount { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public int TotalPages { get; set; }

        public int PageIndexViewLimit { get; set; }

    }
}
