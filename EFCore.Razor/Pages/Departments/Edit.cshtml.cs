using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly IDepartmentService _service;
        private readonly IMapper _mapper;

        public EditModel(IDepartmentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public DepartmentModel Department { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(_mapper.Map<DepartmentDTO>(Department));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Edit;
                    return RedirectToPage("./Success", _mapper.Map<DepartmentModel>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ErrorMessage = result.ErrorMessage;
                }
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await DepartmentExists(Department.Id))
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

        private async Task<bool> DepartmentExists(long id)
        {
            var result = await _service.AnyAsync(d => d.Id == id);
            if (!result.IsSuccess)
            {
                RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
                return false;
            }

            return result.Value;
        }
    }
}
