using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Events.Dtos
{
    [AutoMap(typeof(UserEvents))]
    public class UserEventsDto:EntityDto<long>
    {
        public int Score { get; set; }
        public bool IsCompleted { get; set; }
        public long EventId { get; set; }
       // public Event Event { get; set; }
        public long UserId { get; set; }
       // public User User { get; set; }
    }
}
