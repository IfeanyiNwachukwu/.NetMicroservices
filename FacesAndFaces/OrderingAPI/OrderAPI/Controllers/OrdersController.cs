using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Persistence;
using System;
using System.Threading.Tasks;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _ordersRepository.GetOrdersAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("{orderId}", Name ="GetByOrderId")]
        public async Task<IActionResult> GetOrderById(string orderId)
        {
            var order = await _ordersRepository.GetOrderAsync(Guid.Parse(orderId));

            if(order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
    }
}
