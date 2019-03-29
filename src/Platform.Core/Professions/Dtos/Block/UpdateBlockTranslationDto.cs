using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(BlockTranslations))]
    public class UpdateBlockTranslationDto:CreateBlockTranslationsDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
