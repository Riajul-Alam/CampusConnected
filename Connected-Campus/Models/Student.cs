using System.ComponentModel.DataAnnotations;

namespace Connected_Campus.Models
{
    public class Student
    {
        [Key]
        [Required]
        public int Id { set; get; }
        [Required]
        public string? Password { set; get; }
        public string? Name { set; get; }
        [Required]
        public string? Email { set; get; }
        [Required]
        public string? Phone { set; get; }
        [Required]
        public string? EmergencyContact { set; get; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string? Gender { set; get; }
        [Required]
        public string? Address { set; get; }
        [Required]
        public string? DepartmentName { set; get; }
    }
}
