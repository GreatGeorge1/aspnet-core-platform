using System.ComponentModel.DataAnnotations;

namespace Platform.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}