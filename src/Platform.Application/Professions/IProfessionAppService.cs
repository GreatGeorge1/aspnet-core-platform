using Platform.Professions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions
{
    public interface IProfessionAppService
    {
        Task CreateProfession(ProfessionDto input);
        Task UpdateProfessionAsync(ProfessionUpdateDto input);
        Task<IEnumerable<ProfessionListDto>> GetProfessions();
        Task<IEnumerable<ProfessionUpdateDto>> GetProfessionsAll();

    }
}
