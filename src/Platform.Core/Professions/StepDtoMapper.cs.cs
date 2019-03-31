using Abp.AutoMapper;
using AutoMapper;
using Platform.Professions.Dtos.Step;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions
{
    internal static class StepDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration, MultiLingualMapContext context)
        {
           // configuration.CreateMultiLingualMap<StepInfo, long, StepTranslations, GetInfoStepDto>(context);
            //  configuration.CreateMultiLingualMap<Block, long, BlockTranslations, GetProfeDto>(context);
        }
    }
}
