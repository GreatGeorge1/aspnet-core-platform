﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Platform.Professions.Block))]
    public class BlockDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public ICollection<BlockTranslationDto> Translations { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
        public ICollection<StepBaseDto> Steps { get; set; }
    }
}
