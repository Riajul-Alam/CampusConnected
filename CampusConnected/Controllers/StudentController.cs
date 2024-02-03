using CampusConnected.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnected.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentDBContext context;

        public StudentController(StudentDBContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("StudentSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Student user)
        {
            var myUser = context.Students.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("StudentSession", myUser.Email);
                return RedirectToAction("StudentDashboard");
            }
            else
            {
                ViewBag.Message = "Login Failed..";
            }
            return View();
        }
        public IActionResult StudentDashboard()
        {
            // Your logic for the Main action
            return View();
        }
    }
}
