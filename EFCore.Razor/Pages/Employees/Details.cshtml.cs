using AutoMapper;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;

        public DetailsModel(IEmployeeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public EmployeeModel Employee { get; set; }

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
    }
}
