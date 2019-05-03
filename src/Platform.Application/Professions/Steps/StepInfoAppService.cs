using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
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
            throw new NotSupportedException("Создание вне контекста блока запрещено");
        }

        public async Task CreateTranslation(StepTranslationDto input, long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var translation = ObjectMapper.Map(input, new StepTranslations());
            translation.Id = 0;
            var step = await stepInfoRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            step.Translations.Add(translation);
        }

        public async Task DeleteTranslation(StepTranslationDeleteDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            await translationRepository.DeleteAsync(ts);
        }

        public async Task UpdateTranslation(StepTranslationDto input)
        {
            var ts = ObjectMapper.Map<StepTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }

        protected override IQueryable<StepInfo> CreateFilteredQuery(PagedResultDto<StepInfo> input)
        {
            return stepInfoRepository.GetAllIncluding(p => p.Translations).AsQueryable();
        }

        protected override async Task<StepInfo> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var entity = await stepInfoRepository.GetAllIncluding(p => p.Translations).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(StepInfo), id);
            }
            return entity;
        }
    }
}
