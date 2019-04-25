using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public class StepInfoAppService : AsyncCrudAppService<StepInfo, StepInfoDto, long, PagedResultDto<StepInfo>, StepInfoCreateDto, StepInfoUpdateDto>, IStepInfoAppService
    {
        private readonly IRepository<StepInfo, long> stepInfoRepository;
        private readonly IRepository<StepTranslations, long> translationRepository; 

        public StepInfoAppService(IRepository<StepInfo, long> stepInfoRepository,
            IRepository<StepTranslations, long> translationRepository):base(stepInfoRepository)
        {
            this.stepInfoRepository = stepInfoRepository;
            this.translationRepository = translationRepository;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<StepInfoDto> Create(StepInfoCreateDto input)
        {
            throw new NotImplementedException();
        }

        public async Task CreateTranslation(StepTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new StepTranslations());
            translation.Id = 0;
            var step = await stepInfoRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            step.Translations.Add(translation);
        }

        public async Task DeleteTranslation(StepTranslationDeleteDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await translationRepository.InsertOrUpdateAsync(ts);
        }

        public async Task UpdateTranslation(StepTranslationDto input)
        {
            var ts = ObjectMapper.Map<StepTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }
    }
}
