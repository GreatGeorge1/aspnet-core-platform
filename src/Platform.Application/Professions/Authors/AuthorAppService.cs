using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions.Authors
{
    public class AuthorAppService : AsyncCrudAppService<Author, AuthorDto, long, PagedResultDto<Author>, AuthorCreateDto, AuthorCreateDto>, IAuthorAppService
    {
        private readonly IRepository<Author, long> repository;

        public AuthorAppService(IRepository<Author, long> repository) : base(repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override async Task<Author> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new UserFriendlyException("id cannot be 0 or null");
            }
            var entity = await repository.GetAllIncluding(p => p.Professions).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Author), id);
            }
            return entity;
        }

        protected override IQueryable<Author> CreateFilteredQuery(PagedResultDto<Author> input)
        {
            return repository.GetAllIncluding(p => p.Professions).AsQueryable();
        }
    }
}
