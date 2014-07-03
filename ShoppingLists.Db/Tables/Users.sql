CREATE TABLE [dbo].[Users] (
    [Id]            NVARCHAR (128) NOT NULL,
    [Username]      NVARCHAR (MAX) COLLATE Latin1_General_CS_AS NULL,
    [PasswordHash]  NVARCHAR (MAX) NULL,
    [SecurityStamp] NVARCHAR (MAX) NULL,
    [Discriminator] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);
