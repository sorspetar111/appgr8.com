using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public string PaymentType { get; set; }

        public ICollection<MerchantPayment> MerchantPayments { get; set; }
    }
  

   
}
