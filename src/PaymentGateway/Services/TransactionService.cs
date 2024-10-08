using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Services
{

	public class TransactionService : ITransactionService
	{
		private readonly ApplicationDbContext _context;

		public TransactionService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Guid> CreateTransactionAsync(int merchantId, decimal amount, string currency, int bankId)
		{
			var transactionGuid = GenerateNewTransactionGuid();

			var transaction = new Transaction
			{
				MerchantId = merchantId,
				Amount = amount,
				Currency = currency,
				BankId = bankId,
				TransactionGuid = transactionGuid
			};

			_context.Transactions.Add(transaction);
			await _context.SaveChangesAsync();

			return transactionGuid;
		}

		public async Task<string?> CheckTransactionStatusAsync(Guid transactionGuid)
		{
			var transaction = await _context.Transactions
				.FirstOrDefaultAsync(t => t.TransactionGuid == transactionGuid);

			if (transaction == null)
			{
				return null;
			}

			// First check: Is the transaction canceled?
			if (transaction.IsCanceled)
			{
				return "Canceled";
			}

			// Second check: Evaluate risk based on country comparison
			if (EvaluateRisk(transaction.CardOwnerCountry, transaction.MerchantCountry))
			{
				transaction.TransactionStatus = "Blocked";
				await _context.SaveChangesAsync();
				return "Blocked";
			}

			// If not canceled and not blocked, proceed with bank API call
			if (transaction.TransactionStatus == "Pending")
			{
				await SimulateBankResponseAsync(transaction);
			}

			return transaction.TransactionStatus;
		}

		public async Task<bool> CancelTransactionAsync(Guid transactionGuid)
		{
			var transaction = await _context.Transactions
				.FirstOrDefaultAsync(t => t.TransactionGuid == transactionGuid);

			if (transaction == null || transaction.IsCanceled)
			{
				return false;
			}

			transaction.IsCanceled = true;
			transaction.TransactionStatus = "Canceled";
			transaction.CanceledDate = DateTime.UtcNow;

			await _context.SaveChangesAsync();
			return true;
		}

		// Private function to generate a new GUID
		private Guid GenerateNewTransactionGuid()
		{
			return Guid.NewGuid();
		}

		// Private function to evaluate the risk based on the card and merchant countries
		private bool EvaluateRisk(string cardOwnerCountry, string merchantCountry)
		{
			// Simple rule: If the card owner and merchant are in different countries, mark as high risk
			return !string.Equals(cardOwnerCountry, merchantCountry, StringComparison.OrdinalIgnoreCase);
		}

		private async Task SimulateBankResponseAsync(Transaction transaction)
		{
			await Task.Delay(5000);

			if (!transaction.IsCanceled)
			{
				var random = new Random();
				var status = random.Next(0, 4) switch
				{
					0 => "Approved",
					1 => "Failed",
					2 => "Wrong Card ID",
					3 => "Expired Credit Card",
					_ => "Insufficient Funds"
				};

				transaction.TransactionStatus = status;
				await _context.SaveChangesAsync();
			}
		}
	}

}