using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos.Answer;
using Platform.Professions.Dtos.Step;

namespace Platform.Professions
{
    public class StepTestAppService : ApplicationService,IStepTestAppService
    {
        private readonly IRepository<StepTest, long> stepTestRepository;
        private readonly IRepository<StepTranslations, long> translationRepository;
        private readonly IRepository<Answer, long> answerRepository;
        public StepTestAppService(
            IRepository<StepTest, long> stepTestRepository,
            IRepository<StepTranslations, long> translationRepository,
            IRepository<Answer, long> answerRepository)
        {
            this.stepTestRepository = stepTestRepository;
            this.translationRepository = translationRepository;
            this.answerRepository = answerRepository;
        }

        public async Task AddAnswer(CreateAnswerDto input, long id)
        {
            var step = await stepTestRepository.FirstOrDefaultAsync(p => p.Id == id);
            var answer = ObjectMapper.Map(input, new Answer());
            answer.StepTest = step;
            var newid = await answerRepository.InsertAndGetIdAsync(answer);
        }

        public async Task AddTranslation(CreateStepTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new StepTranslations());
            var step = await stepTestRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            step.Translations.Add(translation);
        }

        public async Task<StepReplyOkDto> DeleteTestStep(long id)
        {
            var step = await stepTestRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            step.IsDeleted = true;
            step.IsActive = false;
            var res = await stepTestRepository.InsertOrUpdateAndGetIdAsync(step);
            return new StepReplyOkDto { id = res, message = "deleted" };
        }

        public async Task DeleteTranslation(DeleteStepTranslationDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await translationRepository.InsertOrUpdateAsync(ts);
        }

        public async Task<GetTestStepAllDto> GetTestStepAll(long id)
        {
            var step = await stepTestRepository.GetAllIncluding(p => p.Translations)
             .FirstOrDefaultAsync(p => p.Id == id);
            var res = ObjectMapper.Map(step, new GetTestStepAllDto());
            return res;
        }

        public async Task RemoveAnswer(RemoveAnswerDto input)
        {
            //TODO cleanup
            var step = await stepTestRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.StepTestId);
            var answer = await answerRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.AnswerId);
            answer.IsActive = false;
            answer.IsDeleted = true;
        }

        public async Task<StepReplyOkDto> UpdateTestStep(UpdateTestStepDto input)
        {
            var step = await stepTestRepository.GetAllIncluding(p => p.Translations)
             .FirstOrDefaultAsync(p => p.Id == input.Id);
            step.Translations.Clear();
            var upd = ObjectMapper.Map(input, step);
            var res = await stepTestRepository.InsertOrUpdateAndGetIdAsync(upd);
            return new StepReplyOkDto { id = res, message = "updated" };
        }

        public async Task UpdateTranslation(UpdateStepTranslationDto input)
        {
            var ts = ObjectMapper.Map<StepTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }
    }
}
