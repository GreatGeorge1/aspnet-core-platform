using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Professions.Dtos;
using Platform.Tests.Dtos;

namespace Platform.Professions.User
{
    [AutoMap(typeof(UserProfessions))]
    public class UserProfessionsDto:EntityDto<long>
    {
        public UserProfessionsProfession Profession { get; set; }
        public int Score { get; set; }
        public ICollection<UserTestResponseDto> UserTests { get; set; }
    }

    [AutoMap(typeof(Profession))]
    public class UserProfessionsProfession
    {
        public long Id { get; set; }
        public AuthorDto Author { get; set; }
        public UserProfessionsProfessionContent Content { get; set; }
    }
    
    [AutoMap(typeof(ProfessionContent))]
    public class UserProfessionsProfessionContent
    {
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        public string VideoUrl { get; set; }
        public long Id { get; set; }
    }
    
}