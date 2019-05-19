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
}