using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.Routing;
using TrackOrders;
using TrackOrders.Configuration;
using TrackOrders.Data;
using TrackOrders.Data.Context;
using TrackOrders.Data.Seed;
using TrackOrders.Hubs;
using TrackOrders.Services;
using TrackOrders.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var dbConnectionString = builder.Configuration.GetConnectionString("Mongo");
if (string.IsNullOrEmpty(dbConnectionString))
{
    throw new Exception("Mongo connection string is not set");
}

builder.Services.AddDatabase(dbConnectionString);
builder.Services.AddServices();
builder.Services.AddMapper();
var settings = builder.Configuration.GetSection("Settings").Get<AppSettings>();
builder.Services.AddJwtAuthentication(settings);
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Settings"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:44498", "https://localhost:7225", "http://localhost:5240")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});


builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<TrackOrdersContext>();
            context.Database.EnsureCreated();
            var passwordHasher = services.GetRequiredService<IPasswordHashService>();
            TrackOrdersContextSeed.Seed(context, passwordHasher);

        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while seeding the database." + ex.Message);
            throw;
        }
    }
}


app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapHub<NotificationHub>("/notificationHub");




app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();

public partial class Program { }

