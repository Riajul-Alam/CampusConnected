using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace CampusConnected.Models
{
    public class Teacher
    {
        [Key]
        public int Id;

        public int TeacherId { get; set;}
        public string Email { get; set;}
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set;} 
        public string TeacherName { get; set;}  
        public int DepartmentId { get; set;}
        public string TeacherDepartment { get; set; }
        [NotMapped]
        public List<Department>? DepartmentList { get; set; }

    }
}
