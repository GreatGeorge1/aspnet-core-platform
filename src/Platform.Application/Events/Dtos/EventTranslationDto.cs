using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Events.Dtos
{
    [AutoMap(typeof(EventTranslations))]
    public class EventTranslationDto:EntityDto<long>
    {
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }

        public string Language { get; set; }
        [Url]
        public string VideoUrl { get; set; }
    }
}
