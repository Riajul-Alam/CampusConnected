using CampusConnected.Migrations;
using CampusConnected.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CampusConnected.Controllers
{
    public class TeacherController : Controller
    {

        private readonly StudentDBContext studentDB;
        public TeacherController(StudentDBContext studentDB)
        {
            this.studentDB = studentDB;
        }

        // GET: TeacherController

        //This part is Updated by Mizan 
        /*        public async Task<IActionResult> Index()
                {
                    var stdData = await studentDB.Teachers.ToListAsync();

                    return View(stdData);
                }
                private Teacher BindModel()
                {
                    //fetch department table date 
                    var departments = studentDB.Departments.ToList();

                    var teacher = new Teacher
                    {
                        DepartmentList = departments
                    };
                    return teacher;
                }
                public IActionResult Create()
                {
                    var stdModel = BindModel();
                    return View(stdModel);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]

                public async Task<IActionResult> Create(Teacher std)
                {


                    if (std.Id != null)
                    {
                        var d = studentDB.Departments.FirstOrDefault(x => x.DId == std.DepartmentId);
                        std.TeacherDepartment = d.Name;
                        await studentDB.Teachers.AddAsync(std);
                        await studentDB.SaveChangesAsync();

                        return RedirectToAction("Index", "Teacher");
                    }

                    //}


                    return View(std);


                }


                public IActionResult Login()
                {
                    if (HttpContext.Session.GetString("UserSession") != null)
                    {
                        return RedirectToAction("Login");
                    }
                    return View();
                }

                [HttpPost]
                public IActionResult Login(Teacher user)
                {
                    var myUser = studentDB.Teachers.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
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

                public IActionResult Logout()
                {
                    if (HttpContext.Session.GetString("UserSession") != null)
                    {
                        HttpContext.Session.Remove("UserSession");
                        return RedirectToAction("Main", "Home");
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
                public IActionResult ShowStudentDetails()
                {
                    if (HttpContext.Session.GetString("UserSession") != null)
                    {
                        ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                    var studentsList = studentDB.Students.ToList();
                    return RedirectToAction("StudentList", "Home");
                }*/
        // End of This Part








        // my Part start from here

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("TeacherSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Teacher user)
        {
            var myUser = studentDB.Teachers.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("TeacherSession", myUser.Email);
                return RedirectToAction("Dashboard","Teacher");
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
            if (HttpContext.Session.GetString("TeacherSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("TeacherSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("TeacherSession") != null)
            {
                HttpContext.Session.Remove("TeacherSession");
                return RedirectToAction("Main", "Home");
            }
            return View();
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AdminSession")== null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Teacher user)
        {
            if (HttpContext.Session.GetString("AdminSession")== null)
            {
                return RedirectToAction("Dashboard");
            }
            if (ModelState.IsValid)
            {
                await studentDB.Teachers.AddAsync(user);
                await studentDB.SaveChangesAsync();
                TempData["Succees"] = "Registered Successfully";
                return RedirectToAction("Create","Teacher");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("TeacherSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("TeacherSession").ToString();
                string userEmail = HttpContext.Session.GetString("TeacherSession");

                Teacher student = studentDB.Teachers.
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

        public async Task<IActionResult> StudentList()
        {
            if (HttpContext.Session.GetString("TeacherSession") == null)
            {
                return RedirectToAction("Login");
            }
            var stdData = await studentDB.Students.ToListAsync();

            return View(stdData);
        }











        //Dummy
        public IActionResult DummyAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DummyAdd(Teacher user)
        {
            if (ModelState.IsValid)
            {
                await studentDB.Teachers.AddAsync(user);
                await studentDB.SaveChangesAsync();
                TempData["Succees"] = "Registered Successfully";
                return RedirectToAction("DummyAdd");
            }
            return View();
        }




    }
}

