using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions.Dtos.Step
{
    public class GetInfoStepDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public int Index { get; set; }
        public DateTime Duration { get; set; }
        public int MinScore { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string? VideoUrl { get; set; }
        public string Language { get; set; }
    }
}
