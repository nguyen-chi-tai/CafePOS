using CafePOS.Business.Services;

namespace CafePOS.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Bước 1: lần chạy đầu tiên (chưa có quản trị viên) → tạo quản trị viên
        try
        {
            if (!new NhanVienService().CoAdmin())
            {
                using var formTao = new FormTaoAdmin();
                if (formTao.ShowDialog() != DialogResult.OK) return;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không kết nối được cơ sở dữ liệu:\n" + ex.Message, "CafePOS",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Bước 2: đăng nhập
        using var formDangNhap = new FormDangNhap();
        if (formDangNhap.ShowDialog() != DialogResult.OK || formDangNhap.NguoiDangNhap is null)
            return;

        // Bước 3: vào màn hình bán hàng với đúng người vừa đăng nhập
        Application.Run(new FormBanHang(formDangNhap.NguoiDangNhap));
    }
}