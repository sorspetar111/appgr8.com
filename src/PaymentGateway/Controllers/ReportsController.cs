// Controllers/ReportsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly PaymentGatewayDbContext _context;

        public ReportsController(PaymentGatewayDbContext context)
        {
            _context = context;
        }

        // Endpoint to get total transactions for a specific month
        [HttpGet("monthly/{year}/{month}")]
        public async Task<IActionResult> GetMonthlyTransactions(int year, int month)
        {
            var transactions = await _context.Transactions
                .Where(t => t.TransactionDate.Year == year && t.TransactionDate.Month == month)
                .ToListAsync();

            var report = new
            {
                TotalTransactions = transactions.Count,
                SuccessfulTransactions = transactions.Count(t => t.TransactionStatus == "Approved"),
                FailedTransactions = transactions.Count(t => t.TransactionStatus == "Declined"),
                CanceledTransactions = transactions.Count(t => t.IsCanceled)
            };

            return Ok(report);
        }

        // Endpoint to get a summary report for a specified date range
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummaryReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var transactions = await _context.Transactions
                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .ToListAsync();

            var report = new
            {
                TotalTransactions = transactions.Count,
                SuccessfulTransactions = transactions.Count(t => t.TransactionStatus == "Approved"),
                FailedTransactions = transactions.Count(t => t.TransactionStatus == "Declined"),
                CanceledTransactions = transactions.Count(t => t.IsCanceled),
                TotalRevenue = transactions.Where(t => t.TransactionStatus == "Approved").Sum(t => t.Amount)
            };

            return Ok(report);
        }

        // Endpoint to get a detailed report of all transactions within a specified date range
        [HttpGet("detailed")]
        public async Task<IActionResult> GetDetailedReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var transactions = await _context.Transactions
                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .Select(t => new
                {
                    t.TransactionGuid,
                    t.CreditCardNumber,
                    t.Amount,
                    t.TransactionStatus,
                    t.TransactionDate
                })
                .ToListAsync();

            return Ok(transactions);
        }

        // Endpoint to get high-risk transactions
        [HttpGet("high-risk")]
        public async Task<IActionResult> GetHighRiskTransactions([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var highRiskTransactions = await _context.Transactions
                .Where(t => t.RiskLevel == "High" && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .Select(t => new
                {
                    t.TransactionGuid,
                    t.CreditCardNumber,
                    t.Amount,
                    t.TransactionStatus,
                    t.TransactionDate,
                    t.RiskReason
                })
                .ToListAsync();

            return Ok(highRiskTransactions);
        }

        // Endpoint to get canceled transactions
        [HttpGet("canceled")]
        public async Task<IActionResult> GetCanceledTransactions([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var canceledTransactions = await _context.Transactions
                .Where(t => t.IsCanceled && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .Select(t => new
                {
                    t.TransactionGuid,
                    t.CreditCardNumber,
                    t.Amount,
                    t.TransactionStatus,
                    t.TransactionDate
                })
                .ToListAsync();

            return Ok(canceledTransactions);
        }

        // Endpoint to get fraud attempts transactions
        [HttpGet("fraud-attempts")]
        public async Task<IActionResult> GetFraudAttemptTransactions([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var fraudTransactions = await _context.Transactions
                .Where(t => t.IsFraudulent && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .Select(t => new
                {
                    t.TransactionGuid,
                    t.CreditCardNumber,
                    t.Amount,
                    t.TransactionStatus,
                    t.TransactionDate,
                    t.FraudReason
                })
                .ToListAsync();

            return Ok(fraudTransactions);
        }
    }
}
