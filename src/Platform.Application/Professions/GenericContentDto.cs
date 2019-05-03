using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions
{
    public abstract class GenericContentDto<TContent, TKey> 
        where TContent : IEntity<TKey>, IPassivable, IMedia
    {
        public TKey Id { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
    }
}
