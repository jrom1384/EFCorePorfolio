using EFCore.MVC.Components;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EFCore.MVC.ViewComponents
{
    public class PagingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
            string action,
            Dictionary<string, string> routeData, 
            int currentPageIndex, int pageSize,
            int matchCount, bool hasPreviousPage, bool hasNextPage, 
            int totalPages, int pageIndexViewLimit)
            
        {
            return View(new PagingViewComponentModel
            {
                Action = action,
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
