using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Razor.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly IEmployeeService _service;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public IndexModel(
            IEmployeeService service,
            IDepartmentService departmentService, 
            IMapper mapper, 
            IOptions<PageSettings> pageSettings)
        {
            _service = service;
            _departmentService = departmentService;
            _mapper = mapper;
            _pageSettings = pageSettings.Value;
        }

        [BindProperty(SupportsGet = true)]
        public GenericPage<EmployeeModel> GenericPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Department_Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public Gender? Gender { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsActive { get; set; }

        public IList<DepartmentModel> Departments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!GenericPage.SortField.Equals(GenericPage.PreviousSortField))
            {
                GenericPage.SortOrder = SortOrder.Ascending;
            }
            else if (GenericPage.IsHeaderClicked)
            {
                GenericPage.SortOrder = GenericPage.SortOrder.Reverse();
            }

            var pageFilterDTO = _mapper.Map<EmployeePageFilterDTO>(GenericPage);
            pageFilterDTO.Department_Id = Department_Id;
            pageFilterDTO.Gender = Gender;
            pageFilterDTO.IsActive = IsActive;
            pageFilterDTO.PageSize = _pageSettings.PageSize;

            var result = await _service.CreatePageListAsync(pageFilterDTO);
            var departmentResult = await _departmentService.GetListAsync();
            if (!result.IsSuccess
                || !departmentResult.IsSuccess)
            {
                return RedirectToPage("../Errors/ApplicationExceptionLevelHandler");
            }

            var paginatedListDTO = result.Value;
            GenericPage.PaginatedList = new PaginatedList<EmployeeModel>(paginatedListDTO.Select(d => _mapper.Map<EmployeeModel>(d)).ToList(), paginatedListDTO.MatchCount,
                GenericPage.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit);

            Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentModel>(d)).ToList();
            return Page();
        }
    }
}
