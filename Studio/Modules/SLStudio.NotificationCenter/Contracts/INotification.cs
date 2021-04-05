using System;

namespace SLStudio.NotificationCenter
{
    public interface INotification
    {
        string Id { get; }
        string Title { get; }
        string Description { get; }
        string Content { get; }
        DateTime PublicationDate { get; }
    }
}