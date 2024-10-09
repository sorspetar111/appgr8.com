using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Services
{

	public class GeoLocationService : IGeoLocationService
	{
		private readonly PaymentGatewayDbContext _context;		


		public GeoLocationService(PaymentGatewayDbContext context)
		{
			_context = context;			
		}
	
        public async Task<string> GetServerUrlByIp(string ipAddress)
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

			var server = _context.ServerURLs.FirstOrDefault(c => c.Country.Code == countryCode);
			return server .URL;

            // Dummy mappings servers. For the production you need separate database table or some cache loading at the begging??? 
            // return countryCode switch
            // {
            //     "US" => "https://us.paymentgateway.com",
            //     "CA" => "https://ca.paymentgateway.com",
            //     "GB" => "https://uk.paymentgateway.com",
            //     "AU" => "https://au.paymentgateway.com",
                
            //     _ => null
            // };
        }




	}

}