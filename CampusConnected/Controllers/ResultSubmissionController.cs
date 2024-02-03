using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampusConnected.Models;
using CampusConnected.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CampusConnected.Controllers
{
    public class ResultSubmissionController : Controller
    {

        private readonly StudentDBContext studentDB;
        public ResultSubmissionController(StudentDBContext studentDB)
        {
            this.studentDB = studentDB;
        }




        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else if(HttpContext.Session.GetString("TeacherSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("TeacherSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            var departments = studentDB.Departments.ToList();
            var students = studentDB.Students.ToList();
            var obj = new ResultSubmission
            {
                StudentList=students,
                DepartmentList = departments,   
            };

            ViewData["ViewModelData"] = obj;

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


        public ActionResult SubmitResult()
        {

            string stdJson = TempData["ResultSubmissionData"] as string;

            if (stdJson != null)
            {
                // Deserialize the JSON string back to ResultSubmission
                ResultSubmission std = JsonConvert.DeserializeObject<ResultSubmission>(stdJson);

                var s_id = std.StudentId;
                var d_id = std.DepartmentId;
                var semester = std.Semester;
                var stdRec = studentDB.Students.FirstOrDefault(s => s.Id == s_id);
                ViewData["StudentId"] = stdRec.StudentId;
                ViewData["StudentName"] = stdRec.StudentName;
                ViewData["DepartmentId"] = stdRec.DepartmentName;
                ViewData["Semester"] = semester;

                //var stdCourseRec =  studentDB.studentCourses.Where(sc => sc.StudentId == s_id && sc.DepartmentId == d_id).ToList();
                var stdCourseRec = studentDB.studentCourses.FirstOrDefault(sc => sc.StudentId == s_id && sc.DepartmentId == d_id);
                
                ViewData["recData"] = stdCourseRec;

                TempData["s_Id"] = s_id;
                TempData["d_Id"] = d_id;
                


                if (stdCourseRec != null)
                {
                    string sm = stdCourseRec.Semester.ToString();
                    if(sm != semester.ToString())
                    {
                        TempData["error"] = "Error";
                        return View();
                    }
                    var stdCourseList = stdCourseRec.CourseidList;

                    List<string> CourseList = getCourseList(stdCourseList);
                    TempData["CourseList"] = CourseList;
                    ViewData["CourseList"] = CourseList;
                    return View();
                }
                else
                {
                    TempData["error"]="Error";
                    return View();
                }


            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitResult(IFormCollection form)
        {
            //var departmentId = ViewData["DepartmentId"] as string;
            //var semester = ViewData["Semester"] as string;
            //var studentId = ViewData["StudentId"] as string;

            var ss_id = form["StudentId"];
            var departmentId = form["DepartmentId"];
            var semester = form["Semester"];
            string studentId = ss_id.ToString();
            string sems = semester.ToString();
            //Console.WriteLine("-----------------------------------");
            //Console.WriteLine(studentId);
            //Console.WriteLine(departmentId);

            if(studentId == null)
            {
                return RedirectToAction("Index", "ResultSubmission");
            }


            var stdCourseRec = studentDB.studentCourses.FirstOrDefault(s=>s.StudentDeptId==studentId);

            if (stdCourseRec == null)
            {
                return RedirectToAction("Index", "ResultSubmission");
            }

            var stdCourseList = stdCourseRec.CourseidList;

            List<string> CourseList = getCourseList(stdCourseList);

            //foreach (var c in CourseList)
            //    Console.WriteLine(c);


            int numberOfCourses = CourseList.Count;
            
            // Arrays to store results
            int[] midResultsArray = new int[numberOfCourses];
            int[] finalResultsArray = new int[numberOfCourses];
            int[] classTestResultsArray = new int[numberOfCourses];
            int[] assignmentResultsArray = new int[numberOfCourses];
            int[] attendanceResultsArray = new int[numberOfCourses];

            for (int i = 0; i < numberOfCourses; i++)
            {
                // Parse and store values in arrays
                midResultsArray[i] = Convert.ToInt32(form[$"MidResults[{i}]"]);
                finalResultsArray[i] = Convert.ToInt32(form[$"FinalResults[{i}]"]);
                classTestResultsArray[i] = Convert.ToInt32(form[$"ClassTestResults[{i}]"]);
                assignmentResultsArray[i] = Convert.ToInt32(form[$"AssignmentResults[{i}]"]);
                attendanceResultsArray[i] = Convert.ToInt32(form[$"AttendanceResults[{i}]"]);
                if (midResultsArray[i] > 30 || midResultsArray[i] < 0) midResultsArray[i] = 0;
                if (finalResultsArray[i] > 50 || finalResultsArray[i] < 0) finalResultsArray[i] = 0;
                if (classTestResultsArray[i] > 5 || classTestResultsArray[i] < 0) classTestResultsArray[i] = 0;
                if (assignmentResultsArray[i] > 5 || assignmentResultsArray[i] < 0) assignmentResultsArray[i] = 0;
                if (attendanceResultsArray[i] > 10 || attendanceResultsArray[i] < 0) attendanceResultsArray[i] = 0;
            }

            //2351
            //column value is like : 
            //course_id:marks,Course_id:marks

            string midResult = "";
            string finalResult = "";
            string classTestResult = "";
            string assignmentResult = "";
            string attendanceResult = "";
            
            string[] courseIdStrings = stdCourseList.Split(',');
  
            for(int i = 0; i < numberOfCourses; i++)
            {
                midResult += courseIdStrings[i] + ":" + midResultsArray[i].ToString();
                finalResult += courseIdStrings[i] + ":" + finalResultsArray[i].ToString();
                classTestResult += courseIdStrings[i] + ":" + classTestResultsArray[i].ToString();
                assignmentResult += courseIdStrings[i] + ":" + assignmentResultsArray[i].ToString();
                attendanceResult += courseIdStrings[i] + ":" + attendanceResultsArray[i].ToString();
                if (i + 1 < numberOfCourses)
                {
                    midResult += ",";
                    finalResult += ",";
                    classTestResult += ",";
                    assignmentResult += ",";
                    attendanceResult += ",";

                }
            }

            double calculatedGrade = calculate_grade(midResultsArray,finalResultsArray,classTestResultsArray,assignmentResultsArray,attendanceResultsArray);

            //var existingRecord = studentDB.studentResult.FirstOrDefault(sr => sr.StudentDeptId == studentId);
            var matchingRecords = studentDB.studentResult.Where(sr => sr.StudentDeptId == studentId).ToList();
            ResultSubmission? existingRecord = null;
            foreach (var record in matchingRecords)
            {
                string sms = record.Semester.ToString();
                if(sms == sems)
                {
                    existingRecord = record;
                    break;
                }
            }
            //int sms = (int)std.Semester;
            //var existingRecord = await studentDB.studentResult.FirstOrDefaultAsync(sr => sr.StudentId == std.StudentId && (int)sr.Semester == sms);

            var sRec = studentDB.Students.FirstOrDefault(x => x.StudentId == studentId);
            if (sRec.Grade == null)
            {
                sRec.Grade = calculatedGrade;
            }
            else
            {

                double preG = (double)sRec.Grade;
                double newGr = (double)(preG + calculatedGrade) / 2.00;
                sRec.Grade = newGr;
            }
            if (existingRecord != null)
            {
               // string curSem = existingRecord.Semester.ToString();

                existingRecord.MidMarks = midResult;
                existingRecord.FinalMarks = finalResult;
                existingRecord.ClassTestMarks =classTestResult ;
                existingRecord.AssignmentMarks = assignmentResult;
                existingRecord.AttendenceMarks = attendanceResult;
                existingRecord.Grade = calculatedGrade;
                existingRecord.PreviousCourese = stdCourseList;
            }
            else
            {
                var studentRec = studentDB.Students.FirstOrDefault(x=>x.StudentId==studentId);
                var newRecord = new ResultSubmission
                {

                    StudentId = studentRec.Id,
                    StudentDeptId = studentId,
                    DepartmentId = studentRec.DepartmentId,
                    //Semester=studentRec.Semester,
                    MidMarks = midResult,
                    FinalMarks=finalResult,
                    ClassTestMarks=classTestResult,
                    AssignmentMarks=assignmentResult,
                    AttendenceMarks=attendanceResult,
                    Grade=calculatedGrade,
                    PreviousCourese=stdCourseList,
                };
                if (semester == "First") newRecord.Semester = ResultSubmission.SemesterList.First;
                else if (semester == "Second") newRecord.Semester = ResultSubmission.SemesterList.Second;
                else if (semester == "Third") newRecord.Semester = ResultSubmission.SemesterList.Third;
                else if (semester == "Fourth") newRecord.Semester = ResultSubmission.SemesterList.Fourth;
                else if (semester == "Fiveth") newRecord.Semester = ResultSubmission.SemesterList.Fiveth;
                else if (semester == "Sixth") newRecord.Semester = ResultSubmission.SemesterList.Sixth;
                else if (semester == "Seventh") newRecord.Semester = ResultSubmission.SemesterList.Seventh;
                else if (semester == "Eighth") newRecord.Semester = ResultSubmission.SemesterList.Eighth;
                else newRecord.Semester = ResultSubmission.SemesterList.First;

                studentDB.studentResult.Add(newRecord);
            }

            //if (calculatedGrade >= 2.00)
            //{

            //}





            List<int> resultHistory = subjectResultCheck(midResultsArray, finalResultsArray, classTestResultsArray, assignmentResultsArray, attendanceResultsArray); 

            //update course status

            //var stdCourseList = stdCourseRec.CourseidList;
            //List<string> CourseList = getCourseList(stdCourseList);
            string newCourse = "";
            string[] courseIds = stdCourseList.Split(',');
            int all = 1;
            string passes = "";
            for (int i = 0; i < resultHistory.Count; i++)
            {
                if (resultHistory[i] == 0)
                {
                    if (newCourse.Length != 0)
                    {
                        newCourse += ",";
                    }
                    newCourse += courseIds[i];

                }
                else { 
                    all = 0;
                    passes += courseIds[i];
                }
            }
            //var sRec = studentDB.Students.FirstOrDefault(x => x.StudentId == studentId);
            if(sRec.CourseComplete == null)
            {
                sRec.CourseComplete = passes;
            }
            else
            {
                string newC = sRec.CourseComplete;
                if (passes.Length != 0)
                {
                    newC += ",";
                }
                newC += passes;
                sRec.CourseComplete = newC;
            }

            //var deletedData = await studentDB.Students.FindAsync(id);
            //if (deletedData != null)
            //{
            //    studentDB.Students.Remove(deletedData);
            //}
            //Console.WriteLine("-----------from course chek-------------------");
            //Console.WriteLine(newCourse);
            //Console.WriteLine(all);

            if (all == 0) {
                var deleteDaata = studentDB.studentCourses.FirstOrDefault(s => s.StudentDeptId == studentId);
                if (newCourse.Length == 0)
                {

                    if (deleteDaata != null)
                    {
                        studentDB.studentCourses.Remove(deleteDaata);

                    }
                }
                else
                {
                    if (deleteDaata != null)
                    {
                        deleteDaata.CourseidList = newCourse;
                        var sem = deleteDaata.Semester.ToString();
                        if (sem == "First") deleteDaata.Semester = StudentCourse.SemesterList.Second;
                        else if (sem == "Second") deleteDaata.Semester = StudentCourse.SemesterList.Third;
                        else if (sem == "Third") deleteDaata.Semester = StudentCourse.SemesterList.Fourth;
                        else if (sem == "Fourth") deleteDaata.Semester = StudentCourse.SemesterList.Fiveth;
                        else if (sem == "Fiveth") deleteDaata.Semester = StudentCourse.SemesterList.Sixth;
                        else if (sem == "Sixth") deleteDaata.Semester = StudentCourse.SemesterList.Seventh;
                        else deleteDaata.Semester = StudentCourse.SemesterList.Eighth;
                    }
                }
                var stData = studentDB.Students.FirstOrDefault(x => x.StudentId == studentId);
                if (stData != null)
                {
                    var sem = stData.Semester.ToString();
                    var curS = "";
                    if (sem == "First") stData.Semester = Student.SemesterList.Second;
                    else if (sem == "Second") stData.Semester = Student.SemesterList.Third;
                    else if (sem == "Third") stData.Semester = Student.SemesterList.Fourth;
                    else if (sem == "Fourth") stData.Semester = Student.SemesterList.Fiveth;
                    else if (sem == "Fiveth") stData.Semester = Student.SemesterList.Sixth;
                    else if (sem == "Sixth") stData.Semester = Student.SemesterList.Seventh;
                    else stData.Semester = Student.SemesterList.Eighth;
                }
               
               
            }



            studentDB.SaveChanges();





            //foreach (var key in form.Keys)
            //{
            //    Console.WriteLine($"{key}: {form[key]}");
            //}


            //for (int i = 0; i < numberOfCourses; i++)
            //{
            //    Console.WriteLine(midResultsArray[i]);
            //}

            return RedirectToAction("Index", "ResultSubmission");
            //return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultSubmission std)
        {
            if (std.Id != null)
            {
                var cCheck = await studentDB.studentCourses.FirstOrDefaultAsync(x => x.StudentId == std.StudentId);
                if (cCheck == null)
                {
                    return RedirectToAction("Index", "ResultSubmission");
                }
                else
                {
                    int sms = (int)std.Semester;
                    var existingRecord = await studentDB.studentResult.FirstOrDefaultAsync(sr => sr.StudentId == std.StudentId && (int)sr.Semester == sms);
                    
                    if (existingRecord == null)
                    {
                       await studentDB.AddAsync(std);
                        await studentDB.SaveChangesAsync();

                    }
                }


              

                string stdJson = JsonConvert.SerializeObject(std);
                TempData["ResultSubmissionData"] = stdJson;

                return RedirectToAction("SubmitResult", "ResultSubmission");
            }

            // Additional logic if needed
            return View(std);
        }



        public double calculate_grade(int[] midResultsArray, int[] finalResultsArray, int[] classTestResultsArray, int[] assignmentResultsArray, int[] attendanceResultsArray)
        {
            int totalCourses = midResultsArray.Length;

            double totalGradePoints = 0;

            for (int i = 0; i < totalCourses; i++)
            {
                int totalMarks = midResultsArray[i] + finalResultsArray[i] + classTestResultsArray[i] + assignmentResultsArray[i] + attendanceResultsArray[i];

                // Calculate average marks for the course (assuming each course has a maximum possible mark of 100)
                double averageMarks = (double)totalMarks / (100);

                // Calculate grade points based on average marks
                double gradePoints;

                if (averageMarks > 0.79)
                {
                    gradePoints = 4.0;
                }
                else if (averageMarks > 0.69)
                {
                    gradePoints = 3.75;
                }
                else if (averageMarks > 0.59)
                {
                    gradePoints = 3.5;
                }
                else if (averageMarks > 0.49)
                {
                    gradePoints = 3.0;
                }
                else if (averageMarks > 0.39)
                {
                    gradePoints = 2.5;
                }
                else
                {
                    gradePoints = 2.0;

                }

                // Accumulate grade points for all courses
                totalGradePoints += gradePoints;
            }

            // Calculate the overall average grade
            double averageGrade = totalGradePoints / totalCourses;

            return averageGrade;
        }



        public List<int> subjectResultCheck(int[] midResultsArray, int[] finalResultsArray, int[] classTestResultsArray, int[] assignmentResultsArray, int[] attendanceResultsArray)
        {
            int totalCourses = midResultsArray.Length;

            List<int> result = new List<int>();
            int fail = 0;
            for (int i = 0; i < totalCourses; i++)
            {
                fail = 0;
                int totalMarks = midResultsArray[i] + finalResultsArray[i] + classTestResultsArray[i] + assignmentResultsArray[i] + attendanceResultsArray[i];

                // Calculate average marks for the course (assuming each course has a maximum possible mark of 100)
                double averageMarks = (double)totalMarks / (100);

                // Calculate grade points based on average marks
                double gradePoints;

                if (averageMarks > 0.79)
                {
                    gradePoints = 4.0;
                }
                else if (averageMarks > 0.69)
                {
                    gradePoints = 3.75;
                }
                else if (averageMarks > 0.59)
                {
                    gradePoints = 3.5;
                }
                else if (averageMarks > 0.49)
                {
                    gradePoints = 3.0;
                }
                else if (averageMarks > 0.39)
                {
                    gradePoints = 2.5;
                }
                else
                {
                    gradePoints = 2.0;
                    fail = 1;

                }

                // Accumulate grade points for all courses
                if(fail==1)
                    result.Add(0);
                else 
                    result.Add(1);
            }

            // Calculate the overall average grade
           return result;
        }


    }
}
