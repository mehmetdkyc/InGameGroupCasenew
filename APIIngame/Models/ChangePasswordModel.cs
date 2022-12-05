namespace APIIngame.Models
{
    public class ChangePasswordModel
    {
        public string Email { get; set; }
        public string OldPasword { get; set; }
        public string NewPassword { get; set; }
    }
}
