using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Professions;
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
        public Step2Dto Test { get; set; }
        public ICollection<UserTestAnswersDto> UserTestAnswers { get; set; }
        //public bool IsCorrect { get; set; }
    }

    [AutoMap(typeof(UserTestAnswers))]
    public class UserTestAnswersDto : EntityDto<long>
    {
       // public long UserTestId { get; set; }
        //public UserTestsDto UserTest { get; set; }
        public AnswerDto Answer { get; set; }
        public long AnswerId { get; set; }
    }

    [AutoMap(typeof(Step))]
    public class Step2Dto : EntityDto<long>
    {
        public StepType Type { get; set; }
        public bool IsActive { get; set; }
        public ICollection<StepContentDto> Content { get; set; }
        public int Duration { get; set; }
       // public Block Block { get; set; }
        public int Index { get; set; }
       // public ICollection<AnswerDto> Answers { get; set; }
    }

}
