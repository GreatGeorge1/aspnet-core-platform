using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions
{
    public interface IMedia
    {
        [MaxLength(300)]
        string Title { get; set; }
        string Description { get; set; }
        string Base64Image { get; set; }
    }
}
