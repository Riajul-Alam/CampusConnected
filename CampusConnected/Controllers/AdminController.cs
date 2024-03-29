﻿using CampusConnected.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnected.Controllers
{
    public class AdminController : Controller
    {
        private readonly StudentDBContext context;

        public AdminController(StudentDBContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Admin user)
        {
            var myUser = context.Admins.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("AdminSession", myUser.Email);
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Login Failed..";
            }
            return View();
        }
/*        public IActionResult GetSession()
        {
            string userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View();
        }*/

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("AdminSession").ToString();
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
			if (HttpContext.Session.GetString("AdminSession") != null)
			{
				ViewBag.MySession = HttpContext.Session.GetString("AdminSession").ToString();
				string userEmail = HttpContext.Session.GetString("AdminSession");

				Admin student = context.Admins.
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

		public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                HttpContext.Session.Remove("AdminSession");
                return RedirectToAction("Main","Home");
            }
            return View();
        }
        public IActionResult ShowStudentDetails()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("AdminSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            var studentsList = context.Students.ToList();
            return RedirectToAction("StudentList", "Home");
        }

        public async Task<IActionResult> TeacherList()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("Dashboard");
            }
            var stdData = await context.Teachers.ToListAsync();
            return View(stdData);
        }

        //Search
        [HttpGet]
        public async Task<IActionResult> TeacherList(string Tsearch)
        {
            ViewData["GetTdetails"] = Tsearch;

            var Tquery = from x in context.Teachers select x;
            if (!string.IsNullOrEmpty(Tsearch))
            {
                Tquery = Tquery.Where(x => x.TeacherName.Contains(Tsearch) || x.TeacherDepartment.Contains(Tsearch));
            }
            return View(await Tquery.AsNoTracking().ToListAsync());
        }




















        //Admin
        //Dummy
        public IActionResult DummyAdd()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> DummyAdd(Admin user)
		{
			if (ModelState.IsValid)
			{
				await context.Admins.AddAsync(user);
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
                return RedirectToAction("DummyAdd1");
            }
            return View();
        }
        public IActionResult DummyAdd2()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> DummyAdd2(Student user)
		{
			if (ModelState.IsValid)
			{
				await context.Students.AddAsync(user);
				await context.SaveChangesAsync();
				TempData["Succees"] = "Registered Successfully";
				return RedirectToAction("DummyAdd2");
			}
			return View();
		}

	}
}