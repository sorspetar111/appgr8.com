using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
     

    public class Bank
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApiEndpoint { get; set; }
        public Guid CountryId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }

   
}
