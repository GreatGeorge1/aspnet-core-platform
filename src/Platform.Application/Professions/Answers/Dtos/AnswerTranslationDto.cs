using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(AnswerTranslation))]
    public class AnswerTranslationDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public string Language { get; set; }
        [MaxLength(300)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
    }
}
