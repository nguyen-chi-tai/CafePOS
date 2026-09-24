using CafePOS.Data;
using CafePOS.Data.Models;

namespace CafePOS.Business.BaoMat;

public static class PhienDangNhap
{
    public static NhanVien? NguoiDung { get; private set; }

    public static bool LaAdmin => NguoiDung?.VaiTro == "Admin";

    // "internal": CHỈ code trong project Business mới được bắt đầu phiên.
    internal static void BatDau(NhanVien nv)
    {
        NguoiDung = nv;
        Db.NhanVienHienTaiId = nv.Id;     // MỚI: để database biết ai đang làm việc
    }

    public static void KetThuc()
    {
        NguoiDung = null;
        Db.NhanVienHienTaiId = null;      // MỚI
    }

    public static void YeuCauAdmin()
    {
        if (!LaAdmin)
            throw new LoiNghiepVu("Bạn không có quyền thực hiện thao tác này.\nChỉ quản trị viên được phép.");
    }
}