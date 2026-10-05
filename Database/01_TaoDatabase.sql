CREATE DATABASE CafePOS;
GO
USE CafePOS;
GO

CREATE TABLE NhanVien (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    HoTen        NVARCHAR(100) NOT NULL,
    TenDangNhap  VARCHAR(50)   NOT NULL UNIQUE,
    MatKhauHash  VARCHAR(255)  NOT NULL,
    VaiTro       VARCHAR(20)   NOT NULL CHECK (VaiTro IN ('Admin', 'ThuNgan')),
    DangLamViec  BIT           NOT NULL DEFAULT 1
);

CREATE TABLE DanhMuc (
    Id   INT IDENTITY(1,1) PRIMARY KEY,
    Ten  NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Mon (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Ten         NVARCHAR(100) NOT NULL,
    DanhMucId   INT           NOT NULL REFERENCES DanhMuc(Id),
    GiaBan      DECIMAL(18,0) NOT NULL CHECK (GiaBan >= 0),
    SoLuongTon  INT           NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    DangBan     BIT           NOT NULL DEFAULT 1
);

CREATE TABLE HoaDon (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    NgayTao     DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    NhanVienId  INT           NOT NULL REFERENCES NhanVien(Id),
    TongTien    DECIMAL(18,0) NOT NULL DEFAULT 0,
    TrangThai   VARCHAR(20)   NOT NULL DEFAULT 'DaThanhToan'
                CHECK (TrangThai IN ('DaThanhToan', 'DaHuy'))
);

CREATE TABLE ChiTietHoaDon (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    HoaDonId  INT           NOT NULL REFERENCES HoaDon(Id),
    MonId     INT           NOT NULL REFERENCES Mon(Id),
    SoLuong   INT           NOT NULL CHECK (SoLuong > 0),
    DonGia    DECIMAL(18,0) NOT NULL
);
GO

INSERT INTO DanhMuc (Ten) VALUES (N'Cà phê'), (N'Trà'), (N'Bánh');

INSERT INTO Mon (Ten, DanhMucId, GiaBan, SoLuongTon) VALUES
    (N'Cà phê s?a ?á',  1, 29000, 100),
    (N'B?c x?u',        1, 32000, 100),
    (N'Trà ?ào cam s?', 2, 45000, 50),
    (N'Bánh croissant', 3, 35000, 20);