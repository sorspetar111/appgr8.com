using Microsoft.EntityFrameworkCore;
using PaymentGateway.Models;

namespace PaymentGateway.Data
{
    public class PaymentGatewayDbContext : DbContext
    {
        public PaymentGatewayDbContext(DbContextOptions<PaymentGatewayDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MerchantPayment> MerchantPayments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionStatus> TransactionStatusTable { get; set; }
        public DbSet<ServerURL> ServerURLs { get; set; }
        public DbSet<RiskManagement> RiskManagements { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

           
            modelBuilder.Entity<Merchant>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Merchant>()
                .HasMany(m => m.Transactions)
                .WithOne(t => t.Merchant);

            modelBuilder.Entity<Merchant>()
                .HasMany(m => m.MerchantPayments)
                .WithOne(mp => mp.Merchant);

            
            modelBuilder.Entity<Bank>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Bank>()
                .HasMany(b => b.Transactions)
                .WithOne(t => t.Bank);

         
            modelBuilder.Entity<Country>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Merchants)
                .WithOne(m => m.Country);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Banks)
                .WithOne(b => b.Country);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.ServerURLs)
                .WithOne(s => s.Country);

            
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Payment>()
                .HasMany(p => p.MerchantPayments)
                .WithOne(mp => mp.Payment);

          
            modelBuilder.Entity<MerchantPayment>()
                .HasKey(mp => new { mp.MerchantId, mp.PaymentId });

          
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Transaction>()
                .HasMany(t => t.TransactionStatuses)
                .WithOne(ts => ts.Transaction);

            modelBuilder.Entity<Transaction>()
                .HasMany(t => t.RiskManagements)
                .WithOne(rm => rm.Transaction);

            modelBuilder.Entity<Transaction>()
                .HasMany(t => t.AuditLogs)
                .WithOne(al => al.Transaction);

            
            modelBuilder.Entity<TransactionStatus>()
                .HasKey(ts => ts.Id);

       
            modelBuilder.Entity<ServerURL>()
                .HasKey(s => s.Id);

           
            modelBuilder.Entity<RiskManagement>()
                .HasKey(rm => rm.Id);

          
            modelBuilder.Entity<Report>()
                .HasKey(r => r.Id);

            
            modelBuilder.Entity<AuditLog>()
                .HasKey(al => al.Id);
        }
    }
}
