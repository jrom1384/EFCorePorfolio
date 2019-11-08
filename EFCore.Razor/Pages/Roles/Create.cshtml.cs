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
    public class CreateModel : PageModel
    {
        private readonly IRoleService _service;
        private readonly IMapper _mapper;

        public CreateModel(IRoleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RoleModel Role { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AddAsync(_mapper.Map<RoleDTO>(Role));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Create;
                    return RedirectToPage("./Success", _mapper.Map<RoleModel>(result.Value));
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

            return Page();
        }
    }
}