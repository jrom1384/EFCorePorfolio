using AutoMapper;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Roles
{
    public class DetailsModel : PageModel
    {
        private readonly IRoleService _service;
        private readonly IMapper _mapper;

        public DetailsModel(IRoleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public RoleModel Role { get; set; }

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
    }
}
