using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Roles
{
    public class EditModel : PageModel
    {
        private readonly IRoleService _service;
        private readonly IMapper _mapper;

        public EditModel(IRoleService service, IMapper mapper)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(_mapper.Map<RoleDTO>(Role));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Edit;
                    return RedirectToPage("./Success", _mapper.Map<RoleModel>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ErrorMessage = result.ErrorMessage;
                }
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await RoleExists(Role.Id))
                //{
                //    return NotFound();
                //}
                else
                {
                    return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
                }
            }

            return Page();
        }

        private async Task<bool> RoleExists(long id)
        {
            var result = await _service.AnyAsync(r => r.Id == id);
            if (!result.IsSuccess)
            {
                RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
                return false;
            }

            return result.Value;
        }
    }
}
