using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Platform.Professions
{
    [Audited]
    public class AnswerTranslation : AuditedEntity<long>, IEntityTranslation<Answer>,IMedia
    {
        public Answer Core { get; set; }
        public int CoreId { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
    }
}