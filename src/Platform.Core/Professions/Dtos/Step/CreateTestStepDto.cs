using Abp.AutoMapper;
using Platform.Professions.Dtos.Answer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Step
{
    [AutoMap(typeof(StepTest))]
    public class CreateTestStepDto:CreateInfoStepDto
    {
        public ICollection<CreateAnswerDto> Answers { get; set; }
    }
}
