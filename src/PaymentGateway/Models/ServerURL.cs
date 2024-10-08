using System;
using System.Collections.Generic;


// ServerUrls Table
// ServerUrlId (PK)	CountryCode (FK)	ServerUrl
// 1	US	https://us.paymentgateway.com
// 2	CA	https://ca.paymentgateway.com
// 3	GB	https://uk.paymentgateway.com
// 4	AU	https://au.paymentgateway.com


namespace PaymentGateway.Models
{
     
   	public class ServerURL
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string URL { get; set; }

        public Country Country { get; set; }
    }

   
}