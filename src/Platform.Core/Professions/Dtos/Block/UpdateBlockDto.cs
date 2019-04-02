using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(Platform.Professions.Block))]
    public class UpdateBlockDto:CreateBlockDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public new ICollection<UpdateBlockTranslationDto> Translations { get; set; }
    }
}
