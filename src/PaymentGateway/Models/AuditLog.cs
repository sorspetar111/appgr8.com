using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
     
     
    public class AuditLog
    {
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public string Action { get; set; }
        public string PerformedBy { get; set; }
        public DateTime Timestamp { get; set; }

        public Transaction Transaction { get; set; }
    }

   
}
