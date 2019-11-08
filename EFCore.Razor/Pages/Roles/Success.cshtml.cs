using EFCore.Common;
using EFCore.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EFCore.Razor.Pages.Roles
{
    public class SuccessModel : PageModel
    {
        [TempData]
        public SaveMode SaveMode { get; set; } = SaveMode.None;

        public RoleModel Role { get; set; }

        public string Message { get; set; }

        public void OnGet(RoleModel model)
        {
            this.Role = model;

            switch (SaveMode)
            {
                case SaveMode.Create:
                case SaveMode.Edit:
                    Message = "Successfully saved role.";
                    break;

                case SaveMode.Delete:
                    Message = "Successfully deleted role.";
                    break;
                case SaveMode.None:
                default:
                    break;
            }
        }
    }
}