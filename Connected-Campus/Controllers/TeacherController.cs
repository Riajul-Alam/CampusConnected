using Microsoft.AspNetCore.Mvc;

namespace Connected_Campus.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
