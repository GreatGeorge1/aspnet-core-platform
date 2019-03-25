using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    public class ProfessionListDto
    {
        public long Id { get; set; }
        public int MinScore { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
    }

}
