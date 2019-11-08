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
    public class RolesController : Controller
    {
        private readonly IRoleService _service;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public RolesController(IRoleService service, IMapper mapper, IOptions<PageSettings> pageSettings)
        {
            _service = service;
            _mapper = mapper;
            _pageSettings = pageSettings.Value;
        }

        // GET: Role
        public async Task<IActionResult> Index(GenericPage<RoleVM> genericPage)
        {
            if (!genericPage.SortField.Equals(genericPage.PreviousSortField))
            {
                genericPage.SortOrder = SortOrder.Ascending;
            }
            else if (genericPage.IsHeaderClicked)
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
            return View(new GenericPage<RoleVM>
            {
                SearchString = genericPage.SearchString,
                SortField = genericPage.SortField,
                PreviousSortField = genericPage.SortField,
                SortOrder = genericPage.SortOrder,

                CurrentPageIndex = genericPage.CurrentPageIndex,
                PaginatedList = new PaginatedList<RoleVM>(paginatedListDTO.Select(dto => _mapper.Map<RoleVM>(dto)).ToList(), paginatedListDTO.MatchCount, 
                    genericPage.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit)
            });
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(r => r.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<RoleVM>(result.Value));
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Role")] RoleVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AddAsync(_mapper.Map<RoleDTO>(viewModel));
                if (result.IsSuccess)
                {
                    TempData[TempDataKeys.SaveMode] = SaveMode.Create;
                    return RedirectToAction(nameof(Success), _mapper.Map<RoleDTO>(result.Value));
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

        // GET: Role/Edit/5
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

            return View(_mapper.Map<RoleVM>(result.Value));
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Role")] RoleVM viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(_mapper.Map<RoleDTO>(viewModel));
                if (result.IsSuccess)
                {
                    TempData[TempDataKeys.SaveMode] = SaveMode.Edit;
                    return RedirectToAction(nameof(Success), _mapper.Map<RoleDTO>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await RoleExists(id))
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

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(r => r.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<RoleVM>(result.Value));
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (result.IsSuccess)
            {
                TempData[TempDataKeys.SaveMode] = SaveMode.Delete;
                return RedirectToAction(nameof(Success), _mapper.Map<RoleDTO>(result.Value));
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

        private async Task<bool> RoleExists(long id)
        {
            var result = await _service.AnyAsync(r => r.Id == id);
            if (!result.IsSuccess)
            {
                RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
                return false;
            }

            return result.Value;
        }

        public IActionResult Success(RoleVM viewModel)
        {
            SaveMode saveMode = (SaveMode)Convert.ToInt32(TempData[TempDataKeys.SaveMode]);
            ViewBag.SaveMode = saveMode.ToString();

            switch (saveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    ViewBag.Message = "Successfully saved role.";
                    break;

                case SaveMode.Delete:
                    ViewBag.Message = "Successfully deleted role.";
                    break;

                case SaveMode.None:
                default:
                    break;
            }

            return View(viewModel);
        }
    }
}
