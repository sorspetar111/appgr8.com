using System;
using System.Collections.Generic;

namespace Report.Services
{

    public class ReportBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReportBackgroundService> _logger;

        public ReportBackgroundService(IServiceProvider serviceProvider, ILogger<ReportBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Schedule the service to run at 23:59:59 daily
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                if (now > nextRun)
                {
                    nextRun = nextRun.AddDays(1);
                }

                var delay = nextRun - now;
                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await ProcessDailyReport(stoppingToken);
                }
            }
        }

        private async Task ProcessDailyReport(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();

                var startDate = DateTime.Now.AddDays(-1);
                var endDate = DateTime.Now;

                // Get transactions from the last 24 hours
                var transactions = await dbContext.Transactions
                    .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                    .ToListAsync(stoppingToken);

                var totalTransactions = transactions.Count;
                var canceledTransactions = transactions.Count(t => t.Status == TransactionStatus.Canceled);
                var fraudAttempts = transactions.Count(t => t.Status == TransactionStatus.Fraud);
                var highRiskTransactions = transactions.Count(t => t.IsHighRisk);

                var report = new Report
                {
                    ReportDate = DateTime.Now,
                    TotalTransactions = totalTransactions,
                    CanceledTransactions = canceledTransactions,
                    FraudAttempts = fraudAttempts,
                    HighRiskTransactions = highRiskTransactions
                };

                dbContext.Reports.Add(report);
                await dbContext.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("Daily report processed and saved.");
            }
        }

    }

}