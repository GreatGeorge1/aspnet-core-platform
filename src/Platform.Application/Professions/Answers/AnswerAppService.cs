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
    public class AnswerAppService : AsyncCrudAppService<Answer, AnswerDto, long, PagedResultDto<Answer>, AnswerCreateDto, AnswerUpdateDto>, IAnswerAppService
    {
        private readonly IRepository<Answer, long> answerRepository;
        private readonly IRepository<AnswerContent, long> translationRepository;

        public AnswerAppService(IRepository<Answer, long> answerRepository, IRepository<AnswerContent, long> translationRepository)
            :base(answerRepository)
        {
            this.answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
            this.translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<AnswerDto> Create(AnswerCreateDto input)
        {
            throw new UserFriendlyException("Создание вне контекста шага запрещено");
        }

        public async Task<AnswerContentDto> UpdateContent(AnswerContentDto input)
        {
            if (input.Id == 0)
            {
                throw new UserFriendlyException("id cannot be 0 or null");
            }
            var ts = ObjectMapper.Map<AnswerContent>(input);
            var old = await translationRepository.GetAllIncluding(p => p.Core).FirstOrDefaultAsync(p => p.Id == input.Id);

            old.Update(ts);
            await translationRepository.InsertOrUpdateAsync(old);
            var updts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            return ObjectMapper.Map<AnswerContentDto>(updts);
        }

        protected override IQueryable<Answer> CreateFilteredQuery(PagedResultDto<Answer> input)
        {
            return answerRepository.GetAllIncluding(p => p.Content).AsQueryable();
        }

        protected override async Task<Answer> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new UserFriendlyException("id в url не может быть 0 или null");
            }
            var entity = await answerRepository.GetAllIncluding(p => p.Content).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Answer), id);
            }
            return entity;
        }
    }
}
