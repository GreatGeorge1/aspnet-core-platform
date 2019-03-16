using System.Threading.Tasks;
using Abp.Application.Services;
using Platform.Sessions.Dto;

namespace Platform.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
