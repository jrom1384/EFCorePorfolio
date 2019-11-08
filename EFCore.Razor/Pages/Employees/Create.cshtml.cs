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
    public class CreateModel : PageModel
    {
        private readonly IEmployeeService _service;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public CreateModel(
            IEmployeeService service,
            IDepartmentService departmentService, 
            IMapper mapper)
        {
            _service = service;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _departmentService.GetListAsync();
            if (!result.IsSuccess)
            {
                RedirectToPage("./Index");
            }

            Departments = result.Value.Select(d => _mapper.Map<DepartmentModel>(d)).ToList();
            return Page();
        }

        [BindProperty]
        public EmployeeModel Employee { get; set; }

        public List<DepartmentModel> Departments { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AddAsync(_mapper.Map<EmployeeDTO>(Employee));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Create;
                    return RedirectToPage("./Success", _mapper.Map<EmployeeModel>(result.Value));
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

            var departmentResult = await _departmentService.GetListAsync();
            if (!departmentResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentModel>(d)).ToList();
            return Page();
        }
    }
}