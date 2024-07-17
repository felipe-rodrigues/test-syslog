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
    public class AuthControllerTest
    {

        private readonly TrackOrdersApiFactory _api;

        public AuthControllerTest(TrackOrdersApiFactory api)
        {
            _api = api;
        }

        [Fact]
        public async Task Login_WithAdminTest_ShouldReturnOk()
        {
            var loginRequest = new UserLoginRequest()
            {
                Email = "admin@gmail.com",
                Password = "12345"
            };

            var client = _api.HttpClient;
            var responseAuth = await client.PostAsJsonAsync("/api/auth", loginRequest);

            responseAuth.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task Login_WithAdminTest_WithWrongPassword_ShouldReturnBadRequest()
        {
            var loginRequest = new UserLoginRequest()
            {
                Email = "admin@gmail.com",
                Password = "111111"
            };

            var client = _api.HttpClient;
            var responseAuth = await client.PostAsJsonAsync("/api/auth", loginRequest);

            responseAuth.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }



        [Fact]
        public async Task Login_WithNewUser_ShouldReturnOk()
        {
            var userRequest = new UserRequest()
            {
                Email = "test@gmail.com",
                Name = "tester",
                Password = "password",
                ConfirmPassword = "password"
            };

            var loginRequest = new UserLoginRequest()
            {
                Email = userRequest.Email,
                Password = userRequest.Password,
            };

            var client = _api.HttpAuthenticatedClient;
            var responseAuth = await client.PostAsJsonAsync("/api/user", userRequest);

            responseAuth.StatusCode.Should().Be(HttpStatusCode.OK);

            var unauthorizedClient = _api.HttpClient;

            var responseLogin = await unauthorizedClient.PostAsJsonAsync("api/auth", loginRequest);
            responseLogin.StatusCode.Should().Be(HttpStatusCode.OK);

        }


    }
}
