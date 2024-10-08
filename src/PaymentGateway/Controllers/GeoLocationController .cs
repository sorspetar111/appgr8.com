// Controllers/GeoLocationController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoLocationController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public GeoLocationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

      
        [HttpGet("server-url/{ipAddress}")]
        public async Task<IActionResult> GetServerUrlByIp(string ipAddress)
        {
            

            /*
                Warning!!! Example public geo-location API endpoint

                GetServerUrlByIp can work fully on-promises with special project and local database contain IP address range for entire world country.
                Please research if we need to not use 3th party providers like "ipapi.co". What if this service stopped? 
                This approach is very simply just for the tests and not for real production.

            */

            string geoApiUrl = $"https://ipapi.co/{ipAddress}/country/";

            try
            {
                
                var response = await _httpClient.GetStringAsync(geoApiUrl);
                var countryCode = response.Trim(); 

                // Map country codes to server URLs
                string serverUrl = GetServerUrlForCountry(countryCode);
                if (string.IsNullOrEmpty(serverUrl))
                {
                    return NotFound("No server found for the specified country.");
                }

                return Ok(new { CountryCode = countryCode, ServerUrl = serverUrl });
            }
            catch
            {
                return BadRequest("Invalid IP address or could not retrieve country information.");
            }
        }

        private string GetServerUrlForCountry(string countryCode)
        {

            // Dummy mappings servers. For the production you need separate database table or some cache loading at the begging??? 
            return countryCode switch
            {
                "US" => "https://us.paymentgateway.com",
                "CA" => "https://ca.paymentgateway.com",
                "GB" => "https://uk.paymentgateway.com",
                "AU" => "https://au.paymentgateway.com",
                
                _ => null
            };
        }
    }
}
