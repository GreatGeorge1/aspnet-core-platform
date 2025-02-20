﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.MultiTenancy.Dto;

namespace Platform.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

