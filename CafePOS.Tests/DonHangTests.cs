using CafePOS.Business;
using CafePOS.Business.Models;
using CafePOS.Data.Models;
using Xunit;

namespace CafePOS.Tests;

public class DonHangTests
{
    // Hàm tạo món mẫu cho test, không cần database
    private static Mon TaoMon(int id = 1, string ten = "Bạc xỉu", decimal gia = 30000, int ton = 10) =>
        new Mon { Id = id, Ten = ten, GiaBan = gia, SoLuongTon = ton };

    [Fact]
    public void ThemMon_MonMoi_TaoMotDongVoiSoLuong1()
    {
        // Arrange: chuẩn bị
        var don = new DonHang();

        // Act: hành động
        don.ThemMon(TaoMon());

        // Assert: kiểm tra kết quả
        Assert.Single(don.CacDong);
        Assert.Equal(1, don.CacDong[0].SoLuong);
    }

    [Fact]
    public void ThemMon_MonDaCoTrongDon_TangSoLuongChuKhongThemDong()
    {
        var don = new DonHang();
        var bacXiu = TaoMon();

        don.ThemMon(bacXiu);
        don.ThemMon(bacXiu);

        Assert.Single(don.CacDong);
        Assert.Equal(2, don.CacDong[0].SoLuong);
    }

    [Fact]
    public void ThemMon_VuotQuaTonKho_BaoLoiVaKhongThayDoiDon()
    {
        var don = new DonHang();
        var mon = TaoMon(ton: 1);
        don.ThemMon(mon);

        var loi = Assert.Throws<LoiNghiepVu>(() => don.ThemMon(mon));

        Assert.Contains("chỉ còn 1", loi.Message);
        Assert.Equal(1, don.CacDong[0].SoLuong);   // đơn vẫn giữ nguyên 1 ly
    }

    [Fact]
    public void ThemMon_MonHetHang_BaoLoiHetHang()
    {
        var don = new DonHang();

        var loi = Assert.Throws<LoiNghiepVu>(() => don.ThemMon(TaoMon(ton: 0)));

        Assert.Contains("hết hàng", loi.Message);
        Assert.True(don.Trong);
    }

    [Fact]
    public void TongTien_NhieuMon_BangTongThanhTienCacDong()
    {
        var don = new DonHang();
        var bacXiu = TaoMon(id: 1, gia: 30000);
        var traDao = TaoMon(id: 2, ten: "Trà đào", gia: 45000);

        don.ThemMon(bacXiu);
        don.ThemMon(bacXiu);
        don.ThemMon(traDao);

        Assert.Equal(105000m, don.TongTien);   // 2 × 30.000 + 45.000
    }

    [Fact]
    public void BotMot_SoLuongVe0_XoaLuonDong()
    {
        var don = new DonHang();
        var mon = TaoMon();
        don.ThemMon(mon);

        don.BotMot(mon.Id);

        Assert.True(don.Trong);
    }

    [Fact]
    public void BotMot_MonKhongCoTrongDon_KhongLamGiCa()
    {
        var don = new DonHang();
        don.ThemMon(TaoMon(id: 1));

        don.BotMot(999);

        Assert.Single(don.CacDong);
    }

    [Fact]
    public void XoaHet_DonCoNhieuMon_DonTrongVaTongBang0()
    {
        var don = new DonHang();
        don.ThemMon(TaoMon(id: 1));
        don.ThemMon(TaoMon(id: 2));

        don.XoaHet();

        Assert.True(don.Trong);
        Assert.Equal(0m, don.TongTien);
    }
}