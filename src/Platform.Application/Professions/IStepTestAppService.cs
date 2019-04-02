using Platform.Professions.Dtos.Answer;
using Platform.Professions.Dtos.Step;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions
{
    public interface IStepTestAppService
    {
        Task<StepReplyOkDto> UpdateTestStep(UpdateTestStepDto input);
        Task<StepReplyOkDto> DeleteTestStep(long id);
        // Task<GetInfoStepDto> GetTestStep(long id);
        Task<GetTestStepAllDto> GetTestStepAll(long id);
        Task AddTranslation(CreateStepTranslationDto input, long id);
        Task UpdateTranslation(UpdateStepTranslationDto input);
        Task DeleteTranslation(DeleteStepTranslationDto input);
        Task AddAnswer(CreateAnswerDto input, long id);
        Task RemoveAnswer(RemoveAnswerDto input);
    }
}
