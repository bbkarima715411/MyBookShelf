-- =============================================================
-- Script de creation : base MyBookShelfDB + table dbo.Books
-- Idempotent : peut etre execute plusieurs fois sans danger.
-- =============================================================

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
        Id      INT IDENTITY(1,1) PRIMARY KEY,
        UserId  NVARCHAR(450) NOT NULL,   -- Id de l'utilisateur proprietaire (AspNetUsers.Id)
        Title   NVARCHAR(200) NOT NULL,
        Author  NVARCHAR(200) NOT NULL,
        Status  INT NOT NULL DEFAULT(0),  -- correspond a BookStatus
        IsFavorite BIT NOT NULL DEFAULT(0), -- livre marque comme favori
        Rating  INT NULL,                 -- 1..5
        Comment NVARCHAR(MAX) NULL        -- note personnelle privee
    );

    CREATE NONCLUSTERED INDEX IX_Books_UserId ON dbo.Books (UserId);
END;
GO
