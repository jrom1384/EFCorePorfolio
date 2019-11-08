using AutoMapper;
using EFCore.Common;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Projects
{
    public class DeleteAssignmentModel : PageModel
    {
        private readonly IAssignmentService _service;
        private readonly IMapper _mapper;

        public DeleteAssignmentModel(IAssignmentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public AssignmentModel Assignment { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

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
                return RedirectToPage("./AssignSuccess", _mapper.Map<AssignmentModel>(result.Value));
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