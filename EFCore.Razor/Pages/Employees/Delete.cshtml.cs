using AutoMapper;
using EFCore.Common;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;

        public DeleteModel(IEmployeeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public EmployeeModel Employee { get; set; }

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.DeleteByIdAsync(id.Value);
            if (result.IsSuccess)
            {
                SaveMode = SaveMode.Delete;
                return RedirectToPage("./Success", _mapper.Map<EmployeeModel>(result.Value));
            }
            else if (result.ErrorType == ErrorType.Defined)
            {
                ErrorMessage = result.ErrorMessage;
                return await OnGetAsync(id);
            }
            else
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }
        }
    }
}
