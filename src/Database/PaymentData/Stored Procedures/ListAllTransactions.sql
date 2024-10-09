SELECT 
    t.Id AS TransactionId,
    t.MerchantId,
    t.BankId,
    t.UserId,
    t.PaymentId,
    t.CreditCardNumber,
    t.Amount,
    t.Currency,
    t.ClientIp,
    t.AccountCurrency,
    t.Status AS CurrentTransactionStatus, 
    t.CreatedAt,
    t.UpdatedAt,
    t.IsHighRisk,
    ts.Status AS LatestTransactionStatus,
    ts.StatusMessage
FROM 
    Transactions t
JOIN 
    TransactionStatusTable ts 
    ON t.Id = ts.TransactionId
JOIN 
    (SELECT TransactionId, MAX(Id) AS MaxStatusId
     FROM TransactionStatusTable
     GROUP BY TransactionId) latestStatus
    ON ts.TransactionId = latestStatus.TransactionId
    AND ts.Id = latestStatus.MaxStatusId;
