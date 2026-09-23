using CafePOS.Data.Models;

namespace CafePOS.Business.BaoMat;

public static class PhienDangNhap
{
    public static NhanVien? NguoiDung { get; private set; }

    public static bool LaAdmin => NguoiDung?.VaiTro == "Admin";

    // "internal": CHỈ code trong project Business mới được bắt đầu phiên.
    // Giao diện không thể tự tuyên bố "tôi là Admin".
    internal static void BatDau(NhanVien nv) => NguoiDung = nv;

    public static void KetThuc() => NguoiDung = null;

    // Người gác cổng: gọi ở đầu mọi thao tác chỉ Admin được làm
    public static void YeuCauAdmin()
    {
        if (!LaAdmin)
            throw new LoiNghiepVu("Bạn không có quyền thực hiện thao tác này.\nChỉ quản trị viên được phép.");
    }
}