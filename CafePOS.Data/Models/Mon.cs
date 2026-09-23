namespace CafePOS.Data.Models;

public class Mon
{
    public int Id { get; set; }
    public string Ten { get; set; } = "";
    public string DanhMuc { get; set; } = "";
    public decimal GiaBan { get; set; }
    public int SoLuongTon { get; set; }
    public int DanhMucId { get; set; }
}