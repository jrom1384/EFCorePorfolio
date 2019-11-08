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
    public class CreateModel : PageModel
    {
        private readonly IDepartmentService _service;
        private readonly IMapper _mapper;

        public CreateModel(IDepartmentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DepartmentModel Department { get; set; }

        public string ErrorMessage { get; set; }

        [TempData]
        public SaveMode SaveMode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AddAsync(_mapper.Map<DepartmentDTO>(Department));
                if (result.IsSuccess)
                {
                    SaveMode = SaveMode.Create;
                    return RedirectToPage("./Success", _mapper.Map<DepartmentModel>(result.Value));
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