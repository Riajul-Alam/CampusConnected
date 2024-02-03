using CampusConnected.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CampusConnected.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CampusConnected.Controllers
{
    public class ResultReportController : Controller
    {
        private readonly StudentDBContext studentDB;
        public ResultReportController(StudentDBContext studentDB)
        {
            this.studentDB = studentDB;
        }

        public IActionResult Index()
        {
            var departments = studentDB.Departments.ToList();
            var students = studentDB.Students.ToList();
            var obj = new ResultReport
            {
                StudentList = students,
                DepartmentList = departments,
            };

            ViewData["ViewModelData"] = obj;

            var sortedResults = studentDB.studentResult.OrderByDescending(result => result.Grade).ToList();
            ViewData["SortedResults"] = sortedResults;

            return View();
        }

        public List<string> getCourseList(String stdCourseList)
        {
            List<string> cList = new List<string>();
            string[] courseIdStrings = stdCourseList.Split(',');
            int[] courseIdArray = courseIdStrings.Select(int.Parse).ToArray();

            var courses = studentDB.Courses.ToList();
            Dictionary<int, string> myDictionary = new Dictionary<int, string>();
            foreach (var course in courses)
            {
                int courseId = course.CId;
                if (courseIdArray.Contains(courseId))
                {
                    string nameofCourse = course.CourseName;
                    myDictionary.Add(courseId, nameofCourse);
                }
            }

            foreach (int num in courseIdArray)
            {
                string coursename = myDictionary[num];
                cList.Add(coursename);
            }

            return cList;

            //for showing and storing course with registration serile and for storing marks in this serial
        }


        public List<int> get_grade(string inputString)
        {
            string[] pairs = inputString.Split(',');

            List<int> yValues = new List<int>();

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split(':');

                // Check if there are at least two parts in the pair
                if (keyValue.Length >= 2)
                {
                    // Extracting the value after ':'
                    string valueString = keyValue[1];

                    // Parsing the value as an integer
                    if (int.TryParse(valueString, out int intValue))
                    {
                        yValues.Add(intValue);
                    }
                }
                else yValues.Add(0);
            }
            return yValues;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(ResultReport std)
        {

            if (std.StudentId != null)
            {
                var studentIdExists = studentDB.studentResult.Any(s => s.StudentId == std.StudentId);

                if (studentIdExists)
                {


                    //var studentResult = studentDB.studentResult.FirstOrDefault(s => s.StudentId == std.StudentId);
                    var matchingRecords = studentDB.studentResult.Where(s => s.StudentId == std.StudentId).ToList();
                    ResultSubmission? studentResult = null;


                    var stdRec = studentDB.Students.FirstOrDefault(y => y.Id == std.StudentId);

                    ViewData["StudentId"] = stdRec.StudentId;
                    ViewData["StudentName"] = stdRec.StudentName;
                    ViewData["DepartmentId"] = stdRec.DepartmentName;
                    ViewData["Email"] = stdRec.Email;
                    ViewData["grade"] = stdRec.Grade;

                    ViewData["CourseList0"] = new List<string>();
                    ViewData["Grades0"] = new List<double>();
                    ViewData["GradeDescriptions0"] = new List<string>();
                
                    ViewData["CourseList1"] = new List<string>();
                    ViewData["Grades1"] = new List<double>();
                    ViewData["GradeDescriptions1"] = new List<string>(); 
                
                    ViewData["CourseList2"] = new List<string>();
                    ViewData["Grades2"] = new List<double>();
                    ViewData["GradeDescriptions2"] = new List<string>();
                
                    ViewData["CourseList3"] = new List<string>();
                    ViewData["Grades3"] = new List<double>();
                    ViewData["GradeDescriptions3"] = new List<string>();
                
                    ViewData["CourseList4"] = new List<string>();
                    ViewData["Grades4"] = new List<double>();
                    ViewData["GradeDescriptions4"] = new List<string>();
                
                    ViewData["CourseList5"] = new List<string>();
                    ViewData["Grades5"] = new List<double>();
                    ViewData["GradeDescriptions5"] = new List<string>();
                
                    ViewData["CourseList6"] = new List<string>();
                    ViewData["Grades6"] = new List<double>();
                    ViewData["GradeDescriptions6"] = new List<string>();
                
                    ViewData["CourseList7"] = new List<string>();
                    ViewData["Grades7"] = new List<double>();
                    ViewData["GradeDescriptions7"] = new List<string>();



                    foreach (var record in matchingRecords)
                    {
                        if (record.MidMarks != null)
                        {
                            studentResult = record;
                            string courselist = studentResult.PreviousCourese;

                            List<string> cList = getCourseList(courselist);
                            List<int> atMarks = get_grade(studentResult.AttendenceMarks);
                            List<int> asMarks = get_grade(studentResult.AssignmentMarks);
                            List<int> cMarks = get_grade(studentResult.ClassTestMarks);
                            List<int> mMarks = get_grade(studentResult.MidMarks);
                            List<int> fMarks = get_grade(studentResult.FinalMarks);


                            List<double> grades = CalculateGrades(cList, atMarks, asMarks, cMarks, mMarks, fMarks);
                            List<string> gradeDescriptions = ConvertToGradeDescriptions(grades);

                            int rs = (int)record.Semester;
                            if (rs == 0)
                            {
                                ViewData["CourseList0"] = cList;
                                ViewData["Grades0"] = grades;
                                ViewData["GradeDescriptions0"] = gradeDescriptions;
                            }
                            else if (rs == 1)
                            {
                                ViewData["CourseList1"] = cList;
                                ViewData["Grades1"] = grades;
                                ViewData["GradeDescriptions1"] = gradeDescriptions;
                            }
                            else if (rs == 2)
                            {
                                ViewData["CourseList2"] = cList;
                                ViewData["Grades2"] = grades;
                                ViewData["GradeDescriptions2"] = gradeDescriptions;
                            }
                            else if (rs == 3)
                            {
                                ViewData["CourseList3"] = cList;
                                ViewData["Grades3"] = grades;
                                ViewData["GradeDescriptions3"] = gradeDescriptions;
                            }
                            else if (rs == 4)
                            {
                                ViewData["CourseList4"] = cList;
                                ViewData["Grades4"] = grades;
                                ViewData["GradeDescriptions4"] = gradeDescriptions;
                            }
                            else if (rs == 5)
                            {
                                ViewData["CourseList5"] = cList;
                                ViewData["Grades5"] = grades;
                                ViewData["GradeDescriptions5"] = gradeDescriptions;
                            }
                            else if (rs == 6)
                            {
                                ViewData["CourseList6"] = cList;
                                ViewData["Grades6"] = grades;
                                ViewData["GradeDescriptions6"] = gradeDescriptions;
                            }
                            else if (rs == 7)
                            {
                                ViewData["CourseList7"] = cList;
                                ViewData["Grades7"] = grades;
                                ViewData["GradeDescriptions7"] = gradeDescriptions;
                            }
                           
                        }
                    }
                    return View("Report");

                }
                else
                    return RedirectToAction("Index", "ResultReport");

            }

            // Additional logic if needed
            return RedirectToAction("Index", "ResultReport");
        }




        static List<string> ConvertToGradeDescriptions(List<double> grades)
        {
            List<string> gradeDescriptions = new List<string>();

            foreach (double grade in grades)
            {
                if (grade >= 4.00)
                {
                    gradeDescriptions.Add("Excellent");
                }
                else if (grade >= 3.75)
                {
                    gradeDescriptions.Add("Very Good");
                }
                else if (grade >= 3.50)
                {
                    gradeDescriptions.Add("Good");
                }
                else if (grade >= 3.00)
                {
                    gradeDescriptions.Add("Satisfactory");
                }
                else if(grade>=2.5)
                {
                    gradeDescriptions.Add("Bad");
                }
                else
                {
                    gradeDescriptions.Add("Fail");
                }
            }

            return gradeDescriptions;
        }

        static List<double> CalculateGrades(List<string> cList, List<int> atMarks, List<int> asMarks, List<int> cMarks, List<int> mMarks, List<int> fMarks)
        {
            List<double> grades = new List<double>();

            for (int i = 0; i < atMarks.Count; i++)
            {
                // Calculate the sum of marks
                int totalMarks = atMarks[i] + asMarks[i] + cMarks[i] + mMarks[i] + fMarks[i];

                // Determine the grade based on the sum
                double grade;
                if (totalMarks > 79)
                {
                    grade = 4.0;
                }
                else if (totalMarks > 69)
                {
                    grade = 3.75;
                }
                else if (totalMarks > 59)
                {
                    grade = 3.5;
                }
                else if (totalMarks > 49)
                {
                    grade = 3.0;
                }
                else if (totalMarks > 39)
                {
                    grade = 2.5;
                }
                else if (totalMarks > 29)
                {
                    grade = 2.0;
                }
                else
                {
                    grade = 1.0;
                }

                grades.Add(grade);
            }

            return grades;
        }


    }
}
