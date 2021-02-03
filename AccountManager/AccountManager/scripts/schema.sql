CREATE TABLE IF NOT EXISTS Accounts (
    AccountId INT NOT NULL PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS MeterReadings (
    ReadingId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    AccountId INT NOT NULL,
    ReadingDatetime DATETIME NOT NULL,
    ReadingValue INT NOT NULL,
    CONSTRAINT FK_AccountMeterReading FOREIGN KEY (AccountId)
    REFERENCES Accounts(AccountId)
);
