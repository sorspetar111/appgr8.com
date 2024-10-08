CREATE PROCEDURE InitiateTransaction
    @MerchantID INT,
    @Amount DECIMAL(18,2),
    @Currency NVARCHAR(3),
    @BankID INT,
    @TransactionGUID UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    -- Insert transaction with 'Pending' status and generate a GUID
    INSERT INTO Transactions (MerchantID, Amount, Currency, BankID, TransactionStatus)
    OUTPUT INSERTED.TransactionGUID INTO @TransactionGUID
    VALUES (@MerchantID, @Amount, @Currency, @BankID, 'Pending');

    -- Simulate sending request to the bank asynchronously
    -- (In a real scenario, this could be done through a background worker)
END;
GO
