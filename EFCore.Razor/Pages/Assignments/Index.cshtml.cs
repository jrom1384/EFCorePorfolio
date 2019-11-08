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

namespace EFCore.Razor.Pages.Assignments
{
    public class IndexModel : PageModel
    {
        private readonly IAssignmentService _service;
        private readonly IDepartmentService _departmentService;
        private readonly IRoleService _roleService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public IndexModel(
            IAssignmentService service, 
            IDepartmentService departmentService,
            IRoleService roleService,
            IProjectService projectService,
            IMapper mapper, 
            IOptions<PageSettings> pageSettings)
        {
            _service = service;
            _departmentService = departmentService;
            _roleService = roleService;
            _projectService = projectService;
            _mapper = mapper;
            _pageSettings = pageSettings.Value;
        }

        [BindProperty(SupportsGet = true)]
        public GenericPage<AssignmentModel> GenericPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Department_Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Role_Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Project_Id { get; set; }

        public IList<DepartmentModel> Departments { get; set; }
        public IList<ProjectModel> Projects { get; set; }
        public IList<RoleModel> Roles { get; set; }

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

            var pageFilterDTO = _mapper.Map<AssignmentPageFilterDTO>(GenericPage);
            pageFilterDTO.Department_Id = Department_Id;
            pageFilterDTO.Project_Id = Project_Id;
            pageFilterDTO.Role_Id = Role_Id;
            pageFilterDTO.PageSize = _pageSettings.PageSize;

            var result = await _service.CreatePageListAsync(pageFilterDTO);
            var departmentResult = await _departmentService.GetListAsync();
            var projectResult = await _projectService.GetListAsync();
            var roleResult = await _roleService.GetListAsync();
            if (!result.IsSuccess
                || !departmentResult.IsSuccess
                || !projectResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            var paginatedListDTO = result.Value;
            GenericPage.PaginatedList = new PaginatedList<AssignmentModel>(paginatedListDTO.Select(d => _mapper.Map<AssignmentModel>(d)).ToList(), paginatedListDTO.MatchCount,
                GenericPage.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit);
            
            Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentModel>(d)).ToList();
            Projects = projectResult.Value.Select(d => _mapper.Map<ProjectModel>(d)).ToList();
            Roles = roleResult.Value.Select(d => _mapper.Map<RoleModel>(d)).ToList();
            return Page();
        }
    }
}