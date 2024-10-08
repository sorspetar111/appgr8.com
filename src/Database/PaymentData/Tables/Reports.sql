CREATE TABLE Reports (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReportDate DATETIME2 NOT NULL,
    TotalTransactions INT NOT NULL,
    CanceledTransactions INT NOT NULL,
    FraudAttempts INT NOT NULL,
    HighRiskTransactions INT NOT NULL
);
