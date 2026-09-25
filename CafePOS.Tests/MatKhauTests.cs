using CafePOS.Business.BaoMat;
using Xunit;

namespace CafePOS.Tests;

public class MatKhauTests
{
    [Fact]
    public void KiemTra_DungMatKhau_TraVeTrue()
    {
        string daLuu = MatKhau.BamMatKhau("matkhau123");

        Assert.True(MatKhau.KiemTra("matkhau123", daLuu));
    }

    [Fact]
    public void KiemTra_SaiMatKhau_TraVeFalse()
    {
        string daLuu = MatKhau.BamMatKhau("matkhau123");

        Assert.False(MatKhau.KiemTra("matkhau124", daLuu));
    }

    [Fact]
    public void BamMatKhau_CungMotMatKhauHaiLan_RaHaiChuoiKhacNhau()
    {
        // Nhờ salt ngẫu nhiên, hai người cùng đặt một mật khẩu vẫn có hash khác nhau
        string lan1 = MatKhau.BamMatKhau("matkhau123");
        string lan2 = MatKhau.BamMatKhau("matkhau123");

        Assert.NotEqual(lan1, lan2);
    }

    [Fact]
    public void BamMatKhau_KetQua_KhongChuaMatKhauGoc()
    {
        string daLuu = MatKhau.BamMatKhau("matkhau123");

        Assert.DoesNotContain("matkhau123", daLuu);
        Assert.StartsWith("PBKDF2$", daLuu);
    }

    // Một bài test, chạy với NHIỀU dữ liệu đầu vào khác nhau
    [Theory]
    [InlineData("CHUA_CO_MAT_KHAU")]      // hash của nhân viên tạm ngày trước
    [InlineData("")]
    [InlineData("PBKDF2$abc$xyz$123")]    // số vòng không phải số
    [InlineData("MD5$1$a$b")]             // sai thuật toán
    public void KiemTra_ChuoiDaLuuHong_TraVeFalseKhongBaoLoi(string chuoiHong)
    {
        Assert.False(MatKhau.KiemTra("bat_ky", chuoiHong));
    }
}