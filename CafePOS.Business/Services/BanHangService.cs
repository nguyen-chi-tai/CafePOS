using CafePOS.Business.Models;
using CafePOS.Data;
using CafePOS.Data.Models;
using CafePOS.Data.Repositories;

namespace CafePOS.Business.Services;

public class BanHangService
{
    // NO KY THUAT: dung tam nhan vien Id = 1.
    // Giai doan 3 se thay bang nguoi dang dang nhap.
    private const int NhanVienTamId = 1;

    private readonly HoaDonRepository _repo = new();

    public KetQuaThanhToan ThanhToan(DonHang donHang)
    {
        if (donHang.Trong)
            throw new LoiNghiepVu("Đơn hàng đang trống.");

        var chiTiet = donHang.CacDong
            .Select(d => new ChiTietBan { MonId = d.MonId, TenMon = d.TenMon, SoLuong = d.SoLuong })
            .ToList();

        try
        {
            int hoaDonId = _repo.TaoHoaDon(NhanVienTamId, chiTiet);
            return new KetQuaThanhToan(hoaDonId, _repo.LayTongTien(hoaDonId));
        }
        catch (KhongDuHangException ex)
        {
            throw new LoiNghiepVu(ex.Message + "\nCó thể máy khác vừa bán mất. Hãy kiểm tra lại đơn.");
        }
    }
}