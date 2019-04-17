using System.Threading.Tasks;
using Platform.Professions.Dtos.Step;

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
