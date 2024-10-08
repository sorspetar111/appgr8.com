using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<PaymentGatewayDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configure supported cultures
var supportedCultures = new[] { "en", "fr", "de", "es", "zh" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = supportedCultures.Select(c => new CultureInfo(c)).ToArray();
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

// Register services
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Register HTTP clients
builder.Services.AddHttpClient<GeoLocationController>();
builder.Services.AddHttpClient<ReportsController>();
builder.Services.AddHttpClient<GatewayController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// Enable localization
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Map controllers
app.MapControllers();

app.Run();


/*
old version
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<PaymentGatewayDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddHttpClient<GeoLocationController>();
builder.Services.AddHttpClient<ReportsController>();
builder.Services.AddHttpClient<GatewayController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
*/


/*
old version

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;

namespace PaymentGateway
{
 
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // services.AddHostedService<ReportBackgroundService>();
                    
                    services.AddHttpClient<GeoLocationController>();
                    services.AddHttpClient<ReportsController>();
					services.AddHttpClient<GatewayController>();
					
					// TODO: Inject TransactionService

                    services.AddDbContext<PaymentGatewayDbContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                });
    }


}



*/

