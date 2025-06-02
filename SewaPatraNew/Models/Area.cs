using System.ComponentModel.DataAnnotations;

namespace SewaPatra.Models
{
    public class Area
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Area Name is required")]
        public string Area_name { get; set; }
        [Required(ErrorMessage = "Area Type is required")]
        public string Area_type { get; set; }

	    public string? Under { get; set; }

    }
}
