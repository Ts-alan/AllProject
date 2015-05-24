
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/02/2014 19:00:59
-- Generated from EDMX file: D:\dev\trunk\DAL\DataModels\CCPDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CCPDb-test];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_dbo_Contracts_dbo_Customers_Customer_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contract] DROP CONSTRAINT [FK_dbo_Contracts_dbo_Customers_Customer_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Contracts_dbo_EndUsers_EndUser_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contract] DROP CONSTRAINT [FK_dbo_Contracts_dbo_EndUsers_EndUser_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Contracts_dbo_SalesPersons_SalesPerson_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contract] DROP CONSTRAINT [FK_dbo_Contracts_dbo_SalesPersons_SalesPerson_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesPerson_User1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesPerson] DROP CONSTRAINT [FK_SalesPerson_User1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Contract]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contract];
GO
IF OBJECT_ID(N'[dbo].[Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer];
GO
IF OBJECT_ID(N'[dbo].[EndUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EndUser];
GO
IF OBJECT_ID(N'[dbo].[SalesPerson]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesPerson];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Contracts'
CREATE TABLE [dbo].[Contracts] (
    [ContractId] bigint IDENTITY(1,1) NOT NULL,
    [CPR] int  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [Status] nvarchar(max)  NULL,
    [Customer_CustomerId] bigint  NULL,
    [EndUser_EndUserId] bigint  NULL,
    [SalesPerson_SalesPersonId] bigint  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [CustomerId] bigint IDENTITY(1,1) NOT NULL,
    [CustomerName] nvarchar(max)  NULL
);
GO

-- Creating table 'EndUsers'
CREATE TABLE [dbo].[EndUsers] (
    [EndUserId] bigint IDENTITY(1,1) NOT NULL,
    [EndUserName] nvarchar(max)  NULL
);
GO

-- Creating table 'SalesPersons'
CREATE TABLE [dbo].[SalesPersons] (
    [SalesPersonId] bigint  NOT NULL,
    [SalesPersonName] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Region] nvarchar(max)  NULL,
    [SalesPersonNumber] bigint  NOT NULL,
    [Status] nvarchar(max)  NULL,
    [ContractPrefix] nvarchar(max)  NULL,
    [User_UserId] bigint  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] bigint  NOT NULL,
    [FirstName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Role] nvarchar(max)  NULL,
    [PasswordHash] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ContractId] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [PK_Contracts]
    PRIMARY KEY CLUSTERED ([ContractId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [EndUserId] in table 'EndUsers'
ALTER TABLE [dbo].[EndUsers]
ADD CONSTRAINT [PK_EndUsers]
    PRIMARY KEY CLUSTERED ([EndUserId] ASC);
GO

-- Creating primary key on [SalesPersonId] in table 'SalesPersons'
ALTER TABLE [dbo].[SalesPersons]
ADD CONSTRAINT [PK_SalesPersons]
    PRIMARY KEY CLUSTERED ([SalesPersonId] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Customer_CustomerId] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [FK_dbo_Contracts_dbo_Customers_Customer_Id]
    FOREIGN KEY ([Customer_CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Contracts_dbo_Customers_Customer_Id'
CREATE INDEX [IX_FK_dbo_Contracts_dbo_Customers_Customer_Id]
ON [dbo].[Contracts]
    ([Customer_CustomerId]);
GO

-- Creating foreign key on [EndUser_EndUserId] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [FK_dbo_Contracts_dbo_EndUsers_EndUser_Id]
    FOREIGN KEY ([EndUser_EndUserId])
    REFERENCES [dbo].[EndUsers]
        ([EndUserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Contracts_dbo_EndUsers_EndUser_Id'
CREATE INDEX [IX_FK_dbo_Contracts_dbo_EndUsers_EndUser_Id]
ON [dbo].[Contracts]
    ([EndUser_EndUserId]);
GO

-- Creating foreign key on [SalesPerson_SalesPersonId] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [FK_dbo_Contracts_dbo_SalesPersons_SalesPerson_Id]
    FOREIGN KEY ([SalesPerson_SalesPersonId])
    REFERENCES [dbo].[SalesPersons]
        ([SalesPersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Contracts_dbo_SalesPersons_SalesPerson_Id'
CREATE INDEX [IX_FK_dbo_Contracts_dbo_SalesPersons_SalesPerson_Id]
ON [dbo].[Contracts]
    ([SalesPerson_SalesPersonId]);
GO

-- Creating foreign key on [User_UserId] in table 'SalesPersons'
ALTER TABLE [dbo].[SalesPersons]
ADD CONSTRAINT [FK_SalesPerson_User1]
    FOREIGN KEY ([User_UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesPerson_User1'
CREATE INDEX [IX_FK_SalesPerson_User1]
ON [dbo].[SalesPersons]
    ([User_UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------