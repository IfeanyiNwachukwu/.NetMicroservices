using System.Collections.Generic;
using System;

namespace MessagingInterfacesConstants.Events
{
    public interface IOrderProcessedEvent
    {
        Guid OrderId { get; }
        string PictureUrl { get; }
        List<byte[]> Faces { get; }
        string UserEmail { get; }
    }
}
