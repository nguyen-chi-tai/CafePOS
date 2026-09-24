using CafePOS.Business.BaoMat;
using CafePOS.Business.Models;
using CafePOS.Data.Repositories;

namespace CafePOS.Business.Services;

public class BaoCaoService
{
    private readonly BaoCaoRepository _repo = new();

    public BaoCaoDayDu LayBaoCao(DateTime tuNgay, DateTime denNgay)
    {
        // Doanh thu là thông tin nhạy cảm: chỉ Admin được xem
        PhienDangNhap.YeuCauAdmin();

        // Người dùng chọn "từ 01/09 đến 30/09" → hiểu là trọn ngày 30/09.
        // Chuyển thành khoảng [00:00 ngày 01/09, 00:00 ngày 01/10)
        DateTime tu = tuNgay.Date;
        DateTime den = denNgay.Date.AddDays(1);

        if (tu >= den)
            throw new LoiNghiepVu("Ngày bắt đầu phải trước hoặc bằng ngày kết thúc.");
        if ((den - tu).TotalDays > 366)
            throw new LoiNghiepVu("Mỗi lần chỉ xem tối đa 1 năm.");

        return new BaoCaoDayDu(
            _repo.LayTongQuan(tu, den),
            _repo.LayTheoMon(tu, den),
            _repo.LayTheoNgay(tu, den),
            _repo.LayTheoNhanVien(tu, den));
    }
}