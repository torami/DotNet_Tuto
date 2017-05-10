CREATE TABLE [dbo].[Users]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY (0,1),
    [Email] NVARCHAR(50) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Country] nvarchar (50) NOT NULL
)