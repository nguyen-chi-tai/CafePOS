using System.Text.RegularExpressions;
using CafePOS.Business.BaoMat;
using CafePOS.Data.Models;
using CafePOS.Data.Repositories;

namespace CafePOS.Business.Services;

public class NhanVienService
{
    private const string ThongBaoSaiDangNhap = "Tên đăng nhập hoặc mật khẩu không đúng.";

    private readonly NhanVienRepository _repo = new();

    public bool CoAdmin() => _repo.DemAdminDangLamViec() > 0;

    public NhanVien DangNhap(string tenDangNhap, string matKhau)
    {
        tenDangNhap = tenDangNhap.Trim();
        if (tenDangNhap == "" || matKhau == "")
            throw new LoiNghiepVu("Hãy nhập tên đăng nhập và mật khẩu.");

        var nv = _repo.LayTheoTenDangNhap(tenDangNhap);

        if (nv == null)
        {
            // Vẫn hash một lần cho tốn thời gian như bình thường,
            // để kẻ tấn công không đoán được "tên này không tồn tại" qua tốc độ phản hồi
            MatKhau.BamMatKhau(matKhau);
            throw new LoiNghiepVu(ThongBaoSaiDangNhap);
        }

        if (!nv.DangLamViec || !MatKhau.KiemTra(matKhau, nv.MatKhauHash))
            throw new LoiNghiepVu(ThongBaoSaiDangNhap);

        return nv;
    }

    public NhanVien TaoAdminDauTien(string hoTen, string tenDangNhap, string matKhau, string nhapLai)
    {
        if (CoAdmin())
            throw new LoiNghiepVu("Hệ thống đã có tài khoản quản trị.");

        hoTen = hoTen.Trim();
        tenDangNhap = tenDangNhap.Trim();

        if (hoTen == "")
            throw new LoiNghiepVu("Họ tên không được để trống.");
        if (!Regex.IsMatch(tenDangNhap, "^[a-zA-Z0-9_.]{3,50}$"))
            throw new LoiNghiepVu("Tên đăng nhập dài 3–50 ký tự, chỉ gồm chữ không dấu, số, dấu chấm hoặc gạch dưới.");
        if (matKhau.Length < 8)
            throw new LoiNghiepVu("Mật khẩu phải có ít nhất 8 ký tự.");
        if (matKhau != nhapLai)
            throw new LoiNghiepVu("Hai lần nhập mật khẩu không khớp.");
        if (_repo.LayTheoTenDangNhap(tenDangNhap) != null)
            throw new LoiNghiepVu("Tên đăng nhập đã tồn tại.");

        var nv = new NhanVien
        {
            HoTen = hoTen,
            TenDangNhap = tenDangNhap,
            MatKhauHash = MatKhau.BamMatKhau(matKhau),
            VaiTro = "Admin",
            DangLamViec = true
        };
        nv.Id = _repo.Them(nv);
        return nv;
    }
}