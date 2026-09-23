using CafePOS.Data.Models;

namespace CafePOS.Business.Models;

public class DonHang
{
    private readonly List<DongDonHang> _cacDong = new();

    public IReadOnlyList<DongDonHang> CacDong => _cacDong;
    public decimal TongTien => _cacDong.Sum(d => d.ThanhTien);
    public bool Trong => _cacDong.Count == 0;

    // Bấm vào một món: món đã có thì +1, chưa có thì thêm dòng mới
    public void ThemMon(Mon mon)
    {
        var dong = _cacDong.FirstOrDefault(d => d.MonId == mon.Id);
        int soLuongMoi = (dong?.SoLuong ?? 0) + 1;

        if (soLuongMoi > mon.SoLuongTon)
            throw new LoiNghiepVu(mon.SoLuongTon == 0
                ? $"\"{mon.Ten}\" đã hết hàng."
                : $"\"{mon.Ten}\" chỉ còn {mon.SoLuongTon} trong kho.");

        if (dong == null)
            _cacDong.Add(new DongDonHang
            {
                MonId = mon.Id,
                TenMon = mon.Ten,
                DonGia = mon.GiaBan,
                SoLuong = 1
            });
        else
            dong.SoLuong = soLuongMoi;
    }

    // Bớt 1: về 0 thì xóa luôn dòng đó
    public void BotMot(int monId)
    {
        var dong = _cacDong.FirstOrDefault(d => d.MonId == monId);
        if (dong == null) return;

        dong.SoLuong--;
        if (dong.SoLuong == 0)
            _cacDong.Remove(dong);
    }

    public void XoaMon(int monId) => _cacDong.RemoveAll(d => d.MonId == monId);

    public void XoaHet() => _cacDong.Clear();
}