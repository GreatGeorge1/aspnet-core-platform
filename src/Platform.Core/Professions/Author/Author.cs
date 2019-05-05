using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions
{
    public class Author : AuditedEntity<long>, IExtendableObject
    {
        public string ExtensionData { get; set; }
        public string Name { get; set; }
        public string Base64Image { get; set; }
        public ICollection<Profession> Professions { get; set; }

        public void SetOrUpdateExtesnsionData<T>(string key, T value) where T : Type
        {
            this.SetData(key, value);
        }

        public T GetExtensionData<T>(string key) where T : Type
        {
            return this.GetData<T>(key);
        }
    }
}
