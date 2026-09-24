using System.Data;
using Microsoft.Data.SqlClient;

namespace CafePOS.Data;

public static class Db
{
    public const string ConnectionString =
        @"Server=.\SQLEXPRESS;Database=CafePOS;Trusted_Connection=True;TrustServerCertificate=True;";

    // Id nhân viên đang làm việc, dùng để database ghi nhật ký "ai đã sửa".
    // Được lớp Business gán khi đăng nhập / đăng xuất.
    public static int? NhanVienHienTaiId { get; set; }

    // Mở kết nối VÀ gắn "thẻ tên" nhân viên lên kết nối đó
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