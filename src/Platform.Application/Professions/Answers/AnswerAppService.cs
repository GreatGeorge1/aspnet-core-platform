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
    public class AnswerAppService : AsyncCrudAppService<Answer, AnswerDto, long, PagedResultDto<Answer>, AnswerCreateDto, AnswerUpdateDto>, IAnswerAppService
    {
        private readonly IRepository<Answer, long> answerRepository;
        private readonly IRepository<AnswerTranslation, long> translationRepository;
        public AnswerAppService(IRepository<Answer, long> answerRepository,
            IRepository<AnswerTranslation, long> translationRepository):base(answerRepository)
        {
            this.answerRepository = answerRepository;
            this.translationRepository = translationRepository;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<AnswerDto> Create(AnswerCreateDto input)
        {
            throw new NotSupportedException("Создание вне контекста шага запрещено");
        }

        public async Task CreateTranslation(AnswerTranslationDto input, long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var translation = ObjectMapper.Map(input, new AnswerTranslation());
            translation.Id = 0;
            var answer = await answerRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            answer.Translations.Add(translation);
        }

        public async Task DeleteTranslation(AnswerTranslationDeleteDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            await translationRepository.DeleteAsync(ts);
        }

        public async Task UpdateTranslation(AnswerTranslationDto input)
        {
            var ts = ObjectMapper.Map<AnswerTranslation>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }

        protected override IQueryable<Answer> CreateFilteredQuery(PagedResultDto<Answer> input)
        {
            return answerRepository.GetAllIncluding(p => p.Translations).AsQueryable();
        }

        protected override async Task<Answer> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var entity = await answerRepository.GetAllIncluding(p => p.Translations).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Answer), id);
            }
            return entity;
        }
    }
}
