using Abp.AutoMapper;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(BlockTranslations))]
    public class UpdateBlockTranslationDto:CreateBlockTranslationsDto
    {
        /// <summary>
        /// BlockTRanslations Id
        /// </summary>
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
