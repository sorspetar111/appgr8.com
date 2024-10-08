using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
    

    public class Merchant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<MerchantPayment> MerchantPayments { get; set; }
    }

   
}
