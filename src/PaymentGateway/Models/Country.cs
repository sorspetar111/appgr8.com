using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
     
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<Merchant> Merchants { get; set; }
        public ICollection<Bank> Banks { get; set; }
        public ICollection<ServerURL> ServerURLs { get; set; }
    }
     

   
}
