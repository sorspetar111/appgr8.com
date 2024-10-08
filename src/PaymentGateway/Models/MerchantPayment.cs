using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
     
   	public class MerchantPayment
    {
        public Guid MerchantId { get; set; }
        public Guid PaymentId { get; set; }

        public Merchant Merchant { get; set; }
        public Payment Payment { get; set; }
    }
     

   
}
