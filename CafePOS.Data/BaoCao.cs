namespace CafePOS.Data.Models;

public record TongQuanDoanhThu(int SoHoaDon, decimal DoanhThu)
{
    public decimal TrungBinhMoiHoaDon => SoHoaDon == 0 ? 0 : DoanhThu / SoHoaDon;
}

public record DoanhThuTheoMon(string TenMon, int SoLuong, decimal DoanhThu);

public record DoanhThuTheoNgay(DateTime Ngay, int SoHoaDon, decimal DoanhThu);

public record DoanhThuTheoNhanVien(string HoTen, int SoHoaDon, decimal DoanhThu);