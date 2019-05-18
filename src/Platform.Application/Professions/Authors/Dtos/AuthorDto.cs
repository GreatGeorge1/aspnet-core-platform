using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Author))]
    public class AuthorDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public string Base64Image { get; set; }
        public string Description { get; set; }
        public ICollection<ProfessionIdDto> Professions { get; set; }
    }

    [AutoMap(typeof(Author))]
    public class AuthorCreateDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public string Base64Image { get; set; }
        public string Description { get; set; }
    }

    [AutoMap(typeof(Profession))]
    public class ProfessionIdDto
    {
        public long Id { get; set; }
    }
}
