using System.Threading.Tasks;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IProfessionAppService
    {
        Task<ProfessionReplyOkDto> CreateProfession(ProfessionCreateDto input);
        Task<ProfessionReplyOkDto> CreateCopy(long id);
        Task<ProfessionReplyOkDto> UpdateProfession(ProfessionUpdateDto input);
        Task<ProfessionReplyOkDto> DeleteProfession(long id);
        //Task<IEnumerable<GetProfessionsDto>> GetProfessions();
        Task<GetProfessionDto> GetProfession(long id);
        Task<GetProfessionAllDto> GetProfessionAllLang(long id);
        //Task<IEnumerable<GetProfessionAllDto>> GetProfessionsAll();
        Task AddBlock(AddBlockDto input, long id);
        Task RemoveBlock(RemoveBlockDto input);
        Task AddTranslation(AddProfessionTranslationDto input, long id);
        Task UpdateTranslation(UpdateProfessionTranslationDto input);
        Task DeleteTranslation(DeleteProfessionTranslationDto input);
        //Task<IEnumerable<GetProfessionTranslationDto>> GetTranslations(long id);
    }
}
