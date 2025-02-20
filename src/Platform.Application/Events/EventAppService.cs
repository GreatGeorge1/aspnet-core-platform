﻿using Abp.Application.Services;
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
        private readonly IRepository<EventTranslations, long> translationRepository;
      

        public EventAppService(IRepository<Event, long> repository, 
            IRepository<Profession,long> professionRepository,
            IRepository<EventTranslations, long> translationRepository
            ) : base(repository)
        {
            eventRepository = repository;
            this.professionRepository = professionRepository;
            this.translationRepository = translationRepository;
        }

        public async Task AddProfession(AddEventProfessionDto input)
        {
            var event1 = await eventRepository.GetAllIncluding(e=>e.EventProfessions)
                .Where(e => e.Id == input.EventId).FirstOrDefaultAsync();
            var profession = await professionRepository.GetAll()
                .Where(e => e.Id == input.ProfessionId).FirstOrDefaultAsync();
            if (event1.EventProfessions == null)
            {
                event1.EventProfessions = new List<EventProfession>();
            }
            event1.EventProfessions.Add(new EventProfession { Event=event1, Profession=profession});
        }

        public async Task AddTranslation(AddEventTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new EventTranslations());
            var event1 = await eventRepository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            event1.Translations.Add(translation);
        }

        //public async Task<EventDto> Create(CreateEventDto input)
        //{
        //    var event1 = ObjectMapper.Map<Event>(input);
        //    var newid = await eventRepository.InsertAndGetIdAsync(event1);
        //    var event2 = await eventRepository.GetAll().Where(e => e.Id == newid).SingleOrDefaultAsync();
        //    return ObjectMapper.Map<EventDto>(event2);
        //}
        /// <summary>
        /// Govno, no rabotaet
        /// </summary>
        public async Task<EventDto> CreateCopy(long id)
        {
            var event1 = await eventRepository.GetAllIncluding(p => p.Translations, p => p.EventProfessions)
              .Where(e => e.Id == id).FirstOrDefaultAsync();

            var event2 = new Event
            {
                DateStart = event1.DateStart,
                DateEnd=event1.DateEnd,
                IsActive = false,
                Translations = new List<EventTranslations>(),
                EventProfessions = new List<EventProfession>()
            };
            var oldts = event1.Translations.ToList();
            foreach (var item in oldts)
            {
                var ts = ObjectMapper.Map<AddEventTranslationDto>(item);
                ts.Title = "Copy -- " + ts.Title;
                event2.Translations.Add(
                    ObjectMapper.Map<EventTranslations>(ts));
            }

            var newid = await eventRepository.InsertAndGetIdAsync(event2);

            event2 = await eventRepository.GetAllIncluding(p => p.Translations, p => p.EventProfessions)
                .Where(e => e.Id == newid).FirstOrDefaultAsync();

            var oldpp = event1.EventProfessions.ToList();
            foreach (var item in oldpp)
            {
                var prof = await professionRepository.GetAll()
               .Where(e => e.Id == item.ProfessionId).FirstOrDefaultAsync();
                var ts = new EventProfession { Profession = prof, Event = event2 };
                event2.EventProfessions.Add(ts);
            }

            return ObjectMapper.Map<EventDto>(event2);
        }

    

        public async Task RemoveProfession(RemoveEventProfessionDto input)
        {
            var event1 = await eventRepository.GetAllIncluding(e=>e.EventProfessions)
                .Where(e => e.Id == input.EventId)
                .FirstOrDefaultAsync();
            // event1.Where(e=>e.EventId==input)
            var ep = event1.EventProfessions.Where(ep => ep.ProfessionId == input.ProfessionId).FirstOrDefault();
            event1.EventProfessions.Remove(ep);
        }

        public async Task RemoveTranslation(RemoveEventTranslationDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.EventTranslationId);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await translationRepository.InsertOrUpdateAsync(ts);
        }


        public async Task UpdateTranslation(EventTranslationDto input)
        {
            var ts = ObjectMapper.Map<EventTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }
    }
}
