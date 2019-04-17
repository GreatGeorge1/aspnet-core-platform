using Abp.AutoMapper;
using AutoMapper;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    internal static class ProfessionListDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration, MultiLingualMapContext context)
        {
            configuration.CreateMultiLingualMap<Profession, long, ProfessionTranslations, GetProfessionsDto>(context);
            configuration.CreateMultiLingualMap<Profession, long, ProfessionTranslations, GetProfessionDto>(context);
        }
    }
}
