using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusConnected.Models
{
    public class ResultSubmission
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        [Column("StudentDId", TypeName = "varchar(100)")]
        public string? StudentDeptId { get; set; }
        public int DepartmentId { get; set; }
        public double? Grade { get; set; }
        public SemesterList Semester { get; set; }
        public string? PreviousCourese { get; set; }
        
        //column value is like : 
        //course_id:marks,Course_id:marks
        public string? MidMarks { get; set; }   
        public string? FinalMarks { get; set; }
        public string? ClassTestMarks { get; set; }
        public string? AssignmentMarks { get; set; }
        public string? AttendenceMarks { get; set; }


        [NotMapped]
        public List<Department> DepartmentList { get; set; }

        [NotMapped]
        public List<Student> StudentList { get; set; }
       
        public enum SemesterList
        {
            First,
            Second,
            Third,
            Fourth,
            Fiveth,
            Sixth,
            Seventh,
            Eighth
        }



    }
}
