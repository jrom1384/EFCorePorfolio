using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //https://app.freeprivacypolicy.com/builder/download/retrieve?token=512b85efc278ab6024c6e873926565a9
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
