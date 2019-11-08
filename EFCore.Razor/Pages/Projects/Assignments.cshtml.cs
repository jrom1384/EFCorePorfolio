using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Projects
{
    public class AssignmentsModel : PageModel
    {
        private readonly IAssignmentService _service;
        private readonly IProjectService _projectService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public AssignmentsModel(
            IAssignmentService service,
            IProjectService projectService,
            IRoleService roleService,
            IMapper mapper,
            IOptions<PageSettings> pageSettings)
        {
            _service = service;
            _projectService = projectService;
            _roleService = roleService;
            _mapper = mapper;
            _pageSettings = pageSettings.Value;
        }

        [BindProperty(SupportsGet = true)]
        public GenericPage<AssignmentModel> GenericPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Project_Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Project { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Role_Id { get; set; }

        public IList<RoleModel> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectResult = await _projectService.FindByIdAsync(id.Value);
            if (!projectResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Project_Id = projectResult.Value.Id;
            Project = projectResult.Value.Project;

            if (!GenericPage.SortField.Equals(GenericPage.PreviousSortField))
            {
                GenericPage.SortOrder = SortOrder.Ascending;
            }
            else if (GenericPage.IsHeaderClicked)
            {
                GenericPage.SortOrder = GenericPage.SortOrder.Reverse();
            }

            var pageFilterDTO = _mapper.Map<AssignmentPageFilterDTO>(GenericPage);
            pageFilterDTO.Project_Id = projectResult.Value.Id;
            pageFilterDTO.Role_Id = Role_Id;
            pageFilterDTO.PageSize = _pageSettings.PageSize;

            var result = await _service.CreatePageListAsync(pageFilterDTO);
            var roleResult = await _roleService.GetListAsync();
            if (!result.IsSuccess || !roleResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            var paginatedListDTO = result.Value;
            GenericPage.PaginatedList = new PaginatedList<AssignmentModel>(paginatedListDTO.Select(d => _mapper.Map<AssignmentModel>(d)).ToList(), paginatedListDTO.MatchCount,
               GenericPage.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit);

            Roles = roleResult.Value.Select(r => _mapper.Map<RoleModel>(r)).ToList();

            return Page();
        }
    }
}