using System.ComponentModel.DataAnnotations;

namespace Platform.Professions
{
    public interface IMedia
    {
        [MaxLength(300)]
        string Title { get; set; }
        string Description { get; set; }
        string Base64Image { get; set; }

        ///video
        [Url]
        string VideoUrl { get; set; }
    }
}
