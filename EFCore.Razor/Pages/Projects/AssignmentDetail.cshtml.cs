using AutoMapper;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Projects
{
    public class AssignmentDetailModel : PageModel
    {
        private readonly IAssignmentService _service;
        private readonly IMapper _mapper;

        public AssignmentDetailModel(IAssignmentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public AssignmentModel Assignment { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FindByIdAsync(id.Value);
            if (!result.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }
            else if (result.Value == null)
            {
                return NotFound();
            }

            Assignment = _mapper.Map<AssignmentModel>(result.Value);
            return Page();
        }

    }
}