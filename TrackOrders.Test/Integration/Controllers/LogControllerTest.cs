using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrackOrders.Test.Integration.ApiBase;
using TrackOrders.ViewModels;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.SignalR.Client;

namespace TrackOrders.Test.Integration.Controllers
{
    [Collection("Test collection")]

    public class LogControllerTest
    {
        private readonly TrackOrdersApiFactory _api;

        public LogControllerTest(TrackOrdersApiFactory api)
        {
            _api = api;
        }

        [Fact]
        public async Task AddView_WithOrderCreated_WithCorrectOrderNumber_ShouldReturnOk()
        {

            var client = _api.HttpAuthenticatedClient;

            var cep = "01310100";
            var addressCall = await client.GetAsync($"/api/address/cep/{cep}");
            addressCall.StatusCode.Should().Be(HttpStatusCode.OK);
            var address = await addressCall.Content.ReadFromJsonAsync<AddressResponse>();

            var orderRequest = new OrderRequest()
            {
                Number = "001",
                Description = "description",
                Value = 5,
                DeliveryAddress = new AddressRequest()
                {
                    Number = "101",
                    City = address.City,
                    Code = address.Cep,
                    District = address.Neighborhood,
                    State = address.State,
                    Street = address.Street,
                }
            };

            var hub = await _api.GetNotificationHub();
            var message = "";

            hub.On<string>("OrderEvents", msg =>
            {
                message = msg;
            });

            var orderResponse = await client.PostAsJsonAsync("/api/order", orderRequest);
            orderResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var notificationRequest = new LogOrderNotificationRequest()
            {
                IsNewOrderNotification = true,
                HasDelivered = false,
                Message = message,
                OrderNumber = orderRequest.Number,
                Attempt = null
            };

            var addViewNotificationCall = await client.PostAsJsonAsync($"/api/log/order/{orderRequest.Number}/viewed", notificationRequest);
            addViewNotificationCall.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task AddView_WithOrderCreated_WithIncorrectOrderNumber_ShouldReturnBadRequest()
        {

            var client = _api.HttpAuthenticatedClient;
            var addViewNotificationCall = await client.PostAsJsonAsync($"/api/log/order/XXX/viewed", new { });

            addViewNotificationCall.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
