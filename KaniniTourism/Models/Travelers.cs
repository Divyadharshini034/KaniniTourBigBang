using System.ComponentModel.DataAnnotations;

namespace KaniniTourism.Models
{
    public class Travelers
    {
        [Key]
        public int TravelerId { get; set; }
        [Required]
        public string TravelerName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Invalid email format. The email must end with @gmail.com")]
        public string TravelerEmail { get; set; }

        public string? TravelerMobileNo { get; set; }

        public string? TravelerAddress { get; set; }

        public string? DOB { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string TravelerPass { get; set; }
    }
}
