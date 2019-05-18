using Abp.Domain.Entities;
using System.Collections.Generic;

namespace Platform.Professions
{
    public interface IHasContent<TCore, TContent, TKey>
        where TCore : IEntity<TKey>
        where TContent : GenericContent<TCore, TKey>
    {
        TContent Content { get; set; }
    }
    
    public interface IHasContentCollection<TCore, TContent, TKey>
        where TCore : IEntity<TKey>
        where TContent : GenericContent<TCore, TKey>
    {
        ICollection<TContent> Content { get; set; }
    }
}