using System.Collections.Generic;
using Abp.AutoMapper;
using Platform.Professions.Dtos.Answer;

namespace Platform.Professions.Dtos.Step
{
    [AutoMap(typeof(StepTest))]
    public class CreateTestStepDto:CreateInfoStepDto
    {
        public ICollection<CreateAnswerDto> Answers { get; set; }
    }
}
