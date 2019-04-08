using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Platform.Events.Dtos;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Platform.Events
{
    public class EventAppService : AsyncCrudAppService<Event, EventDto, long, PagedResultDto<Event>, CreateEventDto, UpdateEventDto>, IEventAppService
    {
        private readonly IRepository<Event, long> eventRepository;
        private readonly IRepository<Profession, long> professionRepository;
       
      

        public EventAppService(IRepository<Event, long> repository, 
            IRepository<Profession,long> professionRepository
            ) : base(repository)
        {
            eventRepository = repository;
            this.professionRepository = professionRepository;
        }

        public async Task AddProfession(AddEventProfessionDto input)
        {
            var event1 = await eventRepository.GetAll().Where(e => e.Id == input.EventId).FirstOrDefaultAsync();
            var profession = await professionRepository.GetAll().Where(e => e.Id == input.ProfessionId).FirstOrDefaultAsync();
            if (event1.EventProfessions == null)
            {
                event1.EventProfessions = new List<EventProfession>();
            }
            event1.EventProfessions.Add(new EventProfession { Event=event1, Profession=profession});
        }

        public Task AddTranslation(AddEventTranslationDto input, long id)
        {
            throw new NotImplementedException();
        }

        //public async Task<EventDto> Create(CreateEventDto input)
        //{
        //    var event1 = ObjectMapper.Map<Event>(input);
        //    var newid = await eventRepository.InsertAndGetIdAsync(event1);
        //    var event2 = await eventRepository.GetAll().Where(e => e.Id == newid).SingleOrDefaultAsync();
        //    return ObjectMapper.Map<EventDto>(event2);
        //}

        public async Task<EventDto> CreateCopy(long id)
        {
            var event1 = await eventRepository.GetAll().Where(e => e.Id == id).FirstOrDefaultAsync();
            var newid = await eventRepository.InsertAndGetIdAsync(event1);
            return ObjectMapper.Map<EventDto>(
                await eventRepository.GetAll().Where(e => e.Id == newid).FirstOrDefaultAsync());    
        }

    

        public Task RemoveProfession(RemoveEventProfessionDto input)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTranslation(RemoveEventTranslationDto input)
        {
            throw new NotImplementedException();
        }


        public Task UpdateTranslation(EventTranslationDto input)
        {
            throw new NotImplementedException();
        }
    }
}
