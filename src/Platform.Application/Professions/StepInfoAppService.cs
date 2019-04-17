using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos.Step;

namespace Platform.Professions
{
    public class StepInfoAppService : ApplicationService,IStepInfoAppService
    {
        private readonly IRepository<StepInfo, long> stepInfoRepository;
        private readonly IRepository<StepTranslations, long> translationRepository; 

        public StepInfoAppService(IRepository<StepInfo, long> stepInfoRepository,
            IRepository<StepTranslations, long> translationRepository)
        {
            this.stepInfoRepository = stepInfoRepository;
            this.translationRepository = translationRepository;
        }

        public async Task AddTranslation(CreateStepTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new StepTranslations());
            var step = await stepInfoRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            step.Translations.Add(translation);
        }

        public async Task<StepReplyOkDto> DeleteInfoStep(long id)
        {
            var step = await stepInfoRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            step.IsDeleted = true;
            step.IsActive = false;
            var res = await stepInfoRepository.InsertOrUpdateAndGetIdAsync(step);
            return new StepReplyOkDto { id = res, message = "deleted" };
        }

        public async Task DeleteTranslation(DeleteStepTranslationDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await translationRepository.InsertOrUpdateAsync(ts);
        }

        public async Task<GetInfoStepAllDto> GetInfoStepAll(long id)
        {
            var step = await stepInfoRepository.GetAllIncluding(p => p.Translations)
             .FirstOrDefaultAsync(p => p.Id == id);
            var res = ObjectMapper.Map(step, new GetInfoStepAllDto());
            return res;
        }

        public async Task<StepReplyOkDto> UpdateInfoStep(UpdateInfoStepDto input)
        {
            var step = await stepInfoRepository.GetAllIncluding(p => p.Translations)
             .FirstOrDefaultAsync(p => p.Id == input.Id);
            step.Translations.Clear();
            var upd = ObjectMapper.Map(input, step);
            var res = await stepInfoRepository.InsertOrUpdateAndGetIdAsync(upd);
            return new StepReplyOkDto { id = res, message = "updated" };
        }

        public async Task UpdateTranslation(UpdateStepTranslationDto input)
        {
            var ts = ObjectMapper.Map<StepTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }
    }
}
