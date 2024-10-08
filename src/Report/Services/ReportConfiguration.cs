using System;
using System.Collections.Generic;

namespace Report.Entity
{

    [Obsolete]
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReportDate)
                .IsRequired();

            builder.Property(r => r.TotalTransactions)
                .IsRequired();

            builder.Property(r => r.CanceledTransactions)
                .IsRequired();

            builder.Property(r => r.FraudAttempts)
                .IsRequired();

            builder.Property(r => r.HighRiskTransactions)
                .IsRequired();

            builder.ToTable("Reports");
        }
    }

}