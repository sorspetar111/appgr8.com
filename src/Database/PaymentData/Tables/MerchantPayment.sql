CREATE TABLE MerchantPayment (
    MerchantId UNIQUEIDENTIFIER NOT NULL,
    PaymentId UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (MerchantId, PaymentId),
    FOREIGN KEY (MerchantId) REFERENCES Merchants(Id),
    FOREIGN KEY (PaymentId) REFERENCES Payment(Id)
);
