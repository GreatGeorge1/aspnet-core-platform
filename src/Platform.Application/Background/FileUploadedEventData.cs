using Abp.Events.Bus;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Background
{
    public class FileUploadedEventData : EventData
    {
        public long UserId { get; set; }
        public string FileName { get; set; }
        [EnumDataType(typeof(ParentType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParentType ParentType { get; set; }
        public long ParentId { get; set; }
    }
    
    public class FileDeletedEventData : EventData
    {
        public long UserId { get; set; }
        public string FileName { get; set; }
        [EnumDataType(typeof(ParentType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParentType ParentType { get; set; }
        public long ParentId { get; set; }
    }
    
}
