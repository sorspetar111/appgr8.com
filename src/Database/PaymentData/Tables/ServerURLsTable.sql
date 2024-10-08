 
CREATE TABLE ServerURLsTable (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    CountryId UNIQUEIDENTIFIER NOT NULL,
    URL NVARCHAR(500) NOT NULL,
    FOREIGN KEY (CountryId) REFERENCES Countries(Id)
);



-- ServerUrls Table
-- ServerUrlId (PK)	CountryCode (FK)	ServerUrl
-- 1	US	https://us.paymentgateway.com
-- 2	CA	https://ca.paymentgateway.com
-- 3	GB	https://uk.paymentgateway.com
-- 4	AU	https://au.paymentgateway.com