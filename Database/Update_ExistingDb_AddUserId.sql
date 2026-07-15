-- =============================================================
-- Script de mise a jour : ajoute la colonne UserId a dbo.Books
-- sur une base EXISTANTE, sans supprimer aucune donnee.
-- Idempotent : peut etre execute plusieurs fois sans danger.
-- =============================================================

USE [MyBookShelfDB];
GO

-- 1) Ajout de la colonne UserId (NULL dans un premier temps pour
--    ne pas casser les lignes existantes).
IF OBJECT_ID(N'dbo.Books', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.Books', N'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD UserId NVARCHAR(450) NULL;
END;
GO

-- 2) Index sur UserId (toutes les requetes de l'application filtrent
--    par utilisateur).
IF OBJECT_ID(N'dbo.Books', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes
                   WHERE name = N'IX_Books_UserId'
                     AND object_id = OBJECT_ID(N'dbo.Books'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Books_UserId ON dbo.Books (UserId);
END;
GO

-- 3) FACULTATIF : rattacher les livres existants (UserId NULL) a un
--    utilisateur. Les livres sans proprietaire ne sont visibles par
--    personne dans l'application.
--    Remplacez la valeur ci-dessous par l'Id du compte concerne
--    (colonne Id de la table AspNetUsers), puis decommentez :
--
-- UPDATE dbo.Books
-- SET UserId = N'ID-UTILISATEUR-ICI'
-- WHERE UserId IS NULL;
-- GO

-- 4) FACULTATIF : une fois tous les livres rattaches a un compte,
--    rendre la colonne obligatoire (comme dans le script de creation) :
--
-- IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE UserId IS NULL)
-- BEGIN
--     ALTER TABLE dbo.Books ALTER COLUMN UserId NVARCHAR(450) NOT NULL;
-- END;
-- GO
