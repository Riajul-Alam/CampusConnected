using System.ComponentModel.DataAnnotations;

namespace Connected_Campus.Models
{
    public class Course
    {
        [Key]
        public string? CourseCode { get; set; }
        public string? CourseTitle { get; set; }
        public double? CreditHours { set; get; }
    }
}
