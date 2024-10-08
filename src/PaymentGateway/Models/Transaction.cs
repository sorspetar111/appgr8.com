using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
             
  	public class Transaction
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public Guid BankId { get; set; }
        public Guid UserId { get; set; }
        public Guid PaymentId { get; set; }
        public string CreditCardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string AccountCurrency { get; set; }
        public string ClientIp { get; set; }        
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsHighRisk { get; set; }

        public Merchant Merchant { get; set; }
        public Bank Bank { get; set; }
        public User User { get; set; }
        public Payment Payment { get; set; }
        public ICollection<TransactionStatus> TransactionStatuses { get; set; }
        public ICollection<RiskManagement> RiskManagements { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
   

    
    public class TransactionRequest
    {
        public int MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int BankId { get; set; }

        
        public string CreditCardNumber { get; set; } = string.Empty;
        public string CardOwnerCountry { get; set; } = string.Empty;
        public string MerchantCountry { get; set; } = string.Empty;
    }


    [Osolate("old version. Use Transaction. Check Transaction for any missing properties.")]
    public class Transaction__
    {
        public int TransactionId { get; set; }
        public Guid TransactionGuid { get; set; } = Guid.NewGuid();
        public int MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int BankId { get; set; }
        public string TransactionStatus { get; set; } = "Pending";
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
      
        public bool IsCanceled { get; set; } = false;
        public DateTime? CanceledDate { get; set; }
    }


}
