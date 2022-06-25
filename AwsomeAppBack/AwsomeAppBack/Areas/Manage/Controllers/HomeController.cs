using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwsomeAppBack.Areas.Manage.Controllers
{
    public class HomeController : Controller
    {
        [Area("Manage")]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
