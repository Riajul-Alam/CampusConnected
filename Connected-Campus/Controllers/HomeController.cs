using Connected_Campus.Data;
using Connected_Campus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Connected_Campus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
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
                HttpContext.Session.SetString("UserSession", myUser.Email);
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Login Failed..";
            }
            return View();
        }
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                string userEmail = HttpContext.Session.GetString("UserSession");

                Student student = context.Students.
                    FirstOrDefault(s => s.Email == userEmail);

                if (student != null)
                {
                    return View(student);
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // CourseRegistration start Here
        public IActionResult CourseRegistration()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                string userEmail = HttpContext.Session.GetString("UserSession");
                Student student = context.Students.FirstOrDefault(s => s.Email == userEmail);
                ViewBag.MySession2=student.Id;
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CourseRegistration(RegisteredCourse user)
        {
            if (ModelState.IsValid)
            {
                await context.RegisteredCourses.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Succees"] = "Registered Successfully";
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        // CourseRegistration END Here

        [HttpGet]
        //Show registered Course Details
        public IActionResult ShowRegisteredCourses()
        {
            int currentStudentId;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                string userEmail = HttpContext.Session.GetString("UserSession");
                Student student = context.Students.FirstOrDefault(s => s.Email == userEmail);
                ViewBag.MySession2=student.Id;
                currentStudentId=student.Id;
            }
            else
            {
                return RedirectToAction("Login");
            }
            // Get the current student's ID (Replace this with your logic to fetch the current student ID)

            var registeredCourses = context.RegisteredCourses
                .Where(rc => rc.StudentId == currentStudentId)
                .Select(rc => new Connected_Campus.Models.RegisteredCourse
                {
                    StudentId = rc.StudentId,
                    CourseCode = rc.CourseCode,
                    CourseTitle = rc.CourseTitle,
                    CreditHours = rc.CreditHours
                })
                .ToList();

            return View(registeredCourses);
        }
        //Show registered Course Details END HERE

        // Offered Courses here
        public IActionResult ShowOfferedCourses()
        {
            int currentStudentId;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                string userEmail = HttpContext.Session.GetString("UserSession");
                Student student = context.Students.FirstOrDefault(s => s.Email == userEmail);
                ViewBag.MySession2=student.Id;
                currentStudentId=student.Id;
            }
            else
            {
                return RedirectToAction("Login");
            }
            // Get the current student's ID (Replace this with your logic to fetch the current student ID)


            // Fetch courses that the current student has not registered for
            var unregisteredCourses = context.Courses
                .Where(c => !context.RegisteredCourses.Any(rc => rc.StudentId == currentStudentId && rc.CourseCode == c.CourseCode))
                .ToList();

            return View(unregisteredCourses);
        }






        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Student user)
        {
            if (ModelState.IsValid)
            {
                await context.Students.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Succees"] = "Registered Successfully";
                return RedirectToAction("Login");
            }
            return View();
        }


        //Dummy
        public IActionResult DummyAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DummyAdd(Course user)
        {
            if (ModelState.IsValid)
            {
                await context.Courses.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Succees"] = "Registered Successfully";
                return RedirectToAction("DummyAdd");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}