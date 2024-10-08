using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
     
    public class Report
    {
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public int TotalTransactions { get; set; }
        public int CanceledTransactions { get; set; }
        public int FraudAttempts { get; set; }
        public int HighRiskTransactions { get; set; }
    }
     

   
}
