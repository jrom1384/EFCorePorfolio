using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.MVC.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.MVC.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentService _service;
        private readonly IDepartmentService _departmentService;
        private readonly IProjectService _projectService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public AssignmentsController(
            IAssignmentService service,
            IDepartmentService departmentService,
            IProjectService projectService,
            IRoleService roleService,
            IMapper mapper, 
            IOptions<PageSettings> pageSettings)
        {
            _service = service;
            _departmentService = departmentService;
            _projectService = projectService;
            _roleService = roleService;
            _mapper = mapper;
            _pageSettings = pageSettings.Value;
        }

        // GET: Assignments
        public async Task<IActionResult> Index(AssignmentPageFilterVM pageFilter)
        {
            if (!pageFilter.SortField.Equals(pageFilter.PreviousSortField))
            {
                pageFilter.SortOrder = SortOrder.Ascending;
            }
            else if (pageFilter.IsHeaderClicked)
            {
                pageFilter.SortOrder = pageFilter.SortOrder.Reverse();
            }

            var pageFilterDTO = _mapper.Map<AssignmentPageFilterDTO>(pageFilter);
            pageFilterDTO.PageSize = _pageSettings.PageSize;

            var result = await _service.CreatePageListAsync(pageFilterDTO);
            var departmentResult = await _departmentService.GetListAsync();
            var projectResult = await _projectService.GetListAsync();
            var roleResult = await _roleService.GetListAsync();
            if (!result.IsSuccess
                || !departmentResult.IsSuccess
                || !projectResult.IsSuccess
                || !roleResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            var paginatedListDTO = result.Value;
            ViewBag.Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentVM>(d)).ToList();
            ViewBag.Roles = roleResult.Value.Select(r => _mapper.Map<RoleVM>(r)).ToList();
            ViewBag.Projects = projectResult.Value.Select(p => _mapper.Map<ProjectVM>(p)).ToList();

            return View(new AssignmentPageFilterVM
            {
                SearchString = pageFilter.SearchString,
                Department_Id = pageFilter.Department_Id,
                Role_Id = pageFilter.Role_Id,
                Project_Id = pageFilter.Project_Id,

                SortField = pageFilter.SortField,
                PreviousSortField = pageFilter.SortField,
                SortOrder = pageFilter.SortOrder,
                CurrentPageIndex = pageFilter.CurrentPageIndex,

                PaginatedList = new PaginatedList<AssignmentVM>(paginatedListDTO.Select(dto => _mapper.Map<AssignmentVM>(dto)).ToList(), paginatedListDTO.MatchCount,
                    pageFilter.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit),
            });
        }
    }
}
