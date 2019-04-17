using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Platform.Professions.Dtos.Block;

namespace Platform.Professions.Dtos
{
    public class GetProfessionsDto
    {
        public long Id { get; set; }
        public int MinScore { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
        public string Language { get; set; }
        public ICollection<GetBlockDto> Blocks{get;set;}
    }
}
