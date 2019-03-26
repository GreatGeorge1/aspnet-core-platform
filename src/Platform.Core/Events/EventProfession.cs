using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Events
{
    public class EventProfession
    {
        public long EventId { get; set; }
        public Event Event { get; set; }
        public long ProfessionId { get; set; }
        public Profession Profession { get; set; }
    }
}
