using Microsoft.AspNetCore.Mvc;

namespace Expenses_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
