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

    // ===== Đăng nhập =====

    public NhanVien DangNhap(string tenDangNhap, string matKhau)
    {
        tenDangNhap = tenDangNhap.Trim();
        if (tenDangNhap == "" || matKhau == "")
            throw new LoiNghiepVu("Hãy nhập tên đăng nhập và mật khẩu.");

        var nv = _repo.LayTheoTenDangNhap(tenDangNhap);

        if (nv == null)
        {
            MatKhau.BamMatKhau(matKhau);   // tốn thời gian như bình thường, chống đoán tên qua tốc độ
            throw new LoiNghiepVu(ThongBaoSaiDangNhap);
        }

        if (!nv.DangLamViec || !MatKhau.KiemTra(matKhau, nv.MatKhauHash))
            throw new LoiNghiepVu(ThongBaoSaiDangNhap);

        PhienDangNhap.BatDau(nv);   // MỚI: chỉ tới được dòng này khi mật khẩu đúng
        return nv;
    }

    // ===== Tạo tài khoản =====

    public NhanVien TaoAdminDauTien(string hoTen, string tenDangNhap, string matKhau, string nhapLai)
    {
        if (CoAdmin())
            throw new LoiNghiepVu("Hệ thống đã có tài khoản quản trị.");

        return TaoTaiKhoan(hoTen, tenDangNhap, matKhau, nhapLai, "Admin");
    }

    public NhanVien TaoThuNgan(string hoTen, string tenDangNhap, string matKhau, string nhapLai)
    {
        PhienDangNhap.YeuCauAdmin();
        return TaoTaiKhoan(hoTen, tenDangNhap, matKhau, nhapLai, "ThuNgan");
    }

    // ===== Quản lý nhân viên (chỉ Admin) =====

    public List<NhanVien> LayDanhSach()
    {
        PhienDangNhap.YeuCauAdmin();
        return _repo.LayTatCa();
    }

    public void DatLaiMatKhau(int nhanVienId, string matKhau, string nhapLai)
    {
        PhienDangNhap.YeuCauAdmin();
        KiemTraMatKhauMoi(matKhau, nhapLai);

        if (!_repo.DatMatKhau(nhanVienId, MatKhau.BamMatKhau(matKhau)))
            throw new LoiNghiepVu("Nhân viên không còn tồn tại.");
    }

    public void DoiTrangThai(int nhanVienId, bool dangLamViec)
    {
        PhienDangNhap.YeuCauAdmin();

        if (!dangLamViec && nhanVienId == PhienDangNhap.NguoiDung!.Id)
            throw new LoiNghiepVu("Không thể tự vô hiệu hóa tài khoản đang đăng nhập.");

        if (!_repo.DatTrangThai(nhanVienId, dangLamViec))
            throw new LoiNghiepVu("Nhân viên không còn tồn tại.");
    }

    // ===== Dùng chung =====

    private NhanVien TaoTaiKhoan(string hoTen, string tenDangNhap, string matKhau, string nhapLai, string vaiTro)
    {
        hoTen = hoTen.Trim();
        tenDangNhap = tenDangNhap.Trim();

        if (hoTen == "")
            throw new LoiNghiepVu("Họ tên không được để trống.");
        if (!Regex.IsMatch(tenDangNhap, "^[a-zA-Z0-9_.]{3,50}$"))
            throw new LoiNghiepVu("Tên đăng nhập dài 3–50 ký tự, chỉ gồm chữ không dấu, số, dấu chấm hoặc gạch dưới.");
        KiemTraMatKhauMoi(matKhau, nhapLai);
        if (_repo.LayTheoTenDangNhap(tenDangNhap) != null)
            throw new LoiNghiepVu("Tên đăng nhập đã tồn tại.");

        var nv = new NhanVien
        {
            HoTen = hoTen,
            TenDangNhap = tenDangNhap,
            MatKhauHash = MatKhau.BamMatKhau(matKhau),
            VaiTro = vaiTro,
            DangLamViec = true
        };
        nv.Id = _repo.Them(nv);
        return nv;
    }

    private static void KiemTraMatKhauMoi(string matKhau, string nhapLai)
    {
        if (matKhau.Length < 8)
            throw new LoiNghiepVu("Mật khẩu phải có ít nhất 8 ký tự.");
        if (matKhau != nhapLai)
            throw new LoiNghiepVu("Hai lần nhập mật khẩu không khớp.");
    }
}