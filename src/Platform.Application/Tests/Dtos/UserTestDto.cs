using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Professions.Dtos;
using Platform.Professions.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Tests.Dtos
{
    [AutoMap(typeof(UserTests))]
    public class UserTestResponseDto
    {
        //public UserProfessions UserProfession { get; set; }
        public StepDto Test { get; set; }
        public ICollection<UserTestAnswersDto> UserTestAnswers { get; set; }
        //public bool IsCorrect { get; set; }
    }

    public class UserTestAnswersDto : EntityDto<long>
    {
       // public long UserTestId { get; set; }
        //public UserTestsDto UserTest { get; set; }
        public AnswerDto Answer { get; set; }
        public long AnswerId { get; set; }
    }
}
