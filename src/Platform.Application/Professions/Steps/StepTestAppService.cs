﻿using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public class StepTestAppService : AsyncCrudAppService<StepTest, StepTestDto, long, PagedResultDto<StepTest>, StepTestCreateDto, StepTestUpdateDto>, IStepTestAppService
    {
        private readonly IRepository<StepTest, long> stepTestRepository;
        private readonly IRepository<StepTranslations, long> translationRepository;
        private readonly IRepository<Answer, long> answerRepository;
        public StepTestAppService(
            IRepository<StepTest, long> stepTestRepository,
            IRepository<StepTranslations, long> translationRepository,
            IRepository<Answer, long> answerRepository):base(stepTestRepository)
        {
            this.stepTestRepository = stepTestRepository;
            this.translationRepository = translationRepository;
            this.answerRepository = answerRepository;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<StepTestDto> Create(StepTestCreateDto input)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAnswer(AnswerCreateDto input, long id)
        {
            var step = await stepTestRepository.FirstOrDefaultAsync(p => p.Id == id);
            var answer = ObjectMapper.Map(input, new Answer());
            answer.StepTest = step;
            var newid = await answerRepository.InsertAndGetIdAsync(answer);
        }

        public async Task CreateTranslation(StepTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new StepTranslations());
            translation.Id = 0;
            var step = await stepTestRepository.GetAllIncluding(p => p.Translations)
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

      

        public async Task DeleteAnswer(AnswerDeleteDto input)
        {
            //TODO cleanup
            var step = await stepTestRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.StepTestId);
            var answer = await answerRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.AnswerId);
            answer.IsActive = false;
            answer.IsDeleted = true;
        }
     
        public async Task UpdateTranslation(StepTranslationDto input)
        {
            var ts = ObjectMapper.Map<StepTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }
    }
}
