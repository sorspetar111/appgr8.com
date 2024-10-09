using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Interfaces
{
	 
	public interface ITransactionService
	{
		// Task<Guid> CreateTransactionAsync(int merchantId, decimal amount, string currency, int bankId);
		Task<TransactionResult> CreateTransactionAsync(TransactionRequest request)
		Task<string?> CheckTransactionStatusAsync(Guid transactionGuid);
		Task<bool> CancelTransactionAsync(Guid transactionGuid);
	}


}