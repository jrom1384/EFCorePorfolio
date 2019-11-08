using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace EFCore.Razor.Pages.Errors
{
    public class GlobalExceptionLevelHandlerModel : PageModel
    {
        public void OnGet()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Log.Error(exceptionDetails.Error, string.Empty);
        }
    }
}