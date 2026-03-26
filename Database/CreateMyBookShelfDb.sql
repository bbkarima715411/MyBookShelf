-- Crée la base de données si elle n'existe pas et la table Books
IF DB_ID(N'MyBookShelfDB') IS NULL
BEGIN
    CREATE DATABASE [MyBookShelfDB];
END;
GO

USE [MyBookShelfDB];
GO

IF OBJECT_ID(N'dbo.Books', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Books
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Author NVARCHAR(200) NOT NULL,
        Status INT NOT NULL DEFAULT(0),    -- correspond à BookStatus
        Rating INT NULL,                   -- 1..5
        Comment NVARCHAR(MAX) NULL
    );
END;
GO

-- Exemple de données
INSERT INTO dbo.Books (Title, Author, Status, Rating, Comment)
VALUES (N'Le Petit Prince', N'Antoine de Saint-Exupéry', 0, 5, N'Classique');