CREATE DATABASE depoquick;

-- DROP SCHEMA dbo;
--CREATE SCHEMA dbo;
-- depoquick.dbo.Deposits definition

-- Drop table

-- DROP TABLE depoquick.dbo.Deposits;

CREATE TABLE depoquick.dbo.Deposits (
                                        Id int IDENTITY(1,1) NOT NULL,
                                        AirConditioning bit NOT NULL,
                                        Area nvarchar(1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Size] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'' NOT NULL,
    CONSTRAINT PK_Deposits PRIMARY KEY (Id)
    );


-- depoquick.dbo.Promotions definition

-- Drop table

-- DROP TABLE depoquick.dbo.Promotions;

CREATE TABLE depoquick.dbo.Promotions (
                                          Id int IDENTITY(1,1) NOT NULL,
                                          ValidityDate_InitialDate datetime2 NOT NULL,
                                          ValidityDate_FinalDate datetime2 NOT NULL,
                                          Label nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                          DiscountRate float NOT NULL,
                                          CONSTRAINT PK_Promotions PRIMARY KEY (Id)
);


-- depoquick.dbo.Users definition

-- Drop table

-- DROP TABLE depoquick.dbo.Users;

CREATE TABLE depoquick.dbo.Users (
                                     Id int IDENTITY(1,1) NOT NULL,
                                     IsAdministrator bit NOT NULL,
                                     Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                     Email nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                     Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                     CONSTRAINT PK_Users PRIMARY KEY (Id)
);


-- depoquick.dbo.Administrators definition

-- Drop table

-- DROP TABLE depoquick.dbo.Administrators;

CREATE TABLE depoquick.dbo.Administrators (
                                              Id int NOT NULL,
                                              CONSTRAINT PK_Administrators PRIMARY KEY (Id),
                                              CONSTRAINT FK_Administrators_Users_Id FOREIGN KEY (Id) REFERENCES depoquick.dbo.Users(Id)
);


-- depoquick.dbo.Clients definition

-- Drop table

-- DROP TABLE depoquick.dbo.Clients;

CREATE TABLE depoquick.dbo.Clients (
                                       Id int NOT NULL,
                                       CONSTRAINT PK_Clients PRIMARY KEY (Id),
                                       CONSTRAINT FK_Clients_Users_Id FOREIGN KEY (Id) REFERENCES depoquick.dbo.Users(Id)
);


-- depoquick.dbo.DepositPromotion definition

-- Drop table

-- DROP TABLE depoquick.dbo.DepositPromotion;

CREATE TABLE depoquick.dbo.DepositPromotion (
                                                DepositsId int NOT NULL,
                                                PromotionsId int NOT NULL,
                                                CONSTRAINT PK_DepositPromotion PRIMARY KEY (DepositsId,PromotionsId),
                                                CONSTRAINT FK_DepositPromotion_Deposits_DepositsId FOREIGN KEY (DepositsId) REFERENCES depoquick.dbo.Deposits(Id) ON DELETE CASCADE,
                                                CONSTRAINT FK_DepositPromotion_Promotions_PromotionsId FOREIGN KEY (PromotionsId) REFERENCES depoquick.dbo.Promotions(Id) ON DELETE CASCADE
);


-- depoquick.dbo.Deposits_AvailableDates definition

-- Drop table

-- DROP TABLE depoquick.dbo.Deposits_AvailableDates;

CREATE TABLE depoquick.dbo.Deposits_AvailableDates (
                                                       DepositId int NOT NULL,
                                                       Id int IDENTITY(1,1) NOT NULL,
                                                       InitialDate datetime2 NOT NULL,
                                                       FinalDate datetime2 NOT NULL,
                                                       CONSTRAINT PK_Deposits_AvailableDates PRIMARY KEY (DepositId,Id),
                                                       CONSTRAINT FK_Deposits_AvailableDates_Deposits_DepositId FOREIGN KEY (DepositId) REFERENCES depoquick.dbo.Deposits(Id) ON DELETE CASCADE
);


