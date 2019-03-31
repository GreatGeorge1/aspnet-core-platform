using Platform.Professions.Dtos.Step;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions
{
    public interface IStepInfoAppService
    {
        Task<StepReplyOkDto> UpdateInfoStep(UpdateInfoStepDto input);
        Task<StepReplyOkDto> DeleteInfoStep(long id);
       // Task<GetInfoStepDto> GetInfoStep(long id);
        Task<GetInfoStepAllDto> GetInfoStepAll(long id);
        Task AddTranslation(CreateStepTranslationDto input, long id);
        Task UpdateTranslation(UpdateStepTranslationDto input);
        Task DeleteTranslation(DeleteStepTranslationDto input);
    }
}
