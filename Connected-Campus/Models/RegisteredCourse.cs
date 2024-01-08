using System.ComponentModel.DataAnnotations;

namespace Connected_Campus.Models
{
    public class RegisteredCourse
    {
        public int? StudentId { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseTitle { get; set; }
        public double? CreditHours { set; get; }
    }
}
