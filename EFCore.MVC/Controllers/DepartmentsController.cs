using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.MVC.Constants;
using EFCore.MVC.Models;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.MVC.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _service;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public DepartmentsController(
            IDepartmentService service, 
            IMapper mapper, 
            IOptions<PageSettings> pageSettings, 
            IServiceProvider provider)
        {
            _service = service;
            _mapper = mapper;
            _pageSettings = pageSettings.Value;

            //using (var context2 = new ApplicationDBContext(provider.GetRequiredService<DbContextOptions<ApplicationDBContext>>()))
            //{
            //    var department = context2.Department.AsNoTracking().ToList();
            //}
        }

        // GET: Department
        public async Task<IActionResult> Index(GenericPage<DepartmentVM> genericPage)
        {
            if (!genericPage.SortField.Equals(genericPage.PreviousSortField))
            {
                genericPage.SortOrder = SortOrder.Ascending;
            }
            else if(genericPage.IsHeaderClicked)
            {
                genericPage.SortOrder = genericPage.SortOrder.Reverse();
            }

            var pageFilterDTO = _mapper.Map<PageFilterDTO>(genericPage);
            pageFilterDTO.PageSize = _pageSettings.PageSize;

            var result = await _service.CreatePageListAsync(pageFilterDTO);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            var paginatedListDTO = result.Value;
            return View(new GenericPage<DepartmentVM>
            {
                SearchString = genericPage.SearchString,
                SortField = genericPage.SortField,
                PreviousSortField = genericPage.SortField,
                SortOrder = genericPage.SortOrder,

                CurrentPageIndex = genericPage.CurrentPageIndex,
                PaginatedList = new PaginatedList<DepartmentVM>(paginatedListDTO.Select(dto => _mapper.Map<DepartmentVM>(dto)).ToList(), paginatedListDTO.MatchCount, 
                    genericPage.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit)
            });
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(d => d.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<DepartmentVM>(result.Value));
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Department")] DepartmentVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AddAsync(_mapper.Map<DepartmentDTO>(viewModel));
                if (result.IsSuccess)
                {
                    TempData[TempDataKeys.SaveMode] = SaveMode.Create;
                    return RedirectToAction(nameof(Success), _mapper.Map<DepartmentVM>(result.Value));
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

            return View(viewModel);
        }

        public IActionResult Success(DepartmentVM viewModel)
        {
            SaveMode saveMode = (SaveMode)Convert.ToInt32(TempData[TempDataKeys.SaveMode]);
            ViewBag.SaveMode = saveMode.ToString();

            switch (saveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    ViewBag.Message = "Successfully saved department.";
                    break;

                case SaveMode.Delete:
                    ViewBag.Message = "Successfully deleted department.";
                    break;

                case SaveMode.None:
                default:
                    break;
            }

            return View(viewModel);
        }

        // GET: Department/Edit/5
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

            return View(_mapper.Map<DepartmentVM>(result.Value));
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Department")] DepartmentVM viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(_mapper.Map<DepartmentDTO>(viewModel));
                if (result.IsSuccess)
                {
                    TempData[TempDataKeys.SaveMode] = SaveMode.Edit;
                    return RedirectToAction(nameof(Success), _mapper.Map<DepartmentVM>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await DepartmentExists(id))
                //{
                //    return NotFound();
                //}
                else
                {
                    return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
                }
            }

            return View(viewModel);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(d => d.Id == id.Value);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<DepartmentVM>(result.Value));
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (result.IsSuccess)
            {
                TempData[TempDataKeys.SaveMode] = SaveMode.Delete;
                return RedirectToAction(nameof(Success), _mapper.Map<DepartmentVM>(result.Value));
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

        private async Task<bool> DepartmentExists(long id)
        {
            var result = await _service.AnyAsync(d => d.Id == id);
            if (!result.IsSuccess)
            {
                RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
                return false;
            }

            return result.Value;
        }

    }
}
