using System.ComponentModel.DataAnnotations;

namespace APIIngame.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }

    }
}
