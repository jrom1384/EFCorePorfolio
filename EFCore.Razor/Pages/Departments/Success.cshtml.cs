using EFCore.Common;
using EFCore.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EFCore.Razor.Pages.Departments
{
    public class SuccessModel : PageModel
    {
        [TempData]
        public SaveMode SaveMode { get; set; } = SaveMode.None;

        public DepartmentModel Department { get; set; }

        public string Message { get; set; }

        public void OnGet(DepartmentModel model)
        {
            this.Department = model;

            switch (SaveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    Message = "Successfully saved department.";
                    break;

                case SaveMode.Delete:
                    Message = "Successfully deleted department.";
                    break;
                case SaveMode.None:
                default:
                    break;
            }
        }
    }
}