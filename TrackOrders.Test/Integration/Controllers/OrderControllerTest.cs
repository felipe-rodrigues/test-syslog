using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TrackOrders.Data.ValueObjects;
using TrackOrders.Test.Integration.ApiBase;
using TrackOrders.ViewModels;

namespace TrackOrders.Test.Integration.Controllers
{
    [Collection("Test collection")]
    public class OrderControllerTest
    {

        private readonly TrackOrdersApiFactory _api;

        public OrderControllerTest(TrackOrdersApiFactory api)
        {
            _api = api;
        }

        [Fact]
        public async Task Add_WithCorrectData_ShouldReturnOkWithData()
        {
            var client = _api.HttpAuthenticatedClient;

            var cep = "01310100";
            var addressCall = await client.GetAsync($"/api/address/cep/{cep}");
            addressCall.StatusCode.Should().Be( HttpStatusCode.OK );
            var address = await  addressCall.Content.ReadFromJsonAsync<AddressResponse>();

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

            var orderResponse = await client.PostAsJsonAsync("/api/order", orderRequest);

            orderResponse.StatusCode.Should().Be( HttpStatusCode.OK );
        }

        [Fact]
        public async Task Add_WithCorrectData_ShouldReturnOkWithData_AndSendNotificationToSignaR()
        {
            var client = _api.HttpAuthenticatedClient;

            var cep = "01310100";
            var addressCall = await client.GetAsync($"/api/address/cep/{cep}");
            addressCall.StatusCode.Should().Be(HttpStatusCode.OK);
            var address = await addressCall.Content.ReadFromJsonAsync<AddressResponse>();

            var orderRequest = new OrderRequest()
            {
                Number = "002",
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
            message.Should().Be("Pedido 002 criado");
        }

        [Fact]
        public async Task AddDeliveryAttempt_WithCorrectData_ShouldReturnOkWithData()
        {
            var client = _api.HttpAuthenticatedClient;

            var cep = "01310100";
            var addressCall = await client.GetAsync($"/api/address/cep/{cep}");
            addressCall.StatusCode.Should().Be(HttpStatusCode.OK);
            var address = await addressCall.Content.ReadFromJsonAsync<AddressResponse>();

            var orderRequest = new OrderRequest()
            {
                Number = "003",
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
            var orderResponse = await client.PostAsJsonAsync("/api/order", orderRequest);
            orderResponse.StatusCode.Should().Be(HttpStatusCode.OK);


            var saveAttemptDeliveryCall = await client.PutAsJsonAsync($"/api/order/{orderRequest.Number}/delivery?delivered=false", new { });
            saveAttemptDeliveryCall.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await saveAttemptDeliveryCall.Content.ReadAsAsync<Delivery>();
            content.HasDelivered.Should().BeFalse();
            content.Attempt.Should().Be(1);
            content.DateTime.Should().BeCloseTo(DateTime.Now,TimeSpan.FromSeconds(30));
        }


        [Fact]
        public async Task AddDeliveryAttempt_WithCorrectData_ShouldUpdateCurrentOrder_WithNewAttempt()
        {
            var client = _api.HttpAuthenticatedClient;

            var cep = "01310100";
            var addressCall = await client.GetAsync($"/api/address/cep/{cep}");
            addressCall.StatusCode.Should().Be(HttpStatusCode.OK);
            var address = await addressCall.Content.ReadFromJsonAsync<AddressResponse>();

            var orderRequest = new OrderRequest()
            {
                Number = "004",
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
            var orderResponse = await client.PostAsJsonAsync("/api/order", orderRequest);
            orderResponse.StatusCode.Should().Be(HttpStatusCode.OK);


            var saveAttemptDeliveryCall = await client.PutAsJsonAsync($"/api/order/{orderRequest.Number}/delivery?delivered=false", new { });
            saveAttemptDeliveryCall.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await saveAttemptDeliveryCall.Content.ReadAsAsync<Delivery>();
            content.HasDelivered.Should().BeFalse();
            content.Attempt.Should().Be(1);
            content.DateTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(30));


            var orderGetCall = await client.GetAsync($"/api/order/{orderRequest.Number}");
            orderGetCall.StatusCode.Should().Be(HttpStatusCode.OK);
            var orderGetContent = await orderGetCall.Content.ReadAsAsync<OrderResponse>();

            orderGetContent.Deliveries.Should().HaveCount(1);
            orderGetContent.Deliveries.First().HasDelivered.Should().BeFalse();
        }


    }
}
