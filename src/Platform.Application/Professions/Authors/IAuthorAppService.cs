using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Authors
{
    public interface IAuthorAppService : IAsyncCrudAppService<AuthorDto, long, PagedResultDto<Author>, AuthorCreateDto, AuthorCreateDto>
    {
    }
}
