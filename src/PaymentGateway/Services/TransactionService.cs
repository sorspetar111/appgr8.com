using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Services
{

	public class TransactionService : ITransactionService
	{
		private readonly PaymentGatewayDbContext _context;		  
        // private readonly IStringLocalizer<GatewayController> _localizer;
		
		private IStringLocalizer<GatewayController> _localizer => HttpContext.RequestServices.GetService(typeof(IStringLocalizer<GatewayController>)) as IStringLocalizer<GatewayController>;


		public TransactionService(PaymentGatewayDbContext context)
		{
			_context = context;			
		}

		// public TransactionService(PaymentGatewayDbContext context, IStringLocalizer<GatewayController> _localizer)
		// {
		// 	_context = context;
		// 	_localizer = localizer;
		// }


		public async Task<TransactionResult> CreateTransactionAsync(TransactionRequest request)
        {
            var transactionGuid = GenerateNewTransactionGuid();

            var transaction = new Transaction
            {
                TransactionGuid = transactionGuid,
                CreditCardNumber = request.CreditCardNumber,
                Amount = request.Amount,
                SellerCountry = request.SellerCountry,
                TransactionDate = DateTime.UtcNow,
                TransactionStatus = _localizer["Pending"]
            };

            _context.Transactions.Add(transaction);

 			var transactionStatus = new TransactionStatusTable
			{
				Id = Guid.NewGuid(), // this may be autoincrement
				TransactionId = transaction.Id,
				Status = _localizer["Unknown"],
				StatusMessage = _localizer["Transaction status is currently unknown."]
			};

			_context.TransactionStatusTable.Add(transactionStatus);

            await _context.SaveChangesAsync();

            return new TransactionResult
            {
                IsSuccess = true,
                TransactionGuid = transactionGuid
            };
        }

		/*
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
		*/

		public async Task<string> CheckTransactionStatusAsync(Guid transactionGuid, string url)
		{
			var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionGuid == transactionGuid);

			if (transaction == null)
			{
				return string.Empty;
			}
			
			if (transaction.IsCanceled)
			{
				return _localizer["Canceled"];
			}
			
			if (EvaluateRisk(transaction.CardOwnerCountry, transaction.MerchantCountry))
			{
				transaction.TransactionStatus = _localizer["Blocked"];
				await _context.SaveChangesAsync();
				return _localizer["Blocked"];
			}
			
			if (transaction.TransactionStatus == _localizer["Pending"])
			{
				// await SimulateBankResponseAsync(transaction, url);
				transaction.TransactionStatus = _localizer[ CallBankApiForStatus(transaction, url)];
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
			transaction.TransactionStatus = _localizer["Canceled"];
			transaction.CanceledDate = DateTime.UtcNow;

			await _context.SaveChangesAsync();
			return true;
		}
		
		private Guid GenerateNewTransactionGuid()
		{
			return Guid.NewGuid();
		}
		
		private bool EvaluateRisk(string cardOwnerCountry, string merchantCountry)
		{
			
			return !string.Equals(cardOwnerCountry, merchantCountry, StringComparison.OrdinalIgnoreCase);
		}

		 // Simulated function for calling the bank's API to get the transaction status
        // TODO: Do not forget to remove Random ;) Must implement real bank request and response. Create a new service for BankService        
        private string CallBankApiForStatus(Transaction transaction,  string url)
        {
            var statuses = new[] { "Approved", "Declined", "Expired", "InsufficientFunds" };

			// calling here bank api by some url from context urls 
            // implement here

            return statuses[new Random().Next(statuses.Length)];
        }

		private async Task SimulateBankResponseAsync(Transaction transaction, string url)
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