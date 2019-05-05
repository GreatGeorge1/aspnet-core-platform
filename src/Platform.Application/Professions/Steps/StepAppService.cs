using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public class StepAppService : AsyncCrudAppService<Step, StepDto, long, PagedResultDto<Step>, StepCreateDto, StepUpdateDto>, IStepAppService
    {
        private readonly IRepository<Step, long> stepRepository;
        private readonly IRepository<StepContent, long> translationRepository;
        private readonly IRepository<Answer, long> answerRepository;

        public StepAppService(IRepository<Step, long> stepRepository, IRepository<StepContent, long> translationRepository, IRepository<Answer, long> answerRepository)
            :base(stepRepository)
        {
            this.stepRepository = stepRepository ?? throw new ArgumentNullException(nameof(stepRepository));
            this.translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            this.answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<StepDto> Create(StepCreateDto input)
        {
            throw new UserFriendlyException("Создание вне контекста блока запрещено");
        }

        public async Task CreateAnswer(AnswerCreateDto input, long id)
        {
            if (id == 0)
            {
                throw new UserFriendlyException("id в url не может быть 0 или null");
            }
            var step = await stepRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (step.Type == StepType.Info)
            {
                throw new UserFriendlyException("нельзя создать ответ для шага типа Info");
            }
            var answer = ObjectMapper.Map(input, new Answer());
            answer.Test = step;
            var newid = await answerRepository.InsertAndGetIdAsync(answer);
        }

        public async Task DeleteAnswer(AnswerDeleteDto input)
        {
            //TODO cleanup
            var step = await stepRepository.GetAllIncluding(p => p.Content)
              .FirstOrDefaultAsync(p => p.Id == input.StepTestId);
            var answer = await answerRepository.GetAllIncluding(p => p.Content)
              .FirstOrDefaultAsync(p => p.Id == input.AnswerId);
            answer.IsActive = false;
            answer.IsDeleted = true;
        }
     
        public async Task<StepContentDto> UpdateContent(StepContentDto input)
        {
            if (input.Id == 0)
            {
                throw new UserFriendlyException("id cannot be 0 or null");
            }
            var ts = ObjectMapper.Map<StepContent>(input);
            var old = await translationRepository.GetAllIncluding(p => p.Core).FirstOrDefaultAsync(p => p.Id == input.Id);

            old.Update(ts);
            await translationRepository.InsertOrUpdateAsync(old);
            var updts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            return ObjectMapper.Map<StepContentDto>(updts);
        }

        protected override IQueryable<Step> CreateFilteredQuery(PagedResultDto<Step> input)
        {
            return stepRepository.GetAllIncluding(p => p.Content, p=>p.Answers).AsQueryable();
        }

        protected override async Task<Step> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new UserFriendlyException("id в url не может быть 0 или null");
            }
            var entity = await stepRepository.GetAllIncluding(p => p.Content, p => p.Answers).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Step), id);
            }
            return entity;
        }
    }
}
