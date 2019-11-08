using System.Collections.Generic;

namespace EFCore.MVC.Components
{
    public class PagingViewComponentModel
    {
        public string Action { get; set; }

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
