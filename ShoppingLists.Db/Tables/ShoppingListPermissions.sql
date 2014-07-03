CREATE TABLE [dbo].[ShoppingListPermissions] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [PermissionTypeId]     INT            NOT NULL,
    [UserId]           NVARCHAR (128) NOT NULL references Users(Id) on delete cascade,
    [ShoppingListId]   BIGINT         NOT NULL references ShoppingLists(Id) on delete cascade,
    [CreatorId]  NVARCHAR (128) NOT NULL,
    [CreatedDate]      DATETIME2 (7)  NOT NULL,
    [AmenderId] NVARCHAR (128) NULL,
    [AmendedDate]     DATETIME2 (7)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

