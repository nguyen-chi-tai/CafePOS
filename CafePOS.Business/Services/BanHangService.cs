using CafePOS.Business.Models;
using CafePOS.Data;
using CafePOS.Data.Models;
using CafePOS.Data.Repositories;

namespace CafePOS.Business.Services;

public class BanHangService
{
    private readonly int _nhanVienId;
    private readonly HoaDonRepository _repo = new();

    // Mỗi phiên bán hàng gắn với đúng một nhân viên đã đăng nhập
    public BanHangService(int nhanVienId)
    {
        _nhanVienId = nhanVienId;
    }

    public KetQuaThanhToan ThanhToan(DonHang donHang)
    {
        if (donHang.Trong)
            throw new LoiNghiepVu("Đơn hàng đang trống.");

        var chiTiet = donHang.CacDong
            .Select(d => new ChiTietBan { MonId = d.MonId, TenMon = d.TenMon, SoLuong = d.SoLuong })
            .ToList();

        try
        {
            int hoaDonId = _repo.TaoHoaDon(_nhanVienId, chiTiet);
            return new KetQuaThanhToan(hoaDonId, _repo.LayTongTien(hoaDonId));
        }
        catch (KhongDuHangException ex)
        {
            throw new LoiNghiepVu(ex.Message + "\nCó thể máy khác vừa bán mất. Hãy kiểm tra lại đơn.");
        }
    }
}