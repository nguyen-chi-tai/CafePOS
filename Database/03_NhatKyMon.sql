USE CafePOS;
GO

CREATE TABLE NhatKyMon (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MonId       INT           NOT NULL REFERENCES Mon(Id),
    ThoiGian    DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    NhanVienId  INT           NULL REFERENCES NhanVien(Id),   -- NULL = sửa ngoài app
    Truong      NVARCHAR(30)  NOT NULL,
    GiaTriCu    NVARCHAR(100) NULL,
    GiaTriMoi   NVARCHAR(100) NULL
);
GO

CREATE TRIGGER trg_Mon_NhatKy
ON Mon
AFTER UPDATE
AS
BEGIN
    -- BẮT BUỘC: không để số dòng của trigger lẫn vào kết quả trả về cho app
    SET NOCOUNT ON;

    -- Đọc "thẻ tên" app gắn lên kết nối (NULL nếu sửa từ SSMS)
    DECLARE @NhanVienId INT = CAST(SESSION_CONTEXT(N'NhanVienId') AS INT);

    INSERT INTO NhatKyMon (MonId, NhanVienId, Truong, GiaTriCu, GiaTriMoi)
    SELECT i.Id, @NhanVienId, N'Giá bán', FORMAT(d.GiaBan, 'N0'), FORMAT(i.GiaBan, 'N0')
    FROM inserted i JOIN deleted d ON i.Id = d.Id
    WHERE i.GiaBan <> d.GiaBan

    UNION ALL
    SELECT i.Id, @NhanVienId, N'Tên món', d.Ten, i.Ten
    FROM inserted i JOIN deleted d ON i.Id = d.Id
    WHERE i.Ten <> d.Ten

    UNION ALL
    SELECT i.Id, @NhanVienId, N'Trạng thái',
           CASE d.DangBan WHEN 1 THEN N'Đang bán' ELSE N'Ngừng bán' END,
           CASE i.DangBan WHEN 1 THEN N'Đang bán' ELSE N'Ngừng bán' END
    FROM inserted i JOIN deleted d ON i.Id = d.Id
    WHERE i.DangBan <> d.DangBan;
END
GO