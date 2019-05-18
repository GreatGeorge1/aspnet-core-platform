﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IStepAppService : IAsyncCrudAppService<StepDto, long, PagedResultDto<Step>, StepCreateDto, StepUpdateDto>
    {
        Task<StepContentDto> UpdateContent(StepContentUpdateDto input);   
        Task<AnswerDto> CreateAnswer(AnswerCreateDto input, long id);
        Task DeleteAnswer(AnswerDeleteDto input);
    }
}
