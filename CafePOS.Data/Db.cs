using System.Data;
using Microsoft.Data.SqlClient;

namespace CafePOS.Data;

public static class Db
{
    private static string? _chuoiKetNoi;

    // Gọi MỘT LẦN khi app khởi động (từ Program.cs), với chuỗi đọc từ file cấu hình
    public static void KhoiTao(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

    // Các repository cũ vẫn dùng tên này, nên KHÔNG phải sửa chúng
    public static string ConnectionString =>
        _chuoiKetNoi ?? throw new InvalidOperationException(
            "Chưa cấu hình kết nối cơ sở dữ liệu. Hãy gọi Db.KhoiTao() khi khởi động.");

    // Id nhân viên đang làm việc, dùng cho nhật ký thay đổi (F3)
    public static int? NhanVienHienTaiId { get; set; }

    public static SqlConnection MoKetNoi()
    {
        var conn = new SqlConnection(ConnectionString);
        conn.Open();

        if (NhanVienHienTaiId is int id)
        {
            using var cmd = new SqlCommand(
                "EXEC sp_set_session_context @key = N'NhanVienId', @value = @Id", conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
        }

        return conn;
    }
}