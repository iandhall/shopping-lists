CREATE TABLE [dbo].[PermissionTypes] (
    [Id]              INT            NOT NULL,
    [Description]     NVARCHAR (200) NOT NULL,
    [SelectedDefault] BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

