using System.ComponentModel.DataAnnotations;

namespace Connected_Campus.Models
{
    public class Administration
    {
        [Key]
        [Required]
        public int Id { set; get; }
        [Required]
        public string? Password { set; get; }
        [Required]
        public string? Name { set; get; }
        [Required]
        public string? Email { set; get; }
        [Required]
        public string? Phone { set; get; }

    }
}
