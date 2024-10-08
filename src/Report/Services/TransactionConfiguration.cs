using System;
using System.Collections.Generic;

namespace Report.Entity
{
    [Obsolete]
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.Status)
                .IsRequired();

            builder.Property(t => t.IsHighRisk)
                .IsRequired();

            builder.ToTable("Transactions");
        }
    }


}