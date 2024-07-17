using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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
    }
}
