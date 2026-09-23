namespace CafePOS.Data.Models;

public class NhanVien
{
    public int Id { get; set; }
    public string HoTen { get; set; } = "";
    public string TenDangNhap { get; set; } = "";
    public string MatKhauHash { get; set; } = "";
    public string VaiTro { get; set; } = "";
    public bool DangLamViec { get; set; }
}