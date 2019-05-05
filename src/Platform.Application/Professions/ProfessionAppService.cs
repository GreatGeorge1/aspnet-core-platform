using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    [AbpAuthorize]
    public class ProfessionAppService : AsyncCrudAppService<Profession,ProfessionDto, long, PagedResultDto<Profession>, ProfessionCreateDto, ProfessionUpdateDto>, IProfessionAppService
    {
        private readonly IRepository<Profession, long> _professionRepository;
        private readonly IRepository<ProfessionContent, long> _translationRepository;
        private readonly IRepository<Block, long> _blockRepository;
        private readonly IRepository<Author, long> _authorRepository;

        public ProfessionAppService(IRepository<Profession, long> professionRepository, IRepository<ProfessionContent, long> translationRepository, IRepository<Block, long> blockRepository, IRepository<Author, long> authorRepository)
            :base(professionRepository)
        {
            _professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
            _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            _blockRepository = blockRepository ?? throw new ArgumentNullException(nameof(blockRepository));
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ProfessionDto> CreateCopy(long id)
        {
            throw new NotImplementedException();
            
        }

        /// <summary>
        /// Обновляет контент профессии, где id => ProfessionTranslations.Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProfessionContentDto> UpdateContent(ProfessionContentDto input)
        {
            if (input.Id == 0)
            {
                throw new UserFriendlyException("id cannot be 0 or null");
            }
            var ts = ObjectMapper.Map<ProfessionContent>(input);
            var old = await _translationRepository.GetAllIncluding(p => p.Core).FirstOrDefaultAsync(p => p.Id == input.Id);

            old.Update(ts);
            await _translationRepository.InsertOrUpdateAsync(old);
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
                throw new UserFriendlyException("id в url не может быть 0 или null");
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
            return _professionRepository.GetAllIncluding(p => p.Content, p=>p.Events, p=>p.Packages, p=>p.Blocks, p => p.Author).AsQueryable();
        }

        protected override async Task<Profession> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new UserFriendlyException("id cannot be 0 or null");
            }
            var entity = await _professionRepository.GetAllIncluding(p => p.Content, p => p.Events, p => p.Packages, p => p.Blocks, p=>p.Author).FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task SetAuthor(long id,long authorid)
        {
            if (id == 0)
            {
                throw new UserFriendlyException("id cannot be 0 or null");
            }
            if (authorid == 0)
            {
                throw new UserFriendlyException("authorid cannot be 0 or null");
            }
            var author = await _authorRepository.FirstOrDefaultAsync(a => a.Id == authorid);
            var profession = await _professionRepository.GetAllIncluding(p => p.Author, p=>p.Content).FirstOrDefaultAsync(p => p.Id == id);
            profession.SetAuthor(author);
            //await _professionRepository.InsertOrUpdateAsync(profession);
        }
    }
}
