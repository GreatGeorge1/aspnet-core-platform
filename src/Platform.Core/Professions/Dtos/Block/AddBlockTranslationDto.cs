using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(BlockTranslations))]
    public class AddBlockTranslationDto:CreateBlockTranslationsDto
    {
       
    }
}
