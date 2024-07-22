using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MongoDb;
using TrackOrders.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TrackOrders.Data.Context;
using Microsoft.AspNetCore.SignalR.Client;
using TrackOrders.Hubs;

namespace TrackOrders.Test.Integration.ApiBase
{
    public class TrackOrdersApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly MongoDbContainer _database = new MongoDbBuilder()
          .WithImage("mongo:6.0")
          .Build();

        public HttpClient HttpClient { get; private set; }
        public HttpClient HttpAuthenticatedClient => AdminClient();

        public async Task InitializeAsync()
        {
            Environment.SetEnvironmentVariable("TEST_MODE", "true");
            await _database.StartAsync();
            HttpClient = CreateClient();
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions));
                services.RemoveAll(typeof(TrackOrdersContext));

                var connection = _database.GetConnectionString();

                var options = new DbContextOptionsBuilder<TrackOrdersContext>()
                                   .UseMongoDB(connection, databaseName: "trackorderstest")
                                   .Options;

                services.AddSingleton(sp => options);
                services.AddDbContext<TrackOrdersContext>(op =>
                {
                    op.UseMongoDB(connection, databaseName: "trackorderstest");
                });
            });


        }

        private HttpClient AdminClient()
        {
            var client = CreateClient();
            var token = GetAdminToken().Result;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return client;
        }

        public async Task<string> GetAdminToken()
        {
            var req = await HttpClient.PostAsJsonAsync("/api/auth", new UserLoginRequest
            {
                Email = "admin@gmail.com",
                Password = "12345"
            });
            var response = await req.Content.ReadAsAsync<UserLoginResponse>();

            return response.Token;
        }

        public async Task<HubConnection> GetNotificationHub()
        {
            var handler = Server.CreateHandler();

            var hub = new HubConnectionBuilder()
                .WithUrl("ws://localhost/notificationHub", o =>
                {
                    o.HttpMessageHandlerFactory = _ => handler;
                })
                .Build();

            await hub.StartAsync();
            
            return hub;
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            Environment.SetEnvironmentVariable("TEST_MODE", null);
            await _database.StopAsync();
        }
    }
}
