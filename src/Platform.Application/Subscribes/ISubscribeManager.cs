using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Platform.Professions.User;

namespace Platform.Subscribes
{
    public interface ISubscribeManager:IDomainService
    {
        Task<bool> SubscribeToProfession(long userid, long professionid);
        Task<bool> UnsubscribeToProfession(long userid, long professionid);
        Task<ICollection<UserProfessions>> GetSubscriptions(long userid);
    }
}