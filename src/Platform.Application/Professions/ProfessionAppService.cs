using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    //[Authorize]
    public class ProfessionAppService : ApplicationService, IProfessionAppService
    {
        private readonly IRepository<Profession, long> _professionRepository;
        private readonly IRepository<ProfessionTranslations, long> _translationRepository;
        private readonly IRepository<Block, long> _blockRepository;

        public ProfessionAppService(IRepository<Profession, long> professionRepository, 
            IRepository<ProfessionTranslations, long> translationRepository, 
            IRepository<Block, long> blockRepository)
        {
            _professionRepository = professionRepository;
            _translationRepository = translationRepository;
            _blockRepository = blockRepository;
        }

        public async Task AddTranslation(AddProfessionTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new ProfessionTranslations());
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            prof.Translations.Add(translation);
        }

        public async Task<ProfessionReplyOkDto> CreateCopy(long id)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

            var newprofdto = ObjectMapper.Map(prof, new ProfessionCreateDto());
            var newprof = ObjectMapper.Map(newprofdto, new Profession());

            var newid = await _professionRepository.InsertAndGetIdAsync(newprof);
            return new ProfessionReplyOkDto { id = newid, message = "created" };
        }

        public async Task DeleteTranslation(DeleteProfessionTranslationDto input)
        {
            var ts = await _professionRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await _professionRepository.InsertOrUpdateAsync(ts);
        }

        /// <summary>
        /// Обновляет перевод профессии, где id => ProfessionTranslations.Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateTranslation(UpdateProfessionTranslationDto input)
        {
            var ts = ObjectMapper.Map<ProfessionTranslations>(input);
            await _translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }

        public async Task<ProfessionReplyOkDto> CreateProfession(ProfessionCreateDto input)
        {
            var prof = ObjectMapper.Map<Profession>(input);
            var newid = await _professionRepository.InsertAndGetIdAsync(prof);
            return new ProfessionReplyOkDto { id = newid, message = "created" };
        }

        public async Task<ProfessionReplyOkDto> UpdateProfession(ProfessionUpdateDto input)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
               .FirstOrDefaultAsync(p => p.Id == input.Id);
            prof.Translations.Clear();
            var upd = ObjectMapper.Map(input, prof);
            var res = await _professionRepository.InsertOrUpdateAndGetIdAsync(upd);
            return new ProfessionReplyOkDto { id = res, message = "updated" };
        }
        public async Task<ProfessionReplyOkDto> DeleteProfession(long id)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            prof.IsDeleted = true;
            prof.IsActive = false;
            var res = await _professionRepository.InsertOrUpdateAndGetIdAsync(prof);
            return new ProfessionReplyOkDto { id = res, message = "deleted" };
        }

        public async Task<GetProfessionDto> GetProfession(long id)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
               .FirstOrDefaultAsync(p => p.Id == id);
            var res = ObjectMapper.Map(prof, new GetProfessionDto());
            return res;
        }

        public async Task<GetProfessionAllDto> GetProfessionAll(long id)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
               .FirstOrDefaultAsync(p => p.Id == id);
            var res = ObjectMapper.Map(prof, new GetProfessionAllDto());
            return res;
        }


        public async Task RemoveBlock(RemoveBlockDto input)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.ProfessionId);
            var block = await _blockRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.BlockId);
            block.IsActive = false;
            block.IsDeleted = true;
        }

        public async Task AddBlock(AddBlockDto input, long id)
        {
            var prof = await _professionRepository.FirstOrDefaultAsync(p => p.Id == id);
            var block = ObjectMapper.Map<Block>(input);
            var newid = await _blockRepository.InsertAndGetIdAsync(block);
            var b = await _blockRepository.FirstOrDefaultAsync(p => p.Id == newid);
            b.Profession = prof;
        }
    }
}
