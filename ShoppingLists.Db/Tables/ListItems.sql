CREATE TABLE [dbo].[ListItems] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Description]      NVARCHAR (MAX) NOT NULL,
    [ShoppingListId]   BIGINT         NOT NULL references ShoppingLists(Id) on delete cascade,
    [StatusId]         SMALLINT       NOT NULL,
    [CreatorId]  NVARCHAR (128) NOT NULL,
    [CreatedDate]      DATETIME2 (7)  NOT NULL,
    [AmenderId] NVARCHAR (128) NULL,
    [AmendedDate]     DATETIME2 (7)  NULL,
    [Quantity]         INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

