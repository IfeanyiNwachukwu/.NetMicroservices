namespace MessagingInterfacesConstants.Constants
{
    public class RabbitMqMassTransitConstants
    {
        public const string RabbitMquri = "rabbitmq://rabbitmq/";

        public const string UserName = "guest";

        public const string Password = "guest";

        public const string RegisterOrderCommandQueue = "register.order.command";

        public const string NotificationServiceQueue = "notification.service.queue";

        public const string OrderDispatchedServiceQueue = "order.dispatch.service.queue";
    }
}
