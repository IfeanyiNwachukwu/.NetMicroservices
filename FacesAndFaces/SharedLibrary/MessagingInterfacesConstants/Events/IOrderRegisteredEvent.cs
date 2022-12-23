using System;

namespace MessagingInterfacesConstants.Events
{
    public interface IOrderRegisteredEvent
    {
        Guid OrderId { get; }
    }
}
