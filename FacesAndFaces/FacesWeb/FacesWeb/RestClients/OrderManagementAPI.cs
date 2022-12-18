using FacesWeb.ViewModels;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacesWeb.RestClients
{
    public class OrderManagementAPI : IOrderManagementAPI
    {
        private IOrderManagementAPI _restClient;
        public OrderManagementAPI(IConfiguration config, HttpClient httpClient)
        {
            //_settings = settings;
            // string apiHostAndPort = _settings.Value.OrdersApiUrl;
            string apiHostAndPort = config.GetSection("ApiServiceLocations").
                GetValue<string>("OrdersApiLocation");
            httpClient.BaseAddress = new Uri($"https://{apiHostAndPort}/api");
            
            _restClient = RestService.For<IOrderManagementAPI>(httpClient);

        }
        public async Task<OrderViewModel> GetOrderById(Guid orderId)
        {
            try
            {
                return await _restClient.GetOrderById(orderId);
            }
            catch (ApiException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
              
            }
        }

        public async Task<List<OrderViewModel>> GetOrders()
        {
            return await _restClient.GetOrders();
        }
    }
}
