using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
     
     
    public class TransactionStatus
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }

        public Transaction Transaction { get; set; }
    }


   
}
