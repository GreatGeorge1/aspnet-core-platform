using Abp.AutoMapper;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(BlockTranslations))]
    public class GetBlockTranslationDto:UpdateBlockTranslationDto
    {
    }
}
