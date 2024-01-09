using Connected_Campus.Data;
using Connected_Campus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connected_Campus.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;

        public AdminController(ApplicationDbContext context)
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
        public IActionResult Login(Administration user)
        {
            var myUser = context.Administrations.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
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

        // Show student details
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
            var studentsList = context.Students.ToList();
            return View(studentsList);
        }

        //create Student
        public IActionResult Create()
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
        [HttpPost]
        public async  Task<IActionResult> Create(Student user)
        {
            if (ModelState.IsValid)
            {
                await context.Students.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Insert_Succees"] = "Inserted Successfully";
                return RedirectToAction("ShowStudentDetails");
            }
            return View(user);
        }

        // Details
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            if(id==null || context.Students==null)
            {
                return NotFound();
            }
            var studentData = await context.Students.FirstOrDefaultAsync(x=>x.Id==id);
            if(studentData==null)
            {
                return NotFound();
            }
            return View(studentData);
        }


        //Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            if (id==null || context.Students==null)
            {
                return NotFound();
            }
            var studentData = await context.Students.FindAsync(id);
            if (studentData==null)
            {
                return NotFound();
            }
            return View(studentData);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Student user,int? id)
        {
            if (id!=user.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Students.Update(user);
                await context.SaveChangesAsync();
                TempData["Edit_Succees"] = "Updated Successfully";
                return RedirectToAction("ShowStudentDetails");
            }
            return View(user);
        }

        //Delete
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var studentData = await context.Students.FindAsync(id);
            if(studentData!=null)
            {
                context.Students.Remove(studentData);
            }
            await context.SaveChangesAsync();
            TempData["Delete_Succees"] = "Deleted Successfully";
            return RedirectToAction("ShowStudentDetails");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            if (id==null || context.Students==null)
            {
                return NotFound();
            }
            var studentData = await context.Students.FirstOrDefaultAsync(x => x.Id==id);
            if (studentData==null)
            {
                return NotFound();
            }
            return View(studentData);
        }

        // Teacher CRUD Start here
        // Show student details
        public IActionResult ShowTeacherDetails()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login","Admin");
            }
            var teachersList = context.Teachers.ToList();
            return View(teachersList);
        }

        //create Student
        public IActionResult CreateTeacher()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login","Admin");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTeacher(Teacher user)
        {
            if (ModelState.IsValid)
            {
                await context.Teachers.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Insert_Succees"] = "Inserted Successfully";
                return RedirectToAction("ShowTeacherDetails");
            }
            return View(user);
        }

        // Details
        public async Task<IActionResult> TeacherDetails(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            if (id==null || context.Teachers==null)
            {
                return NotFound();
            }
            var teacherData = await context.Teachers.FirstOrDefaultAsync(x => x.Id==id);
            if (teacherData==null)
            {
                return NotFound();
            }
            return View(teacherData);
        }


        //Edit
        public async Task<IActionResult> EditTeacher(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login","Admin");
            }
            if (id==null || context.Teachers==null)
            {
                return NotFound();
            }
            var teacherData = await context.Teachers.FindAsync(id);
            if (teacherData==null)
            {
                return NotFound();
            }
            return View(teacherData);
        }
        [HttpPost]
        public async Task<IActionResult> EditTeacher(Teacher user, int? id)
        {
            if (id!=user.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.Teachers.Update(user);
                await context.SaveChangesAsync();
                TempData["Edit_Succees"] = "Updated Successfully";
                return RedirectToAction("ShowTeacherDetails");
            }
            return View(user);
        }

        //Delete
        [HttpPost, ActionName("DeleteTeacher")]
        public async Task<IActionResult> DeleteTeacherConfirmed(int? id)
        {
            var teacherData = await context.Teachers.FindAsync(id);
            if (teacherData!=null)
            {
                context.Teachers.Remove(teacherData);
            }
            await context.SaveChangesAsync();
            TempData["Delete_Succees"] = "Deleted Successfully";
            return RedirectToAction("ShowTeacherDetails");
        }
        public async Task<IActionResult> DeleteTeacher(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login","Admin");
            }
            if (id==null || context.Teachers==null)
            {
                return NotFound();
            }
            var teacherData = await context.Teachers.FirstOrDefaultAsync(x => x.Id==id);
            if (teacherData==null)
            {
                return NotFound();
            }
            return View(teacherData);
        }
        //Teacher CRUD END HERE





        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
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
        public async Task<IActionResult> DummyAdd(Administration user)
        {
            if (ModelState.IsValid)
            {
                await context.Administrations.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Succees"] = "Registered Successfully";
                return RedirectToAction("DummyAdd");
            }
            return View();
        }

        //Dummy
        public IActionResult DummyAdd1()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DummyAdd1(Teacher user)
        {
            if (ModelState.IsValid)
            {
                await context.Teachers.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Succees"] = "Registered Successfully";
                return RedirectToAction("DummyAdd");
            }
            return View();
        }
    }
}
