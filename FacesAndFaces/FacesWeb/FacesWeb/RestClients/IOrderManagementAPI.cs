using FacesWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacesWeb.RestClients
{
    public interface IOrderManagementAPI
    {
        [Get("/orders")]
        Task<List<OrderViewModel>> GetOrders();
       
        [Get("/orders/{orderId}")]
        Task<OrderViewModel> GetOrderById(Guid orderId);

    }
}
