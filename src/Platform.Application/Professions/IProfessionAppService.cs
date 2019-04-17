using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IProfessionAppService:IAsyncCrudAppService<ProfessionDto, long, PagedResultDto<Profession>, ProfessionCreateDto, ProfessionUpdateDto>
    {
        Task<ProfessionDto> CreateCopy(long id);
        Task<BlockDto> AddBlock(BlockCreateDto input, long id);
        Task RemoveBlock(BlockDeleteDto input);
        Task AddTranslation(ProfessionTranslationDto input, long id);
        Task<ProfessionTranslationDto> UpdateTranslation(ProfessionTranslationDto input);
        Task DeleteTranslation(ProfessionTranslationDeleteDto input);
    }
}
