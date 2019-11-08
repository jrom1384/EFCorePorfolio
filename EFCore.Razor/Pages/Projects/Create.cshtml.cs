using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Projects
{
    public class CreateModel : PageModel
    {
        private readonly IProjectService _service;
        private readonly IMapper _mapper;

        public CreateModel(IProjectService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProjectModel Project { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AddAsync(_mapper.Map<ProjectDTO>(Project));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Create;
                    return RedirectToPage("./Success", _mapper.Map<ProjectModel>(result.Value));
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