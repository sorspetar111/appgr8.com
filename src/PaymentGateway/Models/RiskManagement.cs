using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
     
 	public class RiskManagement
    {
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public bool IsFraud { get; set; }
        public bool IsHighRisk { get; set; }
        public string RiskReason { get; set; }
        public DateTime CreatedAt { get; set; }

        public Transaction Transaction { get; set; }
    }
}