using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(ProfessionContent))]
    public class ProfessionContentDto : GenericContentDto<ProfessionContent, long>
    {
    }
}
