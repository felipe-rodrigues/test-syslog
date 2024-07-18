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

namespace TrackOrders.Test.Integration.Controllers
{
    [Collection("Test collection")]

    public class DeliveryControllerTest
    {

        private readonly TrackOrdersApiFactory _api;

        public DeliveryControllerTest(TrackOrdersApiFactory api)
        {
            _api = api;
        }


        [Fact]
        public async Task Save_WithExistentOrder_ShouldReturnOkWithData_AndOrderShouldBeUpdated()
        {
            var client = _api.HttpAuthenticatedClient;

            var cep = "01310100";
            var addressCall = await client.GetAsync($"/api/address/cep/{cep}");
            addressCall.StatusCode.Should().Be(HttpStatusCode.OK);
            var address = await addressCall.Content.ReadFromJsonAsync<AddressResponse>();

            var orderRequest = new OrderRequest()
            {
                Number = "0012",
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

            var saveDeliveryCall = await client.PostAsJsonAsync($"/api/delivery/{orderRequest.Number}", new { });
            saveDeliveryCall.StatusCode.Should().Be(HttpStatusCode.OK);

            var deliveryData = await saveDeliveryCall.Content.ReadAsAsync<DeliveryResponse>();

            deliveryData.OrderNumber.Should().Be(orderRequest.Number);
            deliveryData.HasDelivered.Should().BeTrue();

            var checkOrderResponse = await client.GetAsync($"/api/order/{orderRequest.Number}");
            checkOrderResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var orderContent = await checkOrderResponse.Content.ReadAsAsync<OrderResponse>();

            orderContent.Should().NotBeNull();
            orderContent.HasDelivered.Should().BeTrue();
        }



        [Fact]
        public async Task Save_WithExistentOrder_ButNotDelivered_ShouldReturnOkWithData_AndOrderShouldNotBeUpdated()
        {
            var client = _api.HttpAuthenticatedClient;

            var cep = "01310100";
            var addressCall = await client.GetAsync($"/api/address/cep/{cep}");
            addressCall.StatusCode.Should().Be(HttpStatusCode.OK);
            var address = await addressCall.Content.ReadFromJsonAsync<AddressResponse>();

            var orderRequest = new OrderRequest()
            {
                Number = "0013",
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

            var saveDeliveryCall = await client.PostAsJsonAsync($"/api/delivery/{orderRequest.Number}?delivered=false", new { });
            saveDeliveryCall.StatusCode.Should().Be(HttpStatusCode.OK);

            var deliveryData = await saveDeliveryCall.Content.ReadAsAsync<DeliveryResponse>();

            deliveryData.OrderNumber.Should().Be(orderRequest.Number);
            deliveryData.HasDelivered.Should().BeFalse();

            var checkOrderResponse = await client.GetAsync($"/api/order/{orderRequest.Number}");
            checkOrderResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var orderContent = await checkOrderResponse.Content.ReadAsAsync<OrderResponse>();

            orderContent.Should().NotBeNull();
            orderContent.HasDelivered.Should().BeFalse();
        }
    }
}
