using EFCore.Common;
using EFCore.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EFCore.Razor.Pages.Projects
{
    public class AssignSuccessModel : PageModel
    {
        [TempData]
        public SaveMode SaveMode { get; set; } = SaveMode.None;

        public AssignmentModel Assignment { get; set; }

        public string Message { get; set; }

        public void OnGet(AssignmentModel model)
        {
            this.Assignment = model;

            switch (SaveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    Message = "Successfully saved project assignment.";
                    break;

                case SaveMode.Delete:
                    Message = "Successfully deleted project assignment.";
                    break;
                case SaveMode.None:
                default:
                    break;
            }
        }
    }
}