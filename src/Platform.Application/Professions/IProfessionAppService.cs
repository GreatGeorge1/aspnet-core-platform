using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;
using Platform.Professions.User;

namespace Platform.Professions
{
    public interface IProfessionAppService:IAsyncCrudAppService<ProfessionDto, long, PagedResultDto<Profession>, ProfessionCreateDto, ProfessionUpdateDto>
    {
        Task<ProfessionDto> CreateCopy(long id);
        Task<BlockDto> CreateBlock(BlockCreateDto input, long id);
        Task DeleteBlock(BlockDeleteDto input);
        Task<ProfessionContentDto> UpdateContent(ProfessionContentUpdateDto input);
        Task ChangeContentVersion(long version);
        Task SetAuthor(long id, long authorid);

        Task Subscribe(long professionid);
        Task<ICollection<UserProfessionsDto>> GetSubscriptions();
        Task Unsubscribe(long professionid);
        Task<bool> CheckSubscribe(long professionid);

        Task<ICollection<UserProfessionsDto>> GetUserSubscriptions(long userid);
        //Task Subscribe(long userid, long professionid);
    }
}
