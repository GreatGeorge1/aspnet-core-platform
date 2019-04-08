using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Events.Dtos
{
    public class RemoveEventProfessionDto
    {
        public long EventId { get; set; }
        public long ProfessionId { get; set; }
    }
}
