CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [username] NVARCHAR(30) NOT NULL, 
	[email] NVARCHAR(50) NOT NULL,
    [password] NVARCHAR(30) NOT NULL
)

CREATE TABLE [dbo].[Questions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Question] NTEXT NOT NULL
)

CREATE TABLE [dbo].[AnsweredQuestions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[userId] INT NOT NULL,
	[questionId] INT NOT NULL
)

CREATE TABLE [dbo].[Score]
(
    [userId] INT NOT NULL, 
    [stars] INT NOT NULL DEFAULT 0, 
    [points] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_UserId] FOREIGN KEY (userId) REFERENCES [Users]([Id]), 
    CONSTRAINT [PK_Score] PRIMARY KEY ([userId])
)
