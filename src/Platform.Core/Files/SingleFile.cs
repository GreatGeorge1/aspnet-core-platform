using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Files
{
    public class SingleFile : Entity<long>
    {
        public string Path { get; set; }
    }
}
