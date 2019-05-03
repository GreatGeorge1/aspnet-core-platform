using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IProfessionAppService:IAsyncCrudAppService<ProfessionDto, long, PagedResultDto<Profession>, ProfessionCUDto, ProfessionCUDto>
    {
        Task<ProfessionDto> CreateCopy(long id);
        Task<BlockDto> CreateBlock(BlockCreateDto input, long id);
        Task DeleteBlock(BlockDeleteDto input);
        Task<ProfessionContentDto> UpdateContent(ProfessionContentDto input);
        Task ChangeContentVersion(long version);
    }
}
