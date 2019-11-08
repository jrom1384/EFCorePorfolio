using AutoMapper;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Departments
{
    public class DetailsModel : PageModel
    {
        private readonly IDepartmentService _service;
        private readonly IMapper _mapper;

        public DetailsModel(IDepartmentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public DepartmentModel Department { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(d => d.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Department = _mapper.Map<DepartmentModel>(result.Value);
            if (Department == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
