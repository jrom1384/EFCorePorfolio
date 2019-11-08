using EFCore.Razor.Components;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EFCore.Razor.ViewComponents
{
    public class PagingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
            Dictionary<string, string> routeData, 
            int currentPageIndex, int pageSize,
            int matchCount, bool hasPreviousPage, bool hasNextPage, 
            int totalPages, int pageIndexViewLimit)
            
        {
            return View(new PagingViewComponentModel
            {
                RouteData = routeData,
                CurrentPageIndex = currentPageIndex,
                PageSize = pageSize,
                MatchCount = matchCount,
                HasPreviousPage = hasPreviousPage,
                HasNextPage= hasNextPage,
                TotalPages = totalPages,
                PageIndexViewLimit = pageIndexViewLimit
            });
        }
      
    }
}
