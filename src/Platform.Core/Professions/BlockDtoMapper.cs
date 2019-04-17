using Abp.AutoMapper;
using AutoMapper;
using Platform.Professions.Dtos.Block;

namespace Platform.Professions
{
    internal static class BlockDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration, MultiLingualMapContext context)
        {
            configuration.CreateMultiLingualMap<Block, long, BlockTranslations, GetBlockDto>(context);
          //  configuration.CreateMultiLingualMap<Block, long, BlockTranslations, GetProfeDto>(context);
        }
    }
}
