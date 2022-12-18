using MassTransit;
using MessagingInterfacesConstants.Commands;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OrderAPI.Models;
using OrderAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OrderAPI.Messages.Consumers
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        private IOrdersRepository _orderRepo;
        private readonly IHttpClientFactory _clientFactory;

        public RegisterOrderCommandConsumer(IOrdersRepository orderRepo, IHttpClientFactory clientFactory)
        {
            _orderRepo = orderRepo;
            _clientFactory = clientFactory;
        }

        public async Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            var result = context.Message;

            if(result.OrderId != null && result.PictureUrl != null && result.UserEmail != null && result.ImageData != null){

                SaveOrder(result);

                var client = _clientFactory.CreateClient();
                Tuple<List<byte[]>,Guid> orderDetailData = await GetFacesFromFaceApiAsync(client, result.ImageData, result.OrderId);

                List<byte[]> faces = orderDetailData.Item1;
                Guid orderId = orderDetailData.Item2;

                SaveOrderDetails(orderId, faces);


            }
            //return Task.FromResult(true);
        }

        private async Task<Tuple<List<byte[]>, Guid>> GetFacesFromFaceApiAsync(HttpClient client, byte[] imageData, Guid orderId)
        {
           var byteContecnt = new ByteArrayContent(imageData);
           Tuple<List<byte[]>, Guid> orderDetailData = null;
           byteContecnt.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var urlAddress = $"http://localhost:6001/api/Faces?orderId={orderId}";
            using (var response = await client.PostAsync(urlAddress, byteContecnt))
            {
                string apiResponse = await response.Content.ReadAsStringAsync(); 

                orderDetailData = JsonConvert.DeserializeObject<Tuple<List<byte[]>, Guid>>(apiResponse);
            }

            return orderDetailData;
        }

        private void SaveOrderDetails(Guid orderId, List<byte[]> faces)
        {
            var order = _orderRepo.GetOrderAsync(orderId).Result;

            if(order != null)
            {
                order.Status = Status.Processed;
                foreach(var face in faces)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = orderId,
                        FaceData = face
                    };
                    order.OrderDetails.Add(orderDetail);
                }
                _orderRepo.UpdateOrder(order);
            }
        }

        private void SaveOrder(IRegisterOrderCommand result)
        {
            Order order = new Order
            {
                OrderId = result.OrderId,
                UserEmail = result.UserEmail,
                Status = Status.Registered,
                PictureUrl = result.PictureUrl,
                ImageData = result.ImageData
            };

            _orderRepo.RegisterOrder(order);
        }
    }
}
