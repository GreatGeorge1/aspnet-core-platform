using System.ComponentModel.DataAnnotations;

namespace Platform.Users.Dto
{
    public class SendResetCodeDto
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string ResetFormUrl { get; set; }
    }
    
    public class SendConfirmCodeDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string ConfirmFormUrl { get; set; }
    }
}