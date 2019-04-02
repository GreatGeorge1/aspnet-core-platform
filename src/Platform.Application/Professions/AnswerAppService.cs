using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos.Answer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Professions
{
    public class AnswerAppService : ApplicationService, IAnswerAppService
    {
        private readonly IRepository<Answer, long> answerRepository;
        private readonly IRepository<AnswerTranslation, long> translationRepository;
        public AnswerAppService(IRepository<Answer, long> answerRepository,
            IRepository<AnswerTranslation, long> translationRepository)
        {
            this.answerRepository = answerRepository;
            this.translationRepository = translationRepository;
        }

        public async Task AddTranslation(CreateAnswerTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new AnswerTranslation());
            var answer = await answerRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            answer.Translations.Add(translation);
        }

        public async Task<AnswerReplyOkDto> DeleteAnswer(long id)
        {
            var answer = await answerRepository.GetAllIncluding(p => p.Translations)
               .FirstOrDefaultAsync(p => p.Id == id);
            answer.IsDeleted = true;
            answer.IsActive = false;
            answer.Translations.ToList().ForEach(t=> 
            {
                t.IsActive = false;
                t.IsDeleted = true;
            });
            var res = await answerRepository.InsertOrUpdateAndGetIdAsync(answer);
            return new AnswerReplyOkDto { id = res, message = "deleted" };
        }

        public async Task DeleteTranslation(DeleteAnswerTranslationDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await translationRepository.InsertOrUpdateAsync(ts);
        }

        public async Task<GetAnswerAllDto> GetAnswerAll(long id)
        {
            var answer = await answerRepository.GetAllIncluding(p => p.Translations)
            .FirstOrDefaultAsync(p => p.Id == id);
            var res = ObjectMapper.Map(answer, new GetAnswerAllDto());
            return res;
        }

        public async Task<AnswerReplyOkDto> UpdateAnswer(UpdateAnswerDto input)
        {
            var answer = await answerRepository.GetAllIncluding(p => p.Translations)
             .FirstOrDefaultAsync(p => p.Id == input.Id);
            answer.Translations.Clear();
            var upd = ObjectMapper.Map(input, answer);
            var res = await answerRepository.InsertOrUpdateAndGetIdAsync(upd);
            return new AnswerReplyOkDto { id = res, message = "updated" };
        }

        public async Task UpdateTranslation(UpdateAnswerTranslationDto input)
        {
            var ts = ObjectMapper.Map<AnswerTranslation>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }
    }
}
