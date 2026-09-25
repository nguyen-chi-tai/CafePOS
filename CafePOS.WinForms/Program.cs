using System.Text.Json;
using CafePOS.Business.Services;
using CafePOS.Data;

namespace CafePOS.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Bước 0 (MỚI): đọc cấu hình, "lắp" chuỗi kết nối vào lớp Data
        string? chuoiKetNoi = DocChuoiKetNoi(out string? loi);
        if (chuoiKetNoi == null)
        {
            MessageBox.Show(loi, "CafePOS – Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        Db.KhoiTao(chuoiKetNoi);

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
            MessageBox.Show(
                "Không kết nối được cơ sở dữ liệu.\n\n" +
                "Hãy kiểm tra dòng \"CafePOS\" trong file appsettings.json.\n\n" +
                "Chi tiết: " + ex.Message,
                "CafePOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Bước 2: đăng nhập
        using var formDangNhap = new FormDangNhap();
        if (formDangNhap.ShowDialog() != DialogResult.OK || formDangNhap.NguoiDangNhap is null)
            return;

        // Bước 3: vào màn hình bán hàng
        Application.Run(new FormBanHang(formDangNhap.NguoiDangNhap));
    }

    // Đọc chuỗi kết nối từ appsettings.json nằm CẠNH file .exe
    private static string? DocChuoiKetNoi(out string? loi)
    {
        string duongDan = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(duongDan))
        {
            loi = $"Không tìm thấy file cấu hình:\n{duongDan}";
            return null;
        }

        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(duongDan));
            string? chuoi = doc.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("CafePOS")
                .GetString();

            loi = string.IsNullOrWhiteSpace(chuoi) ? "Chuỗi kết nối trong appsettings.json đang trống." : null;
            return string.IsNullOrWhiteSpace(chuoi) ? null : chuoi;
        }
        catch (Exception ex) when (ex is JsonException or KeyNotFoundException)
        {
            loi = "File appsettings.json bị sai định dạng.\n\nChi tiết: " + ex.Message;
            return null;
        }
    }
}