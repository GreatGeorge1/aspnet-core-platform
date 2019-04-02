using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Step
{
    public class RemoveAnswerDto
    {
        public long StepTestId { get; set; }
        public long AnswerId { get; set; }
    }
}