-- depoquick.dbo.LogEntries definition

-- Drop table

-- DROP TABLE depoquick.dbo.LogEntries;

CREATE TABLE depoquick.dbo.LogEntries (
                                          Id int IDENTITY(1,1) NOT NULL,
                                          Message nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Timestamp] datetime2 NOT NULL,
                                          UserId int NOT NULL,
                                          CONSTRAINT PK_LogEntries PRIMARY KEY (Id),
                                          CONSTRAINT FK_LogEntries_Users_UserId FOREIGN KEY (UserId) REFERENCES depoquick.dbo.Users(Id) ON DELETE CASCADE
);

-- depoquick.dbo.Notifications definition

-- Drop table

-- DROP TABLE depoquick.dbo.Notifications;

CREATE TABLE depoquick.dbo.Notifications (
                                             Id int IDENTITY(1,1) NOT NULL,
                                             Message nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Timestamp] datetime2 NOT NULL,
                                             ClientId int NOT NULL,
                                             CONSTRAINT PK_Notifications PRIMARY KEY (Id),
                                             CONSTRAINT FK_Notifications_Clients_ClientId FOREIGN KEY (ClientId) REFERENCES depoquick.dbo.Clients(Id) ON DELETE CASCADE
);


-- depoquick.dbo.Reservations definition

-- Drop table

-- DROP TABLE depoquick.dbo.Reservations;

CREATE TABLE depoquick.dbo.Reservations (
                                            Id int IDENTITY(1,1) NOT NULL,
                                            DepositId int NOT NULL,
                                            ClientId int NOT NULL,
                                            Date_InitialDate datetime2 NOT NULL,
                                            Date_FinalDate datetime2 NOT NULL,
                                            Status int NOT NULL,
                                            Message nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                            Price int DEFAULT 0 NOT NULL,
                                            RequestedAt datetime2 DEFAULT '0001-01-01T00:00:00.0000000' NOT NULL,
                                            CONSTRAINT PK_Reservations PRIMARY KEY (Id),
                                            CONSTRAINT FK_Reservations_Clients_ClientId FOREIGN KEY (ClientId) REFERENCES depoquick.dbo.Clients(Id) ON DELETE CASCADE,
                                            CONSTRAINT FK_Reservations_Deposits_DepositId FOREIGN KEY (DepositId) REFERENCES depoquick.dbo.Deposits(Id) ON DELETE CASCADE
);


-- depoquick.dbo.Payments definition

-- Drop table

-- DROP TABLE depoquick.dbo.Payments;

CREATE TABLE depoquick.dbo.Payments (
                                        Id int IDENTITY(1,1) NOT NULL,
                                        Status nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                        ReservationId int NOT NULL,
                                        CONSTRAINT PK_Payments PRIMARY KEY (Id),
                                        CONSTRAINT FK_Payments_Reservations_ReservationId FOREIGN KEY (ReservationId) REFERENCES depoquick.dbo.Reservations(Id) ON DELETE CASCADE
);


-- depoquick.dbo.Ratings definition

-- Drop table

-- DROP TABLE depoquick.dbo.Ratings;

CREATE TABLE depoquick.dbo.Ratings (
                                       Id int IDENTITY(1,1) NOT NULL,
                                       Stars int NOT NULL,
                                       Comment nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                       DepositId int NULL,
                                       ReservationId int DEFAULT 0 NOT NULL,
                                       CONSTRAINT PK_Ratings PRIMARY KEY (Id),
                                       CONSTRAINT FK_Ratings_Deposits_DepositId FOREIGN KEY (DepositId) REFERENCES depoquick.dbo.Deposits(Id),
                                       CONSTRAINT FK_Ratings_Reservations_ReservationId FOREIGN KEY (ReservationId) REFERENCES depoquick.dbo.Reservations(Id) ON DELETE CASCADE
);
