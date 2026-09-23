using CafePOS.Data.Models;
using CafePOS.Data.Repositories;

namespace CafePOS.Business.Services;

public class MonService
{
    private readonly MonRepository _repo = new();

    public List<Mon> TimMon(string tuKhoa)
    {
        tuKhoa = tuKhoa.Trim();
        return tuKhoa == "" ? _repo.LayMonDangBan() : _repo.TimKiem(tuKhoa);
    }

    public void Luu(Mon mon)
    {
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

    // MỚI: sau này sẽ thêm quy tắc, ví dụ không ngừng bán món đang có trong hóa đơn chưa thanh toán
    public void NgungBan(int id)
    {
        if (!_repo.NgungBan(id))
            throw new LoiNghiepVu("Món này không còn tồn tại trong hệ thống.");
    }
}