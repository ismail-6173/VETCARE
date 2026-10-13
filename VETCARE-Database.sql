
CREATE DATABASE VetCareDB;
GO

USE VetCareDB;
GO

-- 1. KULLANICILAR
CREATE TABLE Kullanicilar (
    KullaniciID INT IDENTITY(1,1) PRIMARY KEY,
    KullaniciAdi NVARCHAR(50) NOT NULL UNIQUE,
    SifreHash NVARCHAR(255) NOT NULL,
    AdSoyad NVARCHAR(100) NOT NULL,
    Rol NVARCHAR(20) NOT NULL,
    Aktif BIT NOT NULL DEFAULT 1,

    CONSTRAINT CK_Kullanicilar_Rol
        CHECK (Rol IN (N'Yonetici', N'Uye'))
);
GO

-- 2. SAHIPLER
CREATE TABLE Sahipler (
    SahipID INT IDENTITY(1,1) PRIMARY KEY,
    AdSoyad NVARCHAR(100) NOT NULL,
    Telefon NVARCHAR(20),
    Eposta NVARCHAR(100),
    Adres NVARCHAR(250)
);
GO

-- 3. HAYVANLAR
CREATE TABLE Hayvanlar (
    HayvanID INT IDENTITY(1,1) PRIMARY KEY,
    SahipID INT NOT NULL,
    HayvanAdi NVARCHAR(100) NOT NULL,
    Tur NVARCHAR(50) NOT NULL,
    Irk NVARCHAR(50),
    Cinsiyet NVARCHAR(20),
    DogumTarihi DATE,

    CONSTRAINT FK_Hayvanlar_Sahipler
        FOREIGN KEY (SahipID)
        REFERENCES Sahipler(SahipID)
);
GO

-- 4. RANDEVULAR
CREATE TABLE Randevular (
    RandevuID INT IDENTITY(1,1) PRIMARY KEY,
    HayvanID INT NOT NULL,
    RandevuTarihi DATETIME NOT NULL,
    Aciklama NVARCHAR(500),
    Durum NVARCHAR(30) NOT NULL DEFAULT N'Bekliyor',

    CONSTRAINT FK_Randevular_Hayvanlar
        FOREIGN KEY (HayvanID)
        REFERENCES Hayvanlar(HayvanID)
);
GO

-- 5. MUAYENELER
CREATE TABLE Muayeneler (
    MuayeneID INT IDENTITY(1,1) PRIMARY KEY,
    HayvanID INT NOT NULL,
    MuayeneTarihi DATETIME NOT NULL,
    Sikayet NVARCHAR(500),
    Teshis NVARCHAR(500),
    Notlar NVARCHAR(1000),

    CONSTRAINT FK_Muayeneler_Hayvanlar
        FOREIGN KEY (HayvanID)
        REFERENCES Hayvanlar(HayvanID)
);
GO

-- 6. ASILAR
CREATE TABLE Asilar (
    AsiID INT IDENTITY(1,1) PRIMARY KEY,
    HayvanID INT NOT NULL,
    AsiAdi NVARCHAR(100) NOT NULL,
    YapilmaTarihi DATE NOT NULL,
    SonrakiAsiTarihi DATE,

    CONSTRAINT FK_Asilar_Hayvanlar
        FOREIGN KEY (HayvanID)
        REFERENCES Hayvanlar(HayvanID),

    CONSTRAINT CK_Asilar_Tarih
        CHECK (
            SonrakiAsiTarihi IS NULL
            OR SonrakiAsiTarihi >= YapilmaTarihi
        )
);
GO

-- 7. TEDAVILER
CREATE TABLE Tedaviler (
    TedaviID INT IDENTITY(1,1) PRIMARY KEY,
    HayvanID INT NOT NULL,
    MuayeneID INT NOT NULL,
    IlacAdi NVARCHAR(100),
    Doz NVARCHAR(100),
    BaslangicTarihi DATE,
    BitisTarihi DATE,
    Aciklama NVARCHAR(500),

    CONSTRAINT FK_Tedaviler_Hayvanlar
        FOREIGN KEY (HayvanID)
        REFERENCES Hayvanlar(HayvanID),

    CONSTRAINT FK_Tedaviler_Muayeneler
        FOREIGN KEY (MuayeneID)
        REFERENCES Muayeneler(MuayeneID)
);
GO

-- TABLOLARI KONTROL ET
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';
GO
