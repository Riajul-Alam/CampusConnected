using CampusConnected.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnected.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentDBContext studentDB;

        public StudentController(StudentDBContext studentDB)
        {
            this.studentDB = studentDB;
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
            var myUser = studentDB.Students.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("StudentSession", myUser.Email);
                return RedirectToAction("Dashboard", "Student");
            }
            else
            {
                ViewBag.Message = "Login Failed..";
            }
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("StudentSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("StudentSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("StudentSession") != null)
            {
                HttpContext.Session.Remove("StudentSession");
                return RedirectToAction("Main", "Home");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("StudentSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("StudentSession").ToString();
                string userEmail = HttpContext.Session.GetString("StudentSession");

                Student student = studentDB.Students.
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
		public List<string> getAvailableCourse(String registeredCourse)
		{
			List<string> cList = new List<string>();

			var courses = studentDB.Courses.ToList();
			string[] values = registeredCourse.Split(',');
			List<int> idList = values.Select(int.Parse).ToList();

			foreach (var course in courses)
			{
				int courseId = course.CId;
				if (!idList.Contains(courseId))
					cList.Add(course.CourseName);
			}
			return cList;
		}
		public List<string> getRegisteredCourse(String registeredCourse)
		{
			List<string> cList = new List<string>();

			var courses = studentDB.Courses.ToList();
			string[] values = registeredCourse.Split(',');
			List<int> idList = values.Select(int.Parse).ToList();

			foreach (var course in courses)
			{
				int courseId = course.CId;
				if (idList.Contains(courseId))
					cList.Add(course.CourseName);
			}
			return cList;
		}
		/*		[HttpGet]
                //Show registered Course Details
                public IActionResult ShowRegisteredCourses()
                {
                    int currentStudentId;
                    if (HttpContext.Session.GetString("UserSession") != null)
                    {
                        ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                        string userEmail = HttpContext.Session.GetString("UserSession");
                        Student student = studentDB.Students.FirstOrDefault(s => s.Email == userEmail);
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
                }*/
		//Show registered Course Details END HERE

		// Offered Courses here
		/*        public IActionResult ShowOfferedCourses()
				{
					int currentStudentId;
					if (HttpContext.Session.GetString("UserSession") != null)
					{
						ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
						string userEmail = HttpContext.Session.GetString("UserSession");
						Student student = studentDB.Students.FirstOrDefault(s => s.Email == userEmail);
						ViewBag.MySession2=student.Id;
						currentStudentId=student.Id;
					}
					else
					{
						return RedirectToAction("Login");
					}
					// Get the current student's ID (Replace this with your logic to fetch the current student ID)


					// Fetch courses that the current student has not registered for
					var unregisteredCourses = studentDB.Courses
						.Where(c => !studentDB.RegisteredCourses.Any(rc => rc.StudentId == currentStudentId && rc.CourseCode == c.CourseCode))
						.ToList();

					return View(unregisteredCourses);
				}*/
	}
}
