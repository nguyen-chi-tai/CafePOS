using CafePOS.Data.Models;

namespace CafePOS.Business.Models;

public record BaoCaoDayDu(
    TongQuanDoanhThu TongQuan,
    List<DoanhThuTheoMon> TheoMon,
    List<DoanhThuTheoNgay> TheoNgay,
    List<DoanhThuTheoNhanVien> TheoNhanVien);