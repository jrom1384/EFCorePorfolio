using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly IProjectService _service;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public IndexModel(
            IProjectService service, 
            IMapper mapper, 
            IOptions<PageSettings> pageSettings)
        {
            _service = service;
            _mapper = mapper;
            _pageSettings = pageSettings.Value;
        }

        [BindProperty(SupportsGet = true)]
        public GenericPage<ProjectModel> GenericPage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!GenericPage.SortField.Equals(GenericPage.PreviousSortField))
            {
                GenericPage.SortOrder = SortOrder.Ascending;
            }
            else if (GenericPage.IsHeaderClicked)
            {
                GenericPage.SortOrder = GenericPage.SortOrder.Reverse();
            }

            var pageFilterDTO = _mapper.Map<PageFilterDTO>(GenericPage);
            pageFilterDTO.PageSize = _pageSettings.PageSize;

            var result = await _service.CreatePageListAsync(pageFilterDTO);
            if (!result.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            var paginatedListDTO = result.Value;
            GenericPage.PaginatedList = new PaginatedList<ProjectModel>(paginatedListDTO.Select(d => _mapper.Map<ProjectModel>(d)).ToList(), paginatedListDTO.MatchCount,
                GenericPage.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit);

            return Page();
        }
    }
}
