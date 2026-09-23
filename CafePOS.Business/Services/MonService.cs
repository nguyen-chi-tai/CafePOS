using CafePOS.Business.BaoMat;
using CafePOS.Data.Models;
using CafePOS.Data.Repositories;

namespace CafePOS.Business.Services;

public class MonService
{
    private readonly MonRepository _repo = new();

    // Xem thực đơn: ai đăng nhập cũng được (thu ngân cần để bán hàng)
    public List<Mon> TimMon(string tuKhoa)
    {
        tuKhoa = tuKhoa.Trim();
        return tuKhoa == "" ? _repo.LayMonDangBan() : _repo.TimKiem(tuKhoa);
    }

    // Thêm / sửa món: chỉ Admin
    public void Luu(Mon mon)
    {
        PhienDangNhap.YeuCauAdmin();

        mon.Ten = mon.Ten.Trim();

        if (mon.Ten == "") throw new LoiNghiepVu("Tên món không được để trống.");
        if (mon.Ten.Length > 100) throw new LoiNghiepVu("Tên món tối đa 100 ký tự.");
        if (mon.DanhMucId <= 0) throw new LoiNghiepVu("Hãy chọn danh mục.");
        if (mon.GiaBan <= 0) throw new LoiNghiepVu("Giá bán phải lớn hơn 0.");
        if (mon.SoLuongTon < 0) throw new LoiNghiepVu("Tồn kho không được âm.");

        if (mon.Id == 0)
            mon.Id = _repo.Them(mon);
        else if (!_repo.CapNhat(mon))
            throw new LoiNghiepVu("Món này không còn tồn tại trong hệ thống.");
    }

    // Ngừng bán: chỉ Admin
    public void NgungBan(int id)
    {
        PhienDangNhap.YeuCauAdmin();

        if (!_repo.NgungBan(id))
            throw new LoiNghiepVu("Món này không còn tồn tại trong hệ thống.");
    }
}