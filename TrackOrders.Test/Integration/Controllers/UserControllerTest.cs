using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrackOrders.Test.Integration.ApiBase;
using TrackOrders.ViewModels;

namespace TrackOrders.Test.Integration.Controllers
{
    [Collection("Test collection")]
    public class UserControllerTest
    {

        private readonly TrackOrdersApiFactory _api;

        public UserControllerTest(TrackOrdersApiFactory api)
        {
            _api = api;
        }

        [Fact]
        public async Task Add_WithUnauthenticatedUser_ShouldReturnUnauthorized()
        {
            var userRequest = new UserRequest()
            {
                Email = "test@gmail.com",
                Name = "tester",
                Password = "password",
                ConfirmPassword = "password"
            };

            var client = _api.HttpClient;

            var userResponse = await client.PostAsJsonAsync("api/user", userRequest);

            userResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Add_WithAuthenticatedUser_WithCorrectData_ShouldReturnOkWithData()
        {
            var userRequest = new UserRequest()
            {
                Email = "test@gmail.com",
                Name = "tester",
                Password = "password",
                ConfirmPassword = "password"
            };

            var client = _api.HttpAuthenticatedClient;

            var userResponse = await client.PostAsJsonAsync("api/user", userRequest);

            userResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task Add_WithAuthenticatedUser_WithConfirmPasswordWrong_ShouldReturnBadRequest()
        {
            var userRequest = new UserRequest()
            {
                Email = "test@gmail.com",
                Name = "tester",
                Password = "password",
                ConfirmPassword = "password12"
            };

            var client = _api.HttpAuthenticatedClient;

            var userResponse = await client.PostAsJsonAsync("api/user", userRequest);

            userResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
