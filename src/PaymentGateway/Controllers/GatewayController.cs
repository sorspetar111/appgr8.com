
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Controllers
{
    [ApiController]

	// TODO: Refactor [controller] to be replace with hardcode: "api/gatewaytransactions" or "api/transactions"  ??? 
    [Route("api/[controller]")]

	// TODO: Refactor name of controller. Gateway is something more like reverse proxy, Ocelot - .NET, etc...

	// TODO: Need to be implemented cache
	// Async API was implemented (check TransactionService) with guide but fully implementations need que pattern and SAGA (or manual distributive transaction que manager which is very complex and need many developers and time)
	// TODO: Fully scalable architecture? 

	// TODO: Inject TransactionService
 


    public class GatewayController : ControllerBase
    {
        private readonly PaymentGatewayDbContext _context;

		// TODO: Inject TransactionService vs direct _context aprouch or use it mix of both
		private readonly ITransactionService _transactionService;
         private readonly IStringLocalizer<GatewayController> _localizer;


        public GatewayController(PaymentGatewayDbContext context, ITransactionService transactionService, IStringLocalizer<GatewayController> localizer)
        {
            _context = context;
            _transactionService = transactionService;
            _localizer = localizer;
        }

        // Endpoint to create a new transaction with a unique GUID
        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequest request)
        {
            var transactionGuid = GenerateTransactionGuid();

            var transaction = new Transaction
            {
                TransactionGuid = transactionGuid,
                CreditCardNumber = request.CreditCardNumber,
                Amount = request.Amount,
                SellerCountry = request.SellerCountry,
                TransactionDate = DateTime.UtcNow,
                TransactionStatus = "Pending"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { TransactionGuid = transactionGuid });
        }

        // Endpoint to check the status of a transaction by GUID


        [HttpGet("status/{guid}")]
        public async Task<IActionResult> GetTransactionStatus(Guid guid)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionGuid == guid);

            if (transaction == null)
            {
                return NotFound(_localizer["TransactionNotFound"]);  
            }

            if (transaction.IsCanceled)
            {
                return Ok(new { Status = _localizer["Canceled"] });  
            }

            if (IsHighRisk(transaction))
            {
                transaction.TransactionStatus = "Blocked";
                await _context.SaveChangesAsync();
                return Ok(new { Status = _localizer["Blocked"], Message = _localizer["HighRiskMessage"] });
            }

            if (transaction.TransactionStatus != "Pending")
            {
                return Ok(new { Status = _localizer[transaction.TransactionStatus] });
            }

            var bankStatus = CallBankApiForStatus(transaction);
            transaction.TransactionStatus = bankStatus;
            await _context.SaveChangesAsync();

            return Ok(new { Status = _localizer[transaction.TransactionStatus] });
        }

        // [HttpGet("status/{guid}")]
        private async Task<IActionResult> GetTransactionStatus_old(Guid guid)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionGuid == guid);


            if (transaction == null)
            {
                // Fetch the error message in the requested language
                var errorMessage = _localizer["TransactionNotFound"];
                return NotFound(new { ErrorCode = "E001", Message = errorMessage });
            }


            // if (transaction == null)
            // {
            //     return NotFound("Transaction not found.");
            // }

            // Check if the transaction is already canceled
            if (transaction.IsCanceled)
            {
                return Ok(new { Status = "Canceled" });
            }

            // Check if the transaction is high-risk (credit card owner country vs seller country)
            if (IsHighRisk(transaction))
            {
                transaction.TransactionStatus = "Blocked";
                await _context.SaveChangesAsync();
                return Ok(new { Status = "Blocked", Message = "Transaction blocked due to high risk." });
            }

            // If we already have a status, don't call the bank API again
            if (transaction.TransactionStatus != "Pending")
            {
                return Ok(new { Status = transaction.TransactionStatus });
            }

            // Simulate a call to the bank's API to get the real status
            var bankStatus = CallBankApiForStatus(transaction);

            // Update the transaction status in the database
            transaction.TransactionStatus = bankStatus;
            await _context.SaveChangesAsync();

            return Ok(new { Status = transaction.TransactionStatus });
        }

        // Endpoint to cancel a transaction by GUID
        [HttpPost("cancel/{guid}")]
        public async Task<IActionResult> CancelTransaction(Guid guid)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionGuid == guid);

            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            if (transaction.IsCanceled)
            {
                return BadRequest("Transaction is already canceled.");
            }

            // Simulate a call to the bank's API to confirm cancellation
            var bankResponse = CallBankApiForCancellation(transaction);

            // Update the transaction as canceled
            transaction.IsCanceled = true;
            transaction.TransactionStatus = "Canceled";
            await _context.SaveChangesAsync();

            return Ok(new { Status = "Canceled", BankResponse = bankResponse });
        }

        // Helper function to generate a unique GUID for transactions
        private Guid GenerateTransactionGuid()
        {
            return Guid.NewGuid();
        }

        // Simulated function for checking if the transaction is high-risk
        private bool IsHighRisk(Transaction transaction)
        {
            // For example, if the credit card country is different from the seller's country, it's high risk
            var creditCardCountry = GetCountryByCreditCardNumber(transaction.CreditCardNumber);
            var creditCardCountry2 = GetCountryByIp(transaction.ClientIp);


            if (creditCardCountry != transaction.SellerCountry)
            {
                return true;
            }

            return false;
        }

        // TODO: Implement
        private string GetCountryByCreditCardNumber(string creditCardNumber)
        {
            
            return "US";  
        }

        // TODO: Implement GeoLocationServie, Inject, use it. Check country GeoLocationController for more info.
        private string GetCountryByIp(string ip)
        {
             
            return "US";  
        }

        // Simulated function for calling the bank's API to get the transaction status
        // TODO: Do not forget to remove Random ;) Must implement real bank request and response. Create a new service for BankService        
        private string CallBankApiForStatus(Transaction transaction)
        {
            var statuses = new[] { "Approved", "Declined", "Expired", "InsufficientFunds" };
            return statuses[new Random().Next(statuses.Length)];
        }

        // Simulated function for calling the bank's API to cancel the transaction
        private string CallBankApiForCancellation(Transaction transaction)
        {
            // Dummy implementation - replace with actual API call
            return "Cancellation Confirmed by Bank";
        }
    }
}
