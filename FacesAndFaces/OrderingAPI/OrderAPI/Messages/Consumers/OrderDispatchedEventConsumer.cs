using MassTransit;
using MessagingInterfacesConstants.Events;
using Microsoft.AspNetCore.SignalR;
using OrderAPI.Hubs;
using OrderAPI.Models;
using OrderAPI.Persistence;
using System;
using System.Threading.Tasks;

namespace OrderAPI.Messages.Consumers
{
    public class OrderDispatchedEventConsumer : IConsumer<IOrderDispatchedEvent>
    {
        private readonly IOrdersRepository _orderRepository;
        private readonly IHubContext<OrderHub> _hubContext;
        public OrderDispatchedEventConsumer(IOrdersRepository orderRepository, IHubContext<OrderHub> hubContext)
        {
            _orderRepository = orderRepository;
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<IOrderDispatchedEvent> context)
        {
            var message = context.Message;
            Guid OrderId = message.OrderId;

            UpdateDatabase(OrderId);
            await _hubContext.Clients.All.SendAsync("UpdateOrders", new Object[] {"Order Dispatched", OrderId });

          
        }

        private void UpdateDatabase(Guid orderId)
        {
            var order = _orderRepository.GetOrder(orderId);
            if (order != null)
            {
                order.Status = Status.Sent;
                _orderRepository.UpdateOrder(order);


            }
        }
    }
}
