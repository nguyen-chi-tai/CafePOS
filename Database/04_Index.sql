USE CafePOS;
GO

-- M?c l?c theo ngày t?o hóa ??n, dùng cho m?i báo cáo theo kho?ng th?i gian
CREATE INDEX IX_HoaDon_NgayTao
    ON HoaDon (NgayTao)
    INCLUDE (TongTien, TrangThai, NhanVienId);

-- M?c l?c cho khóa ngo?i HoaDonId, dùng khi n?i hóa ??n v?i chi ti?t
CREATE INDEX IX_ChiTietHoaDon_HoaDonId
    ON ChiTietHoaDon (HoaDonId)
    INCLUDE (MonId, SoLuong, DonGia);
GO