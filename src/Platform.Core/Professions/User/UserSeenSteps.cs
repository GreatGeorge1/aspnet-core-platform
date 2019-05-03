using Abp.Domain.Entities.Auditing;

namespace Platform.Professions.User
{
    /// <summary>
    /// показывает просматривал юзер шаг или нет
    /// </summary>
    public class UserSeenSteps: AuditedEntity<long>
    {
        public UserProfessions UserProfession { get; set; }
        public Step Step { get; set; }// Step.Type==Test
    }
}
