using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class User
    {
        [StringLength(10, MinimumLength = 3)]
        [Required]
        [Key]
        public string Username { get; set; }

        [StringLength(10, MinimumLength = 3)]
        [Required]
        public string Password { get; set; }
    }
}
