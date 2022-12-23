using System;

namespace MessagingInterfacesConstants.Events
{
    public interface IOrderDispatchedEvent
    {
        Guid OrderId { get; }

        DateTime DispatchDateTime { get; }
    }
}
