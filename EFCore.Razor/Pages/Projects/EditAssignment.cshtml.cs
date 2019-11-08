using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Projects
{
    public class EditAssignmentModel : PageModel
    {
        private readonly IProjectService _projectService;
        private readonly IAssignmentService _assignmentService;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public EditAssignmentModel(
            IProjectService projectService,
            IAssignmentService assignmentService,
            IEmployeeService employeeService,
            IRoleService roleService,
            IMapper mapper)
        {
            _projectService = projectService;
            _assignmentService = assignmentService;
            _employeeService = employeeService;
            _roleService = roleService;
            _mapper = mapper;
        }

        [BindProperty]
        public AssignmentModel Assignment { get; set; }

        public IList<EmployeeListModel> Employees { get; set; }

        public IList<RoleModel> Roles { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

        public async Task<IActionResult> OnGet(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _assignmentService.FindByIdAsync(id.Value);
            if (!result.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }
            else if (result.Value == null)
            {
                return NotFound();
            }

            Assignment = _mapper.Map<AssignmentModel>(result.Value);

            var employeeResult = await _employeeService.GetListAsync();
            var roleResult = await _roleService.GetListAsync();
            if (!employeeResult.IsSuccess
                || !roleResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Employees = _mapper.Map<List<EmployeeModel>>(employeeResult.Value).Select(e => new EmployeeListModel
            {
                Id = e.Id,
                Name = $"{ e.LastName }, { e.FirstName }"
            }).ToList();

            Roles = _mapper.Map<List<RoleModel>>(roleResult.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _assignmentService.UpdateAsync(_mapper.Map<AssignmentDTO>(Assignment));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Edit;
                    return RedirectToPage("./AssignSuccess", _mapper.Map<AssignmentModel>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
                }
            }
            else
            {
                ErrorMessage = Common.ErrorMessages.InvalidParameterValues;
            }

            var employeeResult = await _employeeService.GetListAsync();
            var roleResult = await _roleService.GetListAsync();
            if (!employeeResult.IsSuccess
                || !roleResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Employees = _mapper.Map<List<EmployeeModel>>(employeeResult.Value).Select(e => new EmployeeListModel
            {
                Id = e.Id,
                Name = $"{ e.LastName }, { e.FirstName }"
            }).ToList();

            Roles = _mapper.Map<List<RoleModel>>(roleResult.Value);

            return Page();
        }
    }
}