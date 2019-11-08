using AutoMapper;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Projects
{
    public class DetailsModel : PageModel
    {
        private readonly IProjectService _service;
        private readonly IMapper _mapper;

        public DetailsModel(IProjectService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public ProjectModel Project { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(p => p.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            Project = _mapper.Map<ProjectModel>(result.Value);
            if (Project == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
