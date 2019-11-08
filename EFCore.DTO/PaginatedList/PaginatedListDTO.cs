using System.Collections.Generic;

namespace EFCore.DTO
{
    public class PaginatedListDTO<TDTO> : List<TDTO> where TDTO : class
    {
        public PaginatedListDTO(List<TDTO> list, int matchCount)
        {
            this.AddRange(list);
            MatchCount = matchCount;
        }

        public int MatchCount { get; set; }
    }
}
