using EFCore.Common;
using EFCore.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EFCore.Razor.Pages.Employees
{
    public class SuccessModel : PageModel
    {
        [TempData]
        public SaveMode SaveMode { get; set; } = SaveMode.None;

        public EmployeeModel Employee { get; set; }

        public string Message { get; set; }

        public void OnGet(EmployeeModel model)
        {
            this.Employee = model;

            switch (SaveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    Message = "Successfully saved employee.";
                    break;

                case SaveMode.Delete:
                    Message = "Successfully deleted employee.";
                    break;
                case SaveMode.None:
                default:
                    break;
            }
        }
    }
}