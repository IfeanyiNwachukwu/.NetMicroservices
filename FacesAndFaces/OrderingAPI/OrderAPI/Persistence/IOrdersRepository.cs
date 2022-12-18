using OrderAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OrderAPI.Persistence
{
    public interface IOrdersRepository
    {
        Task<Order> GetOrderAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task RegisterOrder(Order order);

        Order GetOrder(Guid id);
        void UpdateOrder(Order order);
    }
}
