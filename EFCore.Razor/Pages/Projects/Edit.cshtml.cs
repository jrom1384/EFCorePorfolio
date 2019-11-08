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
    public class EditModel : PageModel
    {
        private readonly IProjectService _service;
        private readonly IMapper _mapper;

        public EditModel(IProjectService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public ProjectModel Project { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(_mapper.Map<ProjectDTO>(Project));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Edit;
                    return RedirectToPage("./Success", _mapper.Map<ProjectModel>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ErrorMessage = result.ErrorMessage;
                }
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await ProjectExists(Project.Id))
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

        private async Task<bool> ProjectExists(long id)
        {
            var result = await _service.AnyAsync(p => p.Id == id);
            if (!result.IsSuccess)
            {
                RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
                return false;
            }

            return result.Value;
        }
    }
}
