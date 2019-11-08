using AutoMapper;
using EFCore.Common;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Roles
{
    public class DeleteModel : PageModel
    {
        private readonly IRoleService _service;
        private readonly IMapper _mapper;

        public DeleteModel(IRoleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public RoleModel Role { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(r => r.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Role = _mapper.Map<RoleModel>(result.Value);
            if (Role == null)
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
                return RedirectToPage("./Success", _mapper.Map<RoleModel>(result.Value));
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
