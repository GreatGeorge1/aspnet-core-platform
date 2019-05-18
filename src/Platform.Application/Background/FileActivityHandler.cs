using Abp;
using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Abp.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Background
{
    public class FileActivityHandler : IEventHandler<FileUploadedEventData>, ITransientDependency
    {
        private readonly INotificationPublisher _notiticationPublisher;

        public FileActivityHandler(INotificationPublisher notiticationPublisher)
        {
            _notiticationPublisher = notiticationPublisher ?? throw new ArgumentNullException(nameof(notiticationPublisher));
        }

        public void HandleEvent(FileUploadedEventData eventData)
        {
            _notiticationPublisher.Publish("FileUploaded", 
                new FileUploadedNotificationData(eventData.FileName, eventData.ParentType, eventData.ParentId),
                userIds: new[] { new UserIdentifier(1, eventData.UserId) });
        }
    }

    [Serializable]
    public class FileUploadedNotificationData : NotificationData
    {
        public FileUploadedNotificationData(string fileName, ParentType parentType, long parentId)
        {
           // UserId = userId;
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            ParentType = parentType;
            ParentId = parentId;
        }

       // public long UserId { get; set; }
        public string FileName { get; set; }
        [EnumDataType(typeof(ParentType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParentType ParentType { get; set; }
        public long ParentId { get; set; }
    }
}
