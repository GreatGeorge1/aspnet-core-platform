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
        public StepTestDto StepTest { get; set; }
        public ICollection<AnswerDto> Answers { get; set; }
        //public bool IsCorrect { get; set; }
    }
}
