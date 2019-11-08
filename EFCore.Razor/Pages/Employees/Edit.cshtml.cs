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

namespace EFCore.Razor.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly IEmployeeService _service;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public EditModel(
            IEmployeeService service,
            IDepartmentService departmentService, 
            IMapper mapper)
        {
            _service = service;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [BindProperty]
        public EmployeeModel Employee { get; set; }

        public List<DepartmentModel> Departments { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultIncludeDepartmentAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Employee = _mapper.Map<EmployeeModel>(result.Value);
            if (Employee == null)
            {
                return NotFound();
            }

            var departmentResult = await _departmentService.GetListAsync();
            if (!departmentResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentModel>(d)).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(_mapper.Map<EmployeeDTO>(Employee));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Edit;
                    return RedirectToPage("./Success", _mapper.Map<EmployeeModel>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ErrorMessage = result.ErrorMessage;
                }
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await EmployeeExists(Employee.Id))
                //{
                //    return NotFound();
                //}
                else
                {
                    return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
                }
            }

            var departmentResult = await _departmentService.GetListAsync();
            if (!departmentResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentModel>(d)).ToList();

            return Page();

        }

        private async Task<bool> EmployeeExists(long id)
        {
            var result = await _service.AnyAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
                return false;
            }

            return result.Value;
        }
    }
}
