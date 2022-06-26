
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/19/2022 20:05:19
-- Generated from EDMX file: C:\Users\user1\Desktop\gooood\פרויקט גמר- עורך קניות\Shopper\Dal\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Shopper_DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ActuallyPurchase_Tbl_Product_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ActuallyPurchase_Tbl] DROP CONSTRAINT [FK_ActuallyPurchase_Tbl_Product_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_ActuallyPurchase_Tbl_PurchasePrognosis_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ActuallyPurchase_Tbl] DROP CONSTRAINT [FK_ActuallyPurchase_Tbl_PurchasePrognosis_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_ActuallyPurchase_Tbl_PurchasesHistory_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ActuallyPurchase_Tbl] DROP CONSTRAINT [FK_ActuallyPurchase_Tbl_PurchasesHistory_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_BehaviourPattern_Tbl_Customer_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BehaviourPattern_Tbl] DROP CONSTRAINT [FK_BehaviourPattern_Tbl_Customer_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_BehaviourPattern_Tbl_Product_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BehaviourPattern_Tbl] DROP CONSTRAINT [FK_BehaviourPattern_Tbl_Product_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_ForeignDate_Tbl_Timing_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ForeignDate_Tbl] DROP CONSTRAINT [FK_ForeignDate_Tbl_Timing_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_Table_ProductCategory_Table]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product_Tbl] DROP CONSTRAINT [FK_Product_Table_ProductCategory_Table];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductCategory_Table_ProductCategory_Table]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductCategory_Tbl] DROP CONSTRAINT [FK_ProductCategory_Table_ProductCategory_Table];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchasePrognosis_Tbl_Product_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchasePrognosis_Tbl] DROP CONSTRAINT [FK_PurchasePrognosis_Tbl_Product_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchasePrognosis_Tbl_PurchasesHistory_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchasePrognosis_Tbl] DROP CONSTRAINT [FK_PurchasePrognosis_Tbl_PurchasesHistory_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchasesHistory_Tbl_Customer_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchasesHistory_Tbl] DROP CONSTRAINT [FK_PurchasesHistory_Tbl_Customer_Tbl];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchasesHistory_Tbl_Timing_Tbl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchasesHistory_Tbl] DROP CONSTRAINT [FK_PurchasesHistory_Tbl_Timing_Tbl];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ActuallyPurchase_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActuallyPurchase_Tbl];
GO
IF OBJECT_ID(N'[dbo].[BehaviourPattern_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BehaviourPattern_Tbl];
GO
IF OBJECT_ID(N'[dbo].[Customer_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer_Tbl];
GO
IF OBJECT_ID(N'[dbo].[ForeignDate_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ForeignDate_Tbl];
GO
IF OBJECT_ID(N'[dbo].[HebrewDate_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HebrewDate_Tbl];
GO
IF OBJECT_ID(N'[dbo].[Product_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product_Tbl];
GO
IF OBJECT_ID(N'[dbo].[ProductCategory_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductCategory_Tbl];
GO
IF OBJECT_ID(N'[dbo].[PurchasePrognosis_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchasePrognosis_Tbl];
GO
IF OBJECT_ID(N'[dbo].[PurchasesHistory_Tbl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchasesHistory_Tbl];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ActuallyPurchase_Tbl'
CREATE TABLE [dbo].[ActuallyPurchase_Tbl] (
    [ActuallyPurchaseId] int IDENTITY(1,1) NOT NULL,
    [PurchasesHistoryId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Amount] int  NOT NULL,
    [PurchasePrognosisId] int  NULL
);
GO

-- Creating table 'BehaviourPattern_Tbl'
CREATE TABLE [dbo].[BehaviourPattern_Tbl] (
    [BehaviourPatternId] int IDENTITY(1,1) NOT NULL,
    [Status] int  NOT NULL,
    [EveryXMonthsBuy] int  NULL,
    [Amount] int  NULL,
    [ProductId] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [NameOfEvent] nvarchar(50)  NULL,
    [IsNewProduct] bit  NULL,
    [TimesOfCancelation] int  NULL,
    [TimesOfAmountChange] int  NULL,
    [TimesNotFoundInPrognosis] int  NULL,
    [XTimesOnMonthBuy] int  NULL,
    [LastPurchaseDate] datetime  NULL
);
GO

-- Creating table 'Customer_Tbl'
CREATE TABLE [dbo].[Customer_Tbl] (
    [CustomerId] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(10)  NULL,
    [LastName] nvarchar(10)  NULL,
    [Password] nchar(10)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'ForeignDate_Tbl'
CREATE TABLE [dbo].[ForeignDate_Tbl] (
    [ForeignDateId] int IDENTITY(1,1) NOT NULL,
    [ForeignDate] datetime  NOT NULL,
    [HebrewDateId] int  NOT NULL
);
GO

-- Creating table 'Product_Tbl'
CREATE TABLE [dbo].[Product_Tbl] (
    [ProductId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [ProductCategoryId] int  NOT NULL,
    [Description] nvarchar(50)  NULL,
    [Image] nvarchar(50)  NULL
);
GO

-- Creating table 'ProductCategory_Tbl'
CREATE TABLE [dbo].[ProductCategory_Tbl] (
    [ProductCategoryId] int IDENTITY(1,1) NOT NULL,
    [ParentProductCategoryId] int  NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'PurchasePrognosis_Tbl'
CREATE TABLE [dbo].[PurchasePrognosis_Tbl] (
    [PurchasePrognosisId] int IDENTITY(1,1) NOT NULL,
    [PurchasesHistoryId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Amount] int  NOT NULL
);
GO

-- Creating table 'PurchasesHistory_Tbl'
CREATE TABLE [dbo].[PurchasesHistory_Tbl] (
    [PurchasesHistoryId] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [PurchaseDate] datetime  NULL,
    [Description] nvarchar(50)  NULL,
    [BelongToCurrentYear] bit  NULL,
    [HebrewDateId] int  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'HebrewDate_Tbl'
CREATE TABLE [dbo].[HebrewDate_Tbl] (
    [HebrewDateId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Day] int  NOT NULL,
    [Month] int  NOT NULL,
    [Year] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ActuallyPurchaseId] in table 'ActuallyPurchase_Tbl'
ALTER TABLE [dbo].[ActuallyPurchase_Tbl]
ADD CONSTRAINT [PK_ActuallyPurchase_Tbl]
    PRIMARY KEY CLUSTERED ([ActuallyPurchaseId] ASC);
GO

-- Creating primary key on [BehaviourPatternId] in table 'BehaviourPattern_Tbl'
ALTER TABLE [dbo].[BehaviourPattern_Tbl]
ADD CONSTRAINT [PK_BehaviourPattern_Tbl]
    PRIMARY KEY CLUSTERED ([BehaviourPatternId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'Customer_Tbl'
ALTER TABLE [dbo].[Customer_Tbl]
ADD CONSTRAINT [PK_Customer_Tbl]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [ForeignDateId] in table 'ForeignDate_Tbl'
ALTER TABLE [dbo].[ForeignDate_Tbl]
ADD CONSTRAINT [PK_ForeignDate_Tbl]
    PRIMARY KEY CLUSTERED ([ForeignDateId] ASC);
GO

-- Creating primary key on [ProductId] in table 'Product_Tbl'
ALTER TABLE [dbo].[Product_Tbl]
ADD CONSTRAINT [PK_Product_Tbl]
    PRIMARY KEY CLUSTERED ([ProductId] ASC);
GO

-- Creating primary key on [ProductCategoryId] in table 'ProductCategory_Tbl'
ALTER TABLE [dbo].[ProductCategory_Tbl]
ADD CONSTRAINT [PK_ProductCategory_Tbl]
    PRIMARY KEY CLUSTERED ([ProductCategoryId] ASC);
GO

-- Creating primary key on [PurchasePrognosisId] in table 'PurchasePrognosis_Tbl'
ALTER TABLE [dbo].[PurchasePrognosis_Tbl]
ADD CONSTRAINT [PK_PurchasePrognosis_Tbl]
    PRIMARY KEY CLUSTERED ([PurchasePrognosisId] ASC);
GO

-- Creating primary key on [PurchasesHistoryId] in table 'PurchasesHistory_Tbl'
ALTER TABLE [dbo].[PurchasesHistory_Tbl]
ADD CONSTRAINT [PK_PurchasesHistory_Tbl]
    PRIMARY KEY CLUSTERED ([PurchasesHistoryId] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [HebrewDateId] in table 'HebrewDate_Tbl'
ALTER TABLE [dbo].[HebrewDate_Tbl]
ADD CONSTRAINT [PK_HebrewDate_Tbl]
    PRIMARY KEY CLUSTERED ([HebrewDateId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ProductId] in table 'ActuallyPurchase_Tbl'
ALTER TABLE [dbo].[ActuallyPurchase_Tbl]
ADD CONSTRAINT [FK_ActuallyPurchase_Table_Product_Table]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product_Tbl]
        ([ProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActuallyPurchase_Table_Product_Table'
CREATE INDEX [IX_FK_ActuallyPurchase_Table_Product_Table]
ON [dbo].[ActuallyPurchase_Tbl]
    ([ProductId]);
GO

-- Creating foreign key on [PurchasePrognosisId] in table 'ActuallyPurchase_Tbl'
ALTER TABLE [dbo].[ActuallyPurchase_Tbl]
ADD CONSTRAINT [FK_ActuallyPurchase_Table_PurchasePrognosis_Table]
    FOREIGN KEY ([PurchasePrognosisId])
    REFERENCES [dbo].[PurchasePrognosis_Tbl]
        ([PurchasePrognosisId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActuallyPurchase_Table_PurchasePrognosis_Table'
CREATE INDEX [IX_FK_ActuallyPurchase_Table_PurchasePrognosis_Table]
ON [dbo].[ActuallyPurchase_Tbl]
    ([PurchasePrognosisId]);
GO

-- Creating foreign key on [PurchasesHistoryId] in table 'ActuallyPurchase_Tbl'
ALTER TABLE [dbo].[ActuallyPurchase_Tbl]
ADD CONSTRAINT [FK_ActuallyPurchase_Table_PurchasesHistory_Table]
    FOREIGN KEY ([PurchasesHistoryId])
    REFERENCES [dbo].[PurchasesHistory_Tbl]
        ([PurchasesHistoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActuallyPurchase_Table_PurchasesHistory_Table'
CREATE INDEX [IX_FK_ActuallyPurchase_Table_PurchasesHistory_Table]
ON [dbo].[ActuallyPurchase_Tbl]
    ([PurchasesHistoryId]);
GO

-- Creating foreign key on [ProductId] in table 'BehaviourPattern_Tbl'
ALTER TABLE [dbo].[BehaviourPattern_Tbl]
ADD CONSTRAINT [FK_BehaviourPattern_Table_Product_Table]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product_Tbl]
        ([ProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BehaviourPattern_Table_Product_Table'
CREATE INDEX [IX_FK_BehaviourPattern_Table_Product_Table]
ON [dbo].[BehaviourPattern_Tbl]
    ([ProductId]);
GO

-- Creating foreign key on [CustomerId] in table 'BehaviourPattern_Tbl'
ALTER TABLE [dbo].[BehaviourPattern_Tbl]
ADD CONSTRAINT [FK_BehaviourPattern_Tbl_Customer_Tbl]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customer_Tbl]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BehaviourPattern_Tbl_Customer_Tbl'
CREATE INDEX [IX_FK_BehaviourPattern_Tbl_Customer_Tbl]
ON [dbo].[BehaviourPattern_Tbl]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'PurchasesHistory_Tbl'
ALTER TABLE [dbo].[PurchasesHistory_Tbl]
ADD CONSTRAINT [FK_PurchasesHistory_Table_Customer_Table]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customer_Tbl]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchasesHistory_Table_Customer_Table'
CREATE INDEX [IX_FK_PurchasesHistory_Table_Customer_Table]
ON [dbo].[PurchasesHistory_Tbl]
    ([CustomerId]);
GO

-- Creating foreign key on [ProductCategoryId] in table 'Product_Tbl'
ALTER TABLE [dbo].[Product_Tbl]
ADD CONSTRAINT [FK_Product_Table_ProductCategory_Table]
    FOREIGN KEY ([ProductCategoryId])
    REFERENCES [dbo].[ProductCategory_Tbl]
        ([ProductCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_Table_ProductCategory_Table'
CREATE INDEX [IX_FK_Product_Table_ProductCategory_Table]
ON [dbo].[Product_Tbl]
    ([ProductCategoryId]);
GO

-- Creating foreign key on [ProductId] in table 'PurchasePrognosis_Tbl'
ALTER TABLE [dbo].[PurchasePrognosis_Tbl]
ADD CONSTRAINT [FK_PurchasePrognosis_Table_Product_Table]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product_Tbl]
        ([ProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchasePrognosis_Table_Product_Table'
CREATE INDEX [IX_FK_PurchasePrognosis_Table_Product_Table]
ON [dbo].[PurchasePrognosis_Tbl]
    ([ProductId]);
GO

-- Creating foreign key on [ParentProductCategoryId] in table 'ProductCategory_Tbl'
ALTER TABLE [dbo].[ProductCategory_Tbl]
ADD CONSTRAINT [FK_ProductCategory_Table_ProductCategory_Table]
    FOREIGN KEY ([ParentProductCategoryId])
    REFERENCES [dbo].[ProductCategory_Tbl]
        ([ProductCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductCategory_Table_ProductCategory_Table'
CREATE INDEX [IX_FK_ProductCategory_Table_ProductCategory_Table]
ON [dbo].[ProductCategory_Tbl]
    ([ParentProductCategoryId]);
GO

-- Creating foreign key on [PurchasesHistoryId] in table 'PurchasePrognosis_Tbl'
ALTER TABLE [dbo].[PurchasePrognosis_Tbl]
ADD CONSTRAINT [FK_PurchasePrognosis_Table_PurchasesHistory_Table]
    FOREIGN KEY ([PurchasesHistoryId])
    REFERENCES [dbo].[PurchasesHistory_Tbl]
        ([PurchasesHistoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchasePrognosis_Table_PurchasesHistory_Table'
CREATE INDEX [IX_FK_PurchasePrognosis_Table_PurchasesHistory_Table]
ON [dbo].[PurchasePrognosis_Tbl]
    ([PurchasesHistoryId]);
GO

-- Creating foreign key on [CustomerId] in table 'BehaviourPattern_Tbl'
ALTER TABLE [dbo].[BehaviourPattern_Tbl]
ADD CONSTRAINT [FK_BehaviourPattern_Tbl_Product_Tbl]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Product_Tbl]
        ([ProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BehaviourPattern_Tbl_Product_Tbl'
CREATE INDEX [IX_FK_BehaviourPattern_Tbl_Product_Tbl]
ON [dbo].[BehaviourPattern_Tbl]
    ([CustomerId]);
GO

-- Creating foreign key on [HebrewDateId] in table 'ForeignDate_Tbl'
ALTER TABLE [dbo].[ForeignDate_Tbl]
ADD CONSTRAINT [FK_ForeignDate_Tbl_Timing_Tbl]
    FOREIGN KEY ([HebrewDateId])
    REFERENCES [dbo].[HebrewDate_Tbl]
        ([HebrewDateId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ForeignDate_Tbl_Timing_Tbl'
CREATE INDEX [IX_FK_ForeignDate_Tbl_Timing_Tbl]
ON [dbo].[ForeignDate_Tbl]
    ([HebrewDateId]);
GO

-- Creating foreign key on [HebrewDateId] in table 'PurchasesHistory_Tbl'
ALTER TABLE [dbo].[PurchasesHistory_Tbl]
ADD CONSTRAINT [FK_PurchasesHistory_Tbl_Timing_Tbl]
    FOREIGN KEY ([HebrewDateId])
    REFERENCES [dbo].[HebrewDate_Tbl]
        ([HebrewDateId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchasesHistory_Tbl_Timing_Tbl'
CREATE INDEX [IX_FK_PurchasesHistory_Tbl_Timing_Tbl]
ON [dbo].[PurchasesHistory_Tbl]
    ([HebrewDateId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------