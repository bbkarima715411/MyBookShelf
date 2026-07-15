-- =============================================================
-- Script de mise a jour Phase 3 : ajoute la colonne IsFavorite
-- a dbo.Books sur une base EXISTANTE, sans supprimer aucune donnee.
-- Idempotent : peut etre execute plusieurs fois sans danger.
--
-- Remarque : le nouveau statut de lecture "Abandonne" (valeur 3)
-- n'exige aucun changement de schema (colonne Status de type INT).
-- =============================================================

USE [MyBookShelfDB];
GO

IF OBJECT_ID(N'dbo.Books', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.Books', N'IsFavorite') IS NULL
BEGIN
    ALTER TABLE dbo.Books
    ADD IsFavorite BIT NOT NULL CONSTRAINT DF_Books_IsFavorite DEFAULT(0);
END;
GO
