CREATE TABLE ErrorMessages (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ErrorCode NVARCHAR(50) NOT NULL,    -- Unique error code
    DefaultMessage NVARCHAR(500) NOT NULL -- Default message in English
);

CREATE TABLE ErrorTranslations (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ErrorMessageId UNIQUEIDENTIFIER NOT NULL, -- Foreign key from ErrorMessages
    LanguageCode NVARCHAR(10) NOT NULL, -- e.g., 'en', 'fr', 'es', etc.
    TranslatedMessage NVARCHAR(500) NOT NULL, -- The translated message in the specified language
    FOREIGN KEY (ErrorMessageId) REFERENCES ErrorMessages(Id)
);


/*

Example Data:
ErrorMessageId	ErrorCode	DefaultMessage
1A2B3C	E001	"Transaction not found."
2A3B4C	E002	"Insufficient funds."

ErrorTranslationId	ErrorMessageId	LanguageCode	TranslatedMessage
1X2Y3Z	1A2B3C	"fr"	"Transaction introuvable."
1X2Y4Z	1A2B3C	"es"	"Transacción no encontrada."
2X3Y4Z	2A3B4C	"fr"	"Fonds insuffisants."

*/