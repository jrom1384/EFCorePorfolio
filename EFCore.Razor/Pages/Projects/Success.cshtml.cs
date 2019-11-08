using EFCore.Common;
using EFCore.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EFCore.Razor.Pages.Projects
{
    public class SuccessModel : PageModel
    {
        [TempData]
        public SaveMode SaveMode { get; set; } = SaveMode.None;

        public ProjectModel Project { get; set; }

        public string Message { get; set; }

        public void OnGet(ProjectModel model)
        {
            this.Project = model;

            switch (SaveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    Message = "Successfully saved project.";
                    break;

                case SaveMode.Delete:
                    Message = "Successfully deleted project.";
                    break;
                case SaveMode.None:
                default:
                    break;
            }
        }
    }
}