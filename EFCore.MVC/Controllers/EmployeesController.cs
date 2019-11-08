using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.MVC.Constants;
using EFCore.MVC.Models;
using EFCore.ServiceLayer;
using EFCore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.MVC.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _service;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public EmployeesController(
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

        // GET: Employee
        public async Task<IActionResult> Index(EmployeePageFilterVM pageFilter)
        {
            if (!pageFilter.SortField.Equals(pageFilter.PreviousSortField))
            {
                pageFilter.SortOrder = SortOrder.Ascending;
            }
            else if (pageFilter.IsHeaderClicked)
            {
                pageFilter.SortOrder = pageFilter.SortOrder.Reverse();
            }

            var pageFilterDTO = _mapper.Map<EmployeePageFilterDTO>(pageFilter);
            pageFilterDTO.PageSize = _pageSettings.PageSize;

            var result = await _service.CreatePageListAsync(pageFilterDTO);
            var departmentResult = await _departmentService.GetListAsync();
            if (!result.IsSuccess
                || !departmentResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            var paginatedListDTO = result.Value;
            ViewBag.Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentVM>(d)).ToList();

            return View(new EmployeePageFilterVM
            {
                SearchString = pageFilter.SearchString,
                Department_Id = pageFilter.Department_Id,
                Gender = pageFilter.Gender,
                IsActive = pageFilter.IsActive,

                SortField = pageFilter.SortField,
                PreviousSortField = pageFilter.SortField,
                SortOrder = pageFilter.SortOrder,
                CurrentPageIndex = pageFilter.CurrentPageIndex,

                PaginatedList = new PaginatedList<EmployeeVM>(paginatedListDTO.Select(dto => _mapper.Map<EmployeeVM>(dto)).ToList(), paginatedListDTO.MatchCount,
                    pageFilter.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit)
            });
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultIncludeDepartmentAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<EmployeeVM>(result.Value));
        }

        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            var result = await _departmentService.GetListAsync();
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Departments = result.Value.Select(d => _mapper.Map<DepartmentVM>(d)).ToList();
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DateOfBirth,Gender,IsActive,Department_Id")] EmployeeVM viewModel)
        {
            if (ModelState.IsValid && viewModel.Department_Id > 0)
            {
                var result = await _service.AddAsync(_mapper.Map<EmployeeDTO>(viewModel));
                if (result.IsSuccess)
                {
                    TempData[TempDataKeys.SaveMode] = SaveMode.Create;
                    return RedirectToAction(nameof(Success), _mapper.Map<EmployeeDTO>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
                }
            }
            else
            { 
                if (ModelState[nameof(viewModel.Gender)].ValidationState == ModelValidationState.Invalid)
                {
                    var displayName = DisplayNameHelper.GetAttributeDisplayName(typeof(EmployeeVM).GetProperty(nameof(viewModel.Gender)));
                    ModelState[nameof(viewModel.Gender)].Errors.Clear();
                    ModelState[nameof(viewModel.Gender)].Errors.Add($"The { displayName } field is required.");
                }

                if (ModelState[nameof(viewModel.DateOfBirth)].ValidationState == ModelValidationState.Invalid)
                {
                    var displayName = DisplayNameHelper.GetAttributeDisplayName(typeof(EmployeeVM).GetProperty(nameof(viewModel.DateOfBirth)));
                    ModelState[nameof(viewModel.DateOfBirth)].Errors.Clear();
                    ModelState[nameof(viewModel.DateOfBirth)].Errors.Add($"The { displayName } field is required.");
                }
            }

            var departmentResult = await _departmentService.GetListAsync();
            if (!departmentResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentVM>(d)).ToList();
            return View(viewModel);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FindByIdAsync(id.Value);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }
            
            var viewModel = _mapper.Map<EmployeeVM>(result.Value);

            var departmentResult = await _departmentService.GetListAsync();
            if (!departmentResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentVM>(d)).ToList();
            return View(viewModel);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,DateOfBirth,Gender,IsActive,Department_Id")] EmployeeVM viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(_mapper.Map<EmployeeDTO>(viewModel));
                if (result.IsSuccess)
                {
                    TempData[TempDataKeys.SaveMode] = SaveMode.Edit;
                    return RedirectToAction(nameof(Success), _mapper.Map<EmployeeDTO>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
                }
            }
            else if (viewModel.Department_Id == 0)
            {
                ModelState.AddModelError(nameof(viewModel.Department_Id), "The department field is required");
            }

            var departmentResult = await _departmentService.GetListAsync();
            if (!departmentResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Departments = departmentResult.Value.Select(d => _mapper.Map<DepartmentVM>(d)).ToList();
            
            return View(viewModel);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultIncludeDepartmentAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<EmployeeVM>(result.Value));
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (result.IsSuccess)
            {
                TempData[TempDataKeys.SaveMode] = SaveMode.Delete;
                return RedirectToAction(nameof(Success), _mapper.Map<EmployeeDTO>(result.Value));
            }
            else if (result.ErrorType == ErrorType.Defined)
            {
                TempData[TempDataKeys.ErrorMessage] = result.ErrorMessage; ;
                return RedirectToAction(nameof(Delete), id);
            }
            else
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }
        }

        private async Task<bool> EmployeeExists(long id)
        {
            var result = await _service.AnyAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
                return false;
            }

            return result.Value;
        }

        public IActionResult Success(EmployeeVM viewModel)
        {
            SaveMode saveMode = (SaveMode)Convert.ToInt32(TempData[TempDataKeys.SaveMode]);
            ViewBag.SaveMode = saveMode.ToString();

            switch (saveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    ViewBag.Message = "Successfully saved employee.";
                    break;

                case SaveMode.Delete:
                    ViewBag.Message = "Successfully deleted employee.";
                    break;

                case SaveMode.None:
                default:
                    break;
            }

            return View(viewModel);
        }
    }
}
