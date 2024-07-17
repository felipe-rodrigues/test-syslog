using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackOrders.Test.Integration.ApiBase;

namespace TrackOrders.Test.Integration.Controllers
{
    [Collection("Test collection")]
    public class AddressControllerTest
    {

        private readonly TrackOrdersApiFactory _api;

        public AddressControllerTest(TrackOrdersApiFactory api)
        {
            _api = api;
        }

        [Fact]
        public async Task SearchCEP_WithCorrectData_ShouldReturnOkWithData()
        {

            var client = _api.HttpClient;
            var cep = "37135226";
            var response = await client.GetAsync($"/api/address/cep/{cep}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
