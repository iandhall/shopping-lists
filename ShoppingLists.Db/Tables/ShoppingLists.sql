CREATE TABLE [dbo].[ShoppingLists] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (MAX) NOT NULL,
    [CreatorId]  NVARCHAR (128) NOT NULL,
    [CreatedDate]      DATETIME2 (7)  NOT NULL,
    [AmenderId] NVARCHAR (128) NULL,
    [AmendedDate]     DATETIME2 (7)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

