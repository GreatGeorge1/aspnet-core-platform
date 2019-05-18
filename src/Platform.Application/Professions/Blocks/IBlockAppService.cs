﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IBlockAppService: IAsyncCrudAppService<BlockDto, long, PagedResultDto<Block>, BlockCreateDto, BlockUpdateDto>
    {
        Task<StepDto> CreateStep(StepCreateDto input, long id);
        Task DeleteStep(StepDeleteDto input);
        Task<BlockContentDto> UpdateContent(BlockContentUpdateDto input);
        Task ChangeContentVersion(long version);
    }
}
