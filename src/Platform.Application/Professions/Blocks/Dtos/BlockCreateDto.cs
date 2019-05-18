﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Professions.Block))]
    public class BlockCreateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public BlockContentDto Content { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
    }
}
