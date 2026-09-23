namespace CafePOS.Business.Models;

public class DongDonHang
{
    public int MonId { get; init; }
    public string TenMon { get; init; } = "";
    public decimal DonGia { get; init; }
    public int SoLuong { get; internal set; }
    public decimal ThanhTien => DonGia * SoLuong;
}