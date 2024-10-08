using System;
using System.Collections.Generic;

namespace PaymentGateway.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
