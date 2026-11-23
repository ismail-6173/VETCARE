USE VetCareDB;
GO

-- Kullanıcı profillerine fotoğraf alanı ekler.
-- Bu komut mevcut kullanıcıları veya diğer kayıtları silmez.
IF COL_LENGTH('dbo.Kullanicilar', 'ProfilFotografi') IS NULL
BEGIN
    ALTER TABLE dbo.Kullanicilar
    ADD ProfilFotografi VARBINARY(MAX) NULL;
END
GO
