using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
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

        public async Task CreateTranslation(AnswerTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new AnswerTranslation());
            translation.Id = 0;
            var answer = await answerRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            answer.Translations.Add(translation);
        }

        public async Task DeleteTranslation(AnswerTranslationDeleteDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await translationRepository.InsertOrUpdateAsync(ts);
        }

        public async Task UpdateTranslation(AnswerTranslationDto input)
        {
            var ts = ObjectMapper.Map<AnswerTranslation>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }
    }
}
