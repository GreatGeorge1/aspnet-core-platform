using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    //[Authorize]
    public class ProfessionAppService : AsyncCrudAppService<Profession,ProfessionDto, long, PagedResultDto<Profession>, ProfessionCUDto, ProfessionCUDto>, IProfessionAppService
    {
        private readonly IRepository<Profession, long> _professionRepository;
        private readonly IRepository<ProfessionContent, long> _translationRepository;
        private readonly IRepository<Block, long> _blockRepository;

        public ProfessionAppService(IRepository<Profession, long> professionRepository, IRepository<ProfessionContent, long> translationRepository, IRepository<Block, long> blockRepository)
            :base(professionRepository)
        {
            _professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
            _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            _blockRepository = blockRepository ?? throw new ArgumentNullException(nameof(blockRepository));
        }

        public async Task<ProfessionDto> CreateCopy(long id)
        {
            throw new NotImplementedException();
            
        }

        /// <summary>
        /// Обновляет перевод профессии, где id => ProfessionTranslations.Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProfessionContentDto> UpdateContent(ProfessionContentDto input)
        {
            var ts = ObjectMapper.Map<ProfessionContent>(input);
            var old = await _translationRepository.GetAllIncluding(p => p.Core).FirstOrDefaultAsync(p => p.Id == input.Id);
            

            var updts = await _translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            return ObjectMapper.Map<ProfessionContentDto>(updts);
        }

        public async Task DeleteBlock(BlockDeleteDto input)
        {
            var prof = await _professionRepository.GetAllIncluding(p => p.Content)
              .FirstOrDefaultAsync(p => p.Id == input.ProfessionId);
            var block = await _blockRepository.GetAllIncluding(p => p.Content)
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
            return _professionRepository.GetAllIncluding(p => p.Content, p=>p.Events, p=>p.Packages, p=>p.Blocks).AsQueryable();
        }

        protected override async Task<Profession> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var entity = await _professionRepository.GetAllIncluding(p => p.Content, p => p.Events, p => p.Packages, p => p.Blocks).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Profession), id);
            }
            return entity;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Task ChangeContentVersion(long contentid)
        {
            throw new NotImplementedException();
        }

    }
}
