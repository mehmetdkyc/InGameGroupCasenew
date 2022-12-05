using System.ComponentModel.DataAnnotations;

namespace APIIngame.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50,MinimumLength =5)]
        public string  Password { get; set; }
        public int RememberMe { get; set; }
    }
}
