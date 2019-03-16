using Abp.AutoMapper;
using AutoMapper;
using Platform.Professions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions
{
    internal static class ProfessionListDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration, MultiLingualMapContext context)
        {
            configuration.CreateMultiLingualMap<Profession, long, ProfessionTranslations, ProfessionListDto>(context);
        }
    }
}
