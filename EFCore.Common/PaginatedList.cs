using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore.Common
{
    public class PaginatedList<T> where T : class
    {
        private int _pageIndex = 0;

        public PaginatedList(List<T> items, int matchCount, int pageIndex, int pageSize, int pageIndexViewLimit)
        {
            this.Items.AddRange(items);
            this.MatchCount = matchCount;
            this.TotalPages = (int)Math.Ceiling((double)MatchCount / pageSize);
            _pageIndex = pageIndex;
            this.PageSize = pageSize;
            this.PageIndexViewLimit = pageIndexViewLimit;
            this.PageIndexList = Enumerable.Range(1, this.TotalPages).ToList();
        }

        public int MatchCount { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public List<int> PageIndexList { get; set; }

        public int PageIndexViewLimit { get; set; } = 5;

        public bool HasPreviousPage
        {
            get
            {
                return _pageIndex > 1;
            }
        }
        public bool HasNextPage
        {
            get
            {
                return _pageIndex < TotalPages;
            }
        }

        public List<T> Items { get; set; } = new List<T>();

    }
}
