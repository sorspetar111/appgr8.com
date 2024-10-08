CREATE PROCEDURE CheckTransactionStatus
    @TransactionGUID UNIQUEIDENTIFIER,
    @TransactionStatus NVARCHAR(50) OUTPUT
AS
BEGIN
    -- Query the current status of the transaction
    SELECT @TransactionStatus = TransactionStatus
    FROM Transactions
    WHERE TransactionGUID = @TransactionGUID;
    
    -- If no result found, set status to 'Not Found'
    IF @TransactionStatus IS NULL
    BEGIN
        SET @TransactionStatus = 'Transaction Not Found';
    END;
END;
GO


