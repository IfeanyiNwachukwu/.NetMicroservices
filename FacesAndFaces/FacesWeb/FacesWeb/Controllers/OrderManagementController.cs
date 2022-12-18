using FacesWeb.RestClients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacesWeb.Controllers
{
    public class OrderManagementController : Controller
    {
        private readonly IOrderManagementAPI _orderManagementApi;

        public OrderManagementController(IOrderManagementAPI orderManagementApi)
        {
            _orderManagementApi = orderManagementApi;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderManagementApi.GetOrders();
            foreach(var order in orders)
            {
                order.ImageString = ConvertAndFormatToString(order.ImageData);
            }

            return View(orders);
        }
       
        [Route("/Details/{orderId}")]
        public async Task<IActionResult> Details(string orderId)
        {
            var order = await _orderManagementApi.GetOrderById(Guid.Parse(orderId));

            
            order.ImageString = ConvertAndFormatToString(order.ImageData);

            foreach (var detail in order.OrderDetails)
            {
                detail.ImageString = ConvertAndFormatToString(detail.FaceData);
            }

            return View(order);
        }
        private string ConvertAndFormatToString(byte[] imageData)
        {
            string imageBase64Data = Convert.ToBase64String(imageData);

            return string.Format("data:image/png;base64, {0}", imageBase64Data);
        }
    }
}
