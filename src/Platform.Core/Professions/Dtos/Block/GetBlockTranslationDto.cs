using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(Professions.BlockTranslations))]
    public class GetBlockTranslationDto:UpdateBlockTranslationDto
    {
    }
}
