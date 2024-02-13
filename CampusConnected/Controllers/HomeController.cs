using CampusConnected.Models;
using CampusConnected.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CampusConnected.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;
        //private readonly StudentRepository _studentRepository = null;


        //data base table rec update 
        private readonly StudentDBContext studentDB;
        public HomeController(StudentDBContext studentDB)
        {
            this.studentDB = studentDB;
        }



        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //    _studentRepository=new StudentRepository();
        //}


        //public List<Student> getAllStudents()
        //{
        //    return _studentRepository.getAllStudents();
        //}
        //public Student getById(int id)
        //{
        //    return _studentRepository.getStudentById(id);
        //}


        public static bool IsEmailValid(string email)
        {
            // Define the regular expression pattern for a valid email address
            string emailPattern = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$";

            // Check if the email matches the pattern
            if (Regex.IsMatch(email, emailPattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IActionResult Main()
        {
            // Your logic for the Main action
            return View();
        }
        public IActionResult StudentDashboard()
        {
            // Your logic for the Main action
            return View();
        }


        public async Task<IActionResult> StudentList()
        {
            var stdData = await studentDB.Students.ToListAsync();

            return View(stdData);
        }

        private Student BindModel()
        {
            //fetch department table date 
            var departments = studentDB.Departments.ToList();

            var student = new Student
            {
                DepartmentList = departments
            };
            return student;
        }



        //action for open create view page
        public IActionResult Create()
        {
            var stdModel = BindModel();
            return View(stdModel);

            
        }

        [HttpGet]
        public IActionResult IsDepartmentInFaculty(string facultyId, int departmentId)
        {
         
            var dep = studentDB.Departments.FirstOrDefault(d => d.DId == departmentId);

            string depF = dep.Faculty.ToString();

            bool isValid=false;

            //if (facultyId == depF) isValid = true;

            if (facultyId == "0" && depF == "Arts") isValid = true;
            if (facultyId == "1" && depF == "Engineering") isValid = true;
            if (facultyId == "2" && depF == "Business") isValid = true;
            if (facultyId == "3" && depF == "Pharmacy") isValid = true;
           

            //Console.WriteLine(facultyId);
            //Console.WriteLine(departmentId);
            //Console.WriteLine(depF);

            //Console.WriteLine("------------------",isValid);
            //Console.WriteLine(isValid);


            // Return the result as JSON
            return Json(new { isValid });
        }


        //action for submit create view page data, httppost = submit data
        //get student model data and add to student database
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Student std)
        {
            //var stdModel = BindModel();
         

            
            //if (ModelState.IsValid)
            //{

           

            if (std.Id != null)
            {
                if (!IsEmailValid(std.Email)) return View(std);

                var dep = studentDB.Departments.FirstOrDefault(d => d.DId == std.DepartmentId);

                var depF = dep.Faculty.ToString();
                var stdF = std.Faculty.ToString();
                //if(depF != stdF)
                //{
                //    return View(std);
                //}

                //if (!IsValidDeptAndFaculty(std.Faculty.ToString(), std.DepartmentName)) return View(std);


                std.DepartmentName = dep.Name;
                string nm  = dep.Name;
                int num = dep.studentNumber+1;
                int x = num;
                string new_id = nm+"_" + num.ToString();
                dep.studentNumber = x + 1;
                //std.StudentId = new_id;
                std.Grade = 0.0;
                std.CourseComplete = "";
                //std.Password = new_id;
                await studentDB.Students.AddAsync(std);
                await studentDB.SaveChangesAsync();
                TempData["insert_sucess"] = "Record Created Sucessfully!";

                return RedirectToAction("StudentList", "Home");
            }

            //}

          
            return View(std);


        }

        //end of post 



        //edit method view open
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || studentDB.Students == null)
            {
                return NotFound();
            }
            var editData = await studentDB.Students.FindAsync(id);
            if (editData == null) { return NotFound(); }
            //var data = BindModel();
            editData.DepartmentList = studentDB.Departments.ToList();
            return View(editData);
        }

        //edit method save 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Student std)
        {
            if (id != std.Id) { return NotFound(std); }
            if (std.Id!=null)
            {
                var dep = studentDB.Departments.FirstOrDefault(d => d.DId == std.DepartmentId);
                std.DepartmentName = dep.Name;
                //var stu = studentDB.Students.FirstOrDefault(s => s.Id == id);
                //std.StudentId = stu.StudentId;
                studentDB.Students.Update(std);
                await studentDB.SaveChangesAsync();
                TempData["update_sucess"] = "Record Updated Sucessfully!";
                return RedirectToAction("StudentList", "Home");
            }
            return View(std);
        }

        //details method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || studentDB.Students == null)
            {
                return NotFound();
            }
            var detailsData = await studentDB.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (detailsData == null)
            {
                return NotFound();
            }
            return View(detailsData);
        }



        //delete method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || studentDB.Students == null)
            {
                return NotFound();
            }
            var deletedData = await studentDB.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (deletedData == null)
            {
                return NotFound();
            }
            return View(deletedData);
        }


        //delete button action

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var deletedData = await studentDB.Students.FindAsync(id);
            if (deletedData != null)
            {
                studentDB.Students.Remove(deletedData);
            }
            await studentDB.SaveChangesAsync();
            TempData["delete_sucess"] = "Record Deleted Sucessfully!";
            return RedirectToAction("StudentList", "Home");
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
        public IActionResult Login(Student user)
        {
            var myUser = studentDB.Students.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);
                return RedirectToAction("StudentDashboard", "Home");
            }
            else
            {
                ViewBag.Message = "Login Failed..";
            }
            return View();
        }

        //search related
        [HttpGet]
        public async Task<IActionResult> StudentList(string stdsearch)
        {
            ViewData["GetStddetails"] = stdsearch;

            var stdquery = from x in studentDB.Students select x;
            if (!string.IsNullOrEmpty(stdsearch))
            {
                stdquery = stdquery.Where(x => x.StudentName.Contains(stdsearch) || x.StudentId.Contains(stdsearch));
            }
            return View(await stdquery.AsNoTracking().ToListAsync());
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