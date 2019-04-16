using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(ProfessionTranslations))]
    public class UpdateProfessionTranslationDto
    {
        /// <summary>
        /// ProfessionTranslations Id, не Profession
        /// </summary>
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
        public string Language { get; set; }
    }
  
}
