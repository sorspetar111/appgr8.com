using System;
using System.Collections.Generic;

namespace Report.Models
{

    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public TransactionStatus Status { get; set; }
        public bool IsHighRisk { get; set; }
    }

]