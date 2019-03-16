using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public class ProfessionAppService : ApplicationService, IProfessionAppService
    {
        private readonly IRepository<Profession, long> _professionRepository;

        public ProfessionAppService(IRepository<Profession, long> professionRepository)
        {
            _professionRepository = professionRepository;
        }

        public async Task CreateProfession(ProfessionDto input)
        {
            var prof = ObjectMapper.Map<Profession>(input);
            //prof.
            await _professionRepository.InsertAsync(prof);
        }

        public async Task<IEnumerable<ProfessionListDto>> GetProfessions()
        {
            var profs = await _professionRepository.GetAllIncluding(p => p.Translations)
               .ToListAsync();
            var res = new List<ProfessionListDto>();
            foreach(var item in profs)
            {
                res.Add(ObjectMapper.Map(item, new ProfessionListDto()));
            }
            return res;
        }

        public async Task UpdateProfessionAsync(ProfessionUpdateDto input)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p=>p.Id == input.Id);

            prof.Translations.Clear();

            ObjectMapper.Map(input, prof);
        }

        Task<IEnumerable<ProfessionUpdateDto>> IProfessionAppService.GetProfessionsAll()
        {
            throw new NotImplementedException();
        }
    }
}
