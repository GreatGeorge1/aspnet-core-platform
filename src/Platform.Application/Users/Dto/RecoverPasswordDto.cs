using System.ComponentModel.DataAnnotations;

namespace Platform.Users.Dto
{
    public class RecoverPasswordDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }
    }
    
    public class ConfirmEmailDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string ConfirmCode { get; set; }
    }
}