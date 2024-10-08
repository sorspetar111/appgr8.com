CREATE TABLE TransactionStatusTable (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    TransactionId UNIQUEIDENTIFIER NOT NULL,
    Status NVARCHAR(100) NOT NULL,
    StatusMessage NVARCHAR(500),
    FOREIGN KEY (TransactionId) REFERENCES Transactions(Id)
);