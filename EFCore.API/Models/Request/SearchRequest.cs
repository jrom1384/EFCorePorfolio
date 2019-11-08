using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class SearchRequest
    {
        public string Search { get; set; } = string.Empty;

        public string SortField { get; set; } = string.Empty;

        public string SortOrder { get; set; } = "Ascending";

        [Required]
        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; } = 1;

        [Required]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 15;
    }
}
