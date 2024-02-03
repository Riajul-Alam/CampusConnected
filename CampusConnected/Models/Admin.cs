using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CampusConnected.Models
{
	public class Admin
	{
		[Key]
		public int Id { set; get; }
		[Required]
		[DataType(DataType.Password)]
		public string? Password { set; get; }
		[Required]
		public string? Name { set; get; }
		[Required]
		public string? Email { set; get; }
	
	}
}
