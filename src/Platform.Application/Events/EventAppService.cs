using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Platform.Events.Dtos;
using Platform.Professions;

namespace Platform.Events
{
    public class EventAppService : AsyncCrudAppService<Event, EventDto, long, PagedResultDto<Event>, EventCreateDto, EventCreateDto>, IEventAppService
    {
        private readonly IRepository<Event, long> eventRepository;
        private readonly IRepository<Profession, long> professionRepository;

        public EventAppService(IRepository<Event, long> eventRepository, IRepository<Profession, long> professionRepository)
            :base(eventRepository)
        {
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
        }

        public override async Task<EventDto> Create(EventCreateDto input)
        {
            var event1 = new Event
            {
                IsActive = input.IsActive,
                DateEnd = input.DateEnd,
                DateStart=input.DateStart
            };
            var profession = await professionRepository.FirstOrDefaultAsync(p => p.Id == input.ProfessionId) ?? throw new ArgumentNullException(nameof(Profession));
            event1.Profession = profession;
            var newid = await eventRepository.InsertAndGetIdAsync(event1);
            var np = await eventRepository.GetAllIncluding(p => p.Profession).FirstOrDefaultAsync(p => p.Id == newid);
            return ObjectMapper.Map<EventDto>(np);
        }

        public override async Task<EventDto> Update(EventCreateDto input)
        {
            var event1 = await eventRepository.GetAllIncluding(p => p.Profession).FirstOrDefaultAsync(p => p.Id == input.Id);
            event1.DateStart = input.DateStart;
            event1.DateEnd = input.DateEnd;
            event1.IsActive = input.IsActive;
            await eventRepository.InsertOrUpdateAsync(event1);
            return ObjectMapper.Map<EventDto>(event1);
        }

        protected override async Task<Event> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var entity = await eventRepository.GetAllIncluding( p => p.Profession).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Event), id);
            }
            return entity;
        }
    }
}
