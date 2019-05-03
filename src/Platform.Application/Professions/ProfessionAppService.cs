using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    //[Authorize]
    public class ProfessionAppService : AsyncCrudAppService<Profession,ProfessionDto, long, PagedResultDto<Profession>, ProfessionCreateDto, ProfessionUpdateDto>, IProfessionAppService
    {
        private readonly IRepository<Profession, long> _professionRepository;
        private readonly IRepository<ProfessionTranslations, long> _translationRepository;
        private readonly IRepository<Block, long> _blockRepository;

        public ProfessionAppService(IRepository<Profession, long> professionRepository,
            IRepository<ProfessionTranslations, long> translationRepository, 
            IRepository<Block, long> blockRepository):base(professionRepository)
        {
            _professionRepository = professionRepository;
            _translationRepository = translationRepository;
            _blockRepository = blockRepository;
        }

        public async Task AddTranslation(ProfessionTranslationDto input, long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var translation = ObjectMapper.Map(input, new ProfessionTranslations());
            translation.Id = 0;
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            prof.Translations.Add(translation);
        }

        public async Task<ProfessionDto> CreateCopy(long id)
        {
            throw new NotImplementedException();
            
        }

        public async Task DeleteTranslation(ProfessionTranslationDeleteDto input)
        {
            var ts = await _translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            await _translationRepository.DeleteAsync(ts);
        }

        /// <summary>
        /// Обновляет перевод профессии, где id => ProfessionTranslations.Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProfessionTranslationDto> UpdateTranslation(ProfessionTranslationDto input)
        {
            var ts = ObjectMapper.Map<ProfessionTranslations>(input);
            var updid= await _translationRepository.InsertOrUpdateAndGetIdAsync(ts);
            var updts = await _translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            return ObjectMapper.Map<ProfessionTranslationDto>(updts);
        }

        public async Task DeleteBlock(BlockDeleteDto input)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.ProfessionId);
            var block = await _blockRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.BlockId);
            block.IsActive = false;
            block.IsDeleted = true;
        }

        public async Task<BlockDto> CreateBlock(BlockCreateDto input, long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var prof = await _professionRepository.FirstOrDefaultAsync(p => p.Id == id);
            var block = ObjectMapper.Map<Block>(input);
            block.Profession = prof;
            var newid = await _blockRepository.InsertAndGetIdAsync(block);
            var b = await _blockRepository.FirstOrDefaultAsync(p => p.Id == newid);
            return ObjectMapper.Map<BlockDto>(b);
        }

        protected override IQueryable<Profession> CreateFilteredQuery(PagedResultDto<Profession> input)
        {
            return _professionRepository.GetAllIncluding(p => p.Translations, p=>p.EventProfessions, p=>p.PackageProfessions, p=>p.Blocks).AsQueryable();
        }

        protected override async Task<Profession> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var entity = await _professionRepository.GetAllIncluding(p => p.Translations, p => p.EventProfessions, p => p.PackageProfessions, p => p.Blocks).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Profession), id);
            }
            return entity;
        }
    }
}
