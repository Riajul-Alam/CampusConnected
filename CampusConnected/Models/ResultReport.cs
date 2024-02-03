using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusConnected.Models
{
    public class ResultReport
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public int StudentId { get; set; }
        [NotMapped]
        public int? DepartmentId { get; set; }
        [NotMapped]
        public List<Department> DepartmentList { get; set; }
        [NotMapped]
        public List<Student> StudentList { get; set; }



    }
}
