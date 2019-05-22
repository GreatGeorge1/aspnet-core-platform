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
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;
using Platform.Professions.User;
using Platform.Subscribes;

namespace Platform.Professions
{
    [AbpAuthorize]
    public class ProfessionAppService : AsyncCrudAppService<Profession,ProfessionDto, long, PagedResultDto<Profession>, ProfessionCreateDto, ProfessionUpdateDto>, IProfessionAppService
    {
        [NotNull]private readonly IRepository<Profession, long> _professionRepository;
        [NotNull]private readonly IRepository<ProfessionContent, long> _translationRepository;
        [NotNull]private readonly IRepository<Block, long> _blockRepository;
        [NotNull]private readonly IRepository<Author, long> _authorRepository;
        [NotNull]private readonly ISubscribeManager _subscribeManager;

        public ProfessionAppService([NotNull]IRepository<Profession, long> professionRepository, 
            [NotNull] IRepository<ProfessionContent, long> translationRepository, 
            [NotNull] IRepository<Block, long> blockRepository, 
            [NotNull] IRepository<Author, long> authorRepository,
            [NotNull] ISubscribeManager subscribeManager)
            :base(professionRepository)
        {
            _professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
            _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            _blockRepository = blockRepository ?? throw new ArgumentNullException(nameof(blockRepository));
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
            _subscribeManager = subscribeManager ?? throw new ArgumentNullException(nameof(subscribeManager));
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ProfessionDto> CreateCopy(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновляет контент профессии, где id => Content.Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProfessionContentDto> UpdateContent(ProfessionContentUpdateDto input)
        {
            if (input.Id == 0)
            {
                throw new UserFriendlyException("id cannot be 0 or null");
            }
            var ts = ObjectMapper.Map<ProfessionContent>(input);
            var old = await _translationRepository.GetAllIncluding(p => p.Core)
                .FirstOrDefaultAsync(p => p.Id == input.Id)??throw new EntityNotFoundException(typeof(ProfessionContent),input.Id);

            old.Update(ts);
            await _translationRepository.InsertOrUpdateAsync(old);
            var updts = await _translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            return ObjectMapper.Map<ProfessionContentDto>(updts);
        }

        public async Task DeleteBlock(BlockDeleteDto input)
        {
            if (input.BlockId == 0)
            {
                throw new UserFriendlyException("BlockId не может быть 0 или null");
            }
            var block = await _blockRepository.GetAllIncluding(p => p.Content)
              .FirstOrDefaultAsync(p => p.Id == input.BlockId) ?? throw new EntityNotFoundException(typeof(Block), input.BlockId);
            await _blockRepository.DeleteAsync(block);
        }

        public async Task<BlockDto> CreateBlock(BlockCreateDto input, long id)
        {
            if (id == 0)
            {
                throw new UserFriendlyException("id в url не может быть 0 или null");
            }
            var prof = await _professionRepository.FirstOrDefaultAsync(p => p.Id == id)??throw new EntityNotFoundException(typeof(Profession),id);
            var block = ObjectMapper.Map<Block>(input);
            block.Profession = prof;
            var newid = await _blockRepository.InsertAndGetIdAsync(block);
            var b = await _blockRepository.FirstOrDefaultAsync(p => p.Id == newid);
            return ObjectMapper.Map<BlockDto>(b);
        }

        protected override IQueryable<Profession> CreateFilteredQuery(PagedResultDto<Profession> input)
        {
            return _professionRepository.GetAllIncluding(p => p.Content, p=>p.Event, p=>p.Package, p=>p.Blocks, p => p.Author).AsQueryable();
        }

        protected override async Task<Profession> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new UserFriendlyException("id cannot be 0 or null");
            }
            var entity = await _professionRepository.GetAllIncluding(p => p.Content, p => p.Event, p => p.Package, p => p.Blocks, p=>p.Author).FirstOrDefaultAsync(p => p.Id == id);
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
            var author = await _authorRepository.FirstOrDefaultAsync(a => a.Id == authorid)??throw new EntityNotFoundException(typeof(Author),authorid);
            var profession = await _professionRepository.GetAllIncluding(p => p.Author, p=>p.Content)
                .FirstOrDefaultAsync(p => p.Id == id)??throw new EntityNotFoundException(typeof(Profession),id);
            profession.SetAuthor(author);
            //await _professionRepository.InsertOrUpdateAsync(profession);
        }

        public async Task Subscribe(long professionid)
        {
            var userid = AbpSession.UserId ?? 0;
            if (userid != 0)
            {
                if (professionid == 0)
                {
                    throw new UserFriendlyException("professionid in uri cannot be null or 0");
                }

                await _subscribeManager.SubscribeToProfession(userid, professionid);
            }
            else
            {
                throw new UserFriendlyException("session user not set");
            }
        }
        public async Task Unsubscribe(long professionid)
        {
            var userid = AbpSession.UserId ?? 0;
            if (userid != 0)
            {
                if (professionid == 0)
                {
                    throw new UserFriendlyException("professionid in uri cannot be null or 0");
                }

                await _subscribeManager.UnsubscribeToProfession(userid, professionid);
            }
            else
            {
                throw new UserFriendlyException("session user not set");
            }
        }

        public async Task<ICollection<UserProfessionsDto>> GetSubscriptions()
        {
            var userid = AbpSession.UserId ?? 0;
            if (userid != 0)
            {
                var res = await _subscribeManager.GetSubscriptions(userid);
                var result = new List<UserProfessionsDto>();
                foreach (var item in res)
                {
                    result.Add(ObjectMapper.Map<UserProfessionsDto>(item));
                }
                return result;
            }

            throw new UserFriendlyException("session user not set");
        }
        
        public async Task<ICollection<UserProfessionsDto>> GetUserSubscriptions(long userid)
        {
            if (userid != 0)
            {
                var res = await _subscribeManager.GetSubscriptions(userid);
                var result = new List<UserProfessionsDto>();
                foreach (var item in res)
                {
                    result.Add(ObjectMapper.Map<UserProfessionsDto>(item));
                }
                return result;
            }

            throw new UserFriendlyException("user not set");
        }
        
        public async Task<bool> CheckSubscribe(long professionid)
        {
            var userid = AbpSession.UserId ?? 0;
            if (userid != 0)
            {
                if (professionid == 0)
                {
                    throw new UserFriendlyException("professionid in uri cannot be null or 0");
                }

                var res=await _subscribeManager.UserIsSubscribed(userid, professionid);
                return res;

            }
            else
            {
                throw new UserFriendlyException("session user not set");
            }
        }
    }
}
