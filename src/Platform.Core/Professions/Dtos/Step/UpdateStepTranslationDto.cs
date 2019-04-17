using Abp.AutoMapper;

namespace Platform.Professions.Dtos.Step
{
    [AutoMap(typeof(StepTranslations))]
    public class UpdateStepTranslationDto:CreateStepTranslationDto
    {
        /// <summary>
        /// StepTranslations Id
        /// </summary>
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
