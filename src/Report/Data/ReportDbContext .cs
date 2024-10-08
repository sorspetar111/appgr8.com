using Microsoft.EntityFrameworkCore;
using Report.Models;
using System;

namespace Report.Data
{
    public class ReportDbContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             
            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Reports"); 
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                    .ValueGeneratedOnAdd(); 

                entity.Property(r => r.ReportDate)
                    .IsRequired(); 

                entity.Property(r => r.TotalTransactions)
                    .IsRequired(); 

                entity.Property(r => r.CanceledTransactions)
                    .IsRequired(); 

                entity.Property(r => r.FraudAttempts)
                    .IsRequired(); 

                entity.Property(r => r.HighRiskTransactions)
                    .IsRequired(); 
            });

            
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions"); 
                entity.HasKey(t => t.Id); 

                entity.Property(t => t.Id)
                    .HasDefaultValueSql("NEWID()") 
                    .IsRequired(); 

                entity.Property(t => t.CreatedAt)
                    .IsRequired(); 

                entity.Property(t => t.Status)
                    .IsRequired(); 

                entity.Property(t => t.IsHighRisk)
                    .IsRequired(); 
            });
        }
    }
}
