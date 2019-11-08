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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.MVC.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _service;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;
        private readonly IAssignmentService _assignmentService;
        private readonly IMapper _mapper;
        private readonly PageSettings _pageSettings;

        public ProjectsController(
            IProjectService service,
            IEmployeeService employeeService,
            IRoleService roleService,
            IAssignmentService assignmentService,
            IMapper mapper, 
            IOptions<PageSettings> pageSettings)
        {
            _service = service;
            _employeeService = employeeService;
            _roleService = roleService;
            _assignmentService = assignmentService;
            _mapper = mapper;
            _pageSettings = pageSettings.Value;
        }

        // GET: Projects
        public async Task<IActionResult> Index(GenericPage<ProjectVM> genericPage)
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
            return View(new GenericPage<ProjectVM>
            {
                SearchString = genericPage.SearchString,
                SortField = genericPage.SortField,
                PreviousSortField = genericPage.SortField,
                SortOrder = genericPage.SortOrder,

                CurrentPageIndex = genericPage.CurrentPageIndex,
                PaginatedList = new PaginatedList<ProjectVM>(paginatedListDTO.Select(dto => _mapper.Map<ProjectVM>(dto)).ToList(), paginatedListDTO.MatchCount, 
                    genericPage.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit)
            });
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(p => p.Id == id.Value);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProjectVM>(result.Value));
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Project")] ProjectVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AddAsync(_mapper.Map<ProjectDTO>(viewModel));
                if (result.IsSuccess)
                {
                    TempData[TempDataKeys.SaveMode] = SaveMode.Create;
                    return RedirectToAction(nameof(Success), _mapper.Map<ProjectDTO>(result.Value));
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

        // GET: Projects/Edit/5
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

            return View(_mapper.Map<ProjectVM>(result.Value));
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Project")] ProjectVM viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(_mapper.Map<ProjectDTO>(viewModel));
                if (result.IsSuccess)
                {
                    TempData[TempDataKeys.SaveMode] = SaveMode.Edit;
                    return RedirectToAction(nameof(Success), _mapper.Map<ProjectDTO>(result.Value));
                }
                else if (result.ErrorType == ErrorType.Defined)
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await ProjectExists(id))
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

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.FirstOrDefaultAsync(p => p.Id == id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProjectVM>(result.Value));
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (result.IsSuccess)
            {
                TempData[TempDataKeys.SaveMode] = SaveMode.Delete;
                return RedirectToAction(nameof(Success), _mapper.Map<ProjectDTO>(result.Value));
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

        private async Task<bool> ProjectExists(long id)
        {
            var result = await _service.AnyAsync(p => p.Id == id);
            if (!result.IsSuccess)
            {
                RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
                return false;
            }

            return result.Value;
        }

        public IActionResult Success(ProjectVM viewModel)
        {
            SaveMode saveMode = (SaveMode)Convert.ToInt32(TempData[TempDataKeys.SaveMode]);
            ViewBag.SaveMode = saveMode.ToString();

            switch (saveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    ViewBag.Message = "Successfully saved project.";
                    break;

                case SaveMode.Delete:
                    ViewBag.Message = "Successfully deleted project.";
                    break;

                case SaveMode.None:
                default:
                    break;
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Assignments(long? id, AssignmentPageFilterVM pageFilter)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectResult = await _service.FindByIdAsync(id.Value);
            if (!projectResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Project = projectResult.Value.Project;
            ViewBag.Project_Id = projectResult.Value.Id;
            if (!pageFilter.SortField.Equals(pageFilter.PreviousSortField))
            {
                pageFilter.SortOrder = SortOrder.Ascending;
            }
            else if (pageFilter.IsHeaderClicked)
            {
                pageFilter.SortOrder = pageFilter.SortOrder.Reverse();
            }

            var pageFilterDTO = _mapper.Map<AssignmentPageFilterDTO>(pageFilter);
            pageFilterDTO.Project_Id = projectResult.Value.Id;
            pageFilterDTO.PageSize = _pageSettings.PageSize;

            var result = await _service.CreatePageListAsync(pageFilterDTO);
            var roleResult = await _roleService.GetListAsync();
            if (!result.IsSuccess || !roleResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            var paginatedListDTO = result.Value;
            ViewBag.Roles = roleResult.Value.Select(r => _mapper.Map<RoleVM>(r)).ToList();

            return View(new AssignmentPageFilterVM
            {
                SearchString = pageFilter.SearchString,
                Project_Id = pageFilter.Project_Id,
                Role_Id = pageFilter.Role_Id,

                SortField = pageFilter.SortField,
                PreviousSortField = pageFilter.SortField,
                SortOrder = pageFilter.SortOrder,
                CurrentPageIndex = pageFilter.CurrentPageIndex,

                PaginatedList = new PaginatedList<AssignmentVM>(paginatedListDTO.Select(dto => _mapper.Map<AssignmentVM>(dto)).ToList(), paginatedListDTO.MatchCount,
                    pageFilter.CurrentPageIndex, _pageSettings.PageSize, _pageSettings.PageIndexViewLimit),
            });
        }

        public async Task<IActionResult> CreateAssignment(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectResult = await _service.FindByIdAsync(id.Value);
            if (!projectResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }
            else if (projectResult.Value == null)
            {
                return NotFound();
            }

            var employeeResult = await _employeeService.GetListAsync();
            var roleResult = await _roleService.GetListAsync();
            if (!employeeResult.IsSuccess
                || !roleResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Employees = _mapper.Map<List<EmployeeVM>>(employeeResult.Value).Select(e => new
            {
                Id = e.Id,
                Name = $"{ e.LastName }, { e.FirstName }"
            });

            ViewBag.Roles = _mapper.Map<List<RoleVM>>(roleResult.Value);

            return View(new AssignmentVM
            {
                Project = projectResult.Value.Project,
                Project_Id = projectResult.Value.Id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAssignment([Bind("Project,Project_Id,Employee_Id,Role_Id")] AssignmentVM viewModel)
        {
            var result = await _assignmentService.AddAsync(_mapper.Map<AssignmentDTO>(viewModel));
            if (result.IsSuccess)
            {
                TempData[TempDataKeys.SaveMode] = SaveMode.Create;
                return RedirectToAction(nameof(AssignSuccess), _mapper.Map<AssignmentVM>(result.Value));
            }
            else if (result.ErrorType == ErrorType.Defined)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
            }
            else
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            var projectResult = await _service.FindByIdAsync(viewModel.Project_Id);
            if (!projectResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }
            else if (projectResult.Value == null)
            {
                return NotFound();
            }

            var employeeResult = await _employeeService.GetListAsync();
            var roleResult = await _roleService.GetListAsync();
            if (!employeeResult.IsSuccess
                || !roleResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Project = projectResult.Value.Project;
            ViewBag.Project_Id = projectResult.Value.Id;
            ViewBag.Employees = _mapper.Map<List<EmployeeVM>>(employeeResult.Value).Select(e => new
            {
                Id = e.Id,
                Name = $"{ e.LastName }, { e.FirstName }"
            });

            ViewBag.Roles = _mapper.Map<List<RoleVM>>(roleResult.Value);

            return View(viewModel);
        }

        public async Task<IActionResult> AssignmentDetail(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _assignmentService.FindByIdAsync(id.Value);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }
            else if (result.Value == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AssignmentVM>(result.Value));
        }

        public IActionResult AssignSuccess(AssignmentVM viewModel)
        {
            SaveMode saveMode = (SaveMode)Convert.ToInt32(TempData[TempDataKeys.SaveMode]);
            ViewBag.SaveMode = saveMode.ToString();

            switch (saveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    ViewBag.Message = "Successfully saved project assignment.";
                    break;

                case SaveMode.Delete:
                    ViewBag.Message = "Successfully deleted project assignment.";
                    break;

                case SaveMode.None:
                default:
                    break;
            }

            return View(viewModel);
        }

        public async Task<IActionResult> EditAssignment(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _assignmentService.FindByIdAsync(id.Value);
            var employeeResult = await _employeeService.GetListAsync();
            var roleResult = await _roleService.GetListAsync();
            if (!result.IsSuccess
                || !employeeResult.IsSuccess
                || !roleResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Employees = _mapper.Map<List<EmployeeVM>>(employeeResult.Value).Select(e => new
            {
                Id = e.Id,
                Name = $"{ e.LastName }, { e.FirstName }"
            });

            ViewBag.Roles = _mapper.Map<List<RoleVM>>(roleResult.Value);

            return View(new AssignmentVM
            {
                Id = result.Value.Id,
                Project = result.Value.Project,
                Project_Id = result.Value.Project_Id,
                Employee_Id = result.Value.Employee_Id,
                Role_Id = result.Value.Role_Id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAssignment([Bind("Id,Project,Project_Id,Employee_Id,Role_Id")] AssignmentVM viewModel)
        {
            var result = await _assignmentService.UpdateAsync(_mapper.Map<AssignmentDTO>(viewModel));
            if (result.IsSuccess)
            {
                TempData[TempDataKeys.SaveMode] = SaveMode.Edit;
                return RedirectToAction(nameof(AssignSuccess), _mapper.Map<AssignmentVM>(result.Value));
            }
            else if (result.ErrorType == ErrorType.Defined)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
            }
            else
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            var projectResult = await _service.FindByIdAsync(viewModel.Project_Id);
            if (!projectResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }
            else if (projectResult.Value == null)
            {
                return NotFound();
            }

            var employeeResult = await _employeeService.GetListAsync();
            var roleResult = await _roleService.GetListAsync();
            if (!employeeResult.IsSuccess
                || !roleResult.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            ViewBag.Project = projectResult.Value.Project;
            ViewBag.Project_Id = projectResult.Value.Id;
            ViewBag.Employees = _mapper.Map<List<EmployeeVM>>(employeeResult.Value).Select(e => new
            {
                Id = e.Id,
                Name = $"{ e.LastName }, { e.FirstName }"
            });

            ViewBag.Roles = _mapper.Map<List<RoleVM>>(roleResult.Value);

            return View(viewModel);
        }

        public async Task<IActionResult> DeleteAssignment(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _assignmentService.FindByIdAsync(id.Value);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(ErrorsController.ApplicationExceptionLevelHandler), "Errors");
            }

            return View(new AssignmentVM
            {
                Id = result.Value.Id,
                Project = result.Value.Project,
                Project_Id = result.Value.Project_Id,
                Employee_Id = result.Value.Employee_Id,
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
                Role_Id = result.Value.Role_Id,
                Role = result.Value.Role
            });
        }

        [HttpPost, ActionName("DeleteAssignment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAssignment(long id)
        {
            var result = await _assignmentService.DeleteByIdAsync(id);
            if (result.IsSuccess)
            {
                TempData[TempDataKeys.SaveMode] = SaveMode.Delete;
                return RedirectToAction(nameof(AssignSuccess), _mapper.Map<AssignmentDTO>(result.Value));
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
    }
}
