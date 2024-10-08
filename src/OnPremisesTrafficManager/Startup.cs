using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                // Get client IP address
                var ip = context.Connection.RemoteIpAddress?.ToString();

                // Load the MaxMind GeoIP2 database
                var geoDbPath = "GeoIP/GeoLite2-Country.mmdb"; // Update with the correct path
                using var reader = new DatabaseReader(geoDbPath);
                
                // Lookup the client's IP
                var city = reader.City(ip);
                var country = city.Country.IsoCode; // Get the country code (e.g., "US", "DE", "FR")

                // Perform redirection based on the country code
                switch (country)
                {
                    case "US":
                        context.Response.Redirect("https://us.example.com");
                        break;
                    case "DE":
                        context.Response.Redirect("https://de.example.com");
                        break;
                    case "FR":
                        context.Response.Redirect("https://fr.example.com");
                        break;
                    default:
                        context.Response.Redirect("https://global.example.com");
                        break;
                }
            });
        });
    }
}
