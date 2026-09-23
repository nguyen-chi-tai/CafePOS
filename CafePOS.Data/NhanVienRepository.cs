using System.Data;
using Microsoft.Data.SqlClient;
using CafePOS.Data.Models;

namespace CafePOS.Data.Repositories;

public class NhanVienRepository
{
    public NhanVien? LayTheoTenDangNhap(string tenDangNhap)
    {
        const string sql = @"
            SELECT Id, HoTen, TenDangNhap, MatKhauHash, VaiTro, DangLamViec
            FROM NhanVien
            WHERE TenDangNhap = @Ten";

        using var conn = new SqlConnection(Db.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@Ten", SqlDbType.VarChar, 50).Value = tenDangNhap;
        conn.Open();

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new NhanVien
        {
            Id = reader.GetInt32(0),
            HoTen = reader.GetString(1),
            TenDangNhap = reader.GetString(2),
            MatKhauHash = reader.GetString(3),
            VaiTro = reader.GetString(4),
            DangLamViec = reader.GetBoolean(5)
        };
    }

    // MỚI: danh sách nhân viên. CỐ Ý KHÔNG lấy cột MatKhauHash:
    // màn hình danh sách không cần nó, nên không mang nó đi đâu cả.
    public List<NhanVien> LayTatCa()
    {
        const string sql = @"
            SELECT Id, HoTen, TenDangNhap, VaiTro, DangLamViec
            FROM NhanVien
            ORDER BY DangLamViec DESC, HoTen";

        var ketQua = new List<NhanVien>();

        using var conn = new SqlConnection(Db.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        conn.Open();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ketQua.Add(new NhanVien
            {
                Id = reader.GetInt32(0),
                HoTen = reader.GetString(1),
                TenDangNhap = reader.GetString(2),
                VaiTro = reader.GetString(3),
                DangLamViec = reader.GetBoolean(4)
            });
        }
        return ketQua;
    }

    public int DemAdminDangLamViec()
    {
        const string sql = "SELECT COUNT(*) FROM NhanVien WHERE VaiTro = 'Admin' AND DangLamViec = 1";

        using var conn = new SqlConnection(Db.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public int Them(NhanVien nv)
    {
        const string sql = @"
            INSERT INTO NhanVien (HoTen, TenDangNhap, MatKhauHash, VaiTro)
            OUTPUT INSERTED.Id
            VALUES (@HoTen, @Ten, @Hash, @VaiTro)";

        using var conn = new SqlConnection(Db.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 100).Value = nv.HoTen;
        cmd.Parameters.Add("@Ten", SqlDbType.VarChar, 50).Value = nv.TenDangNhap;
        cmd.Parameters.Add("@Hash", SqlDbType.VarChar, 255).Value = nv.MatKhauHash;
        cmd.Parameters.Add("@VaiTro", SqlDbType.VarChar, 20).Value = nv.VaiTro;
        conn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    // MỚI: đặt mật khẩu mới (nhận vào chuỗi ĐÃ HASH, không bao giờ nhận mật khẩu thật)
    public bool DatMatKhau(int id, string matKhauHash)
    {
        const string sql = "UPDATE NhanVien SET MatKhauHash = @Hash WHERE Id = @Id";

        using var conn = new SqlConnection(Db.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@Hash", SqlDbType.VarChar, 255).Value = matKhauHash;
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        conn.Open();
        return cmd.ExecuteNonQuery() == 1;
    }

    // MỚI: bật / tắt tài khoản (soft delete cho nhân viên nghỉ việc)
    public bool DatTrangThai(int id, bool dangLamViec)
    {
        const string sql = "UPDATE NhanVien SET DangLamViec = @TrangThai WHERE Id = @Id";

        using var conn = new SqlConnection(Db.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@TrangThai", SqlDbType.Bit).Value = dangLamViec;
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        conn.Open();
        return cmd.ExecuteNonQuery() == 1;
    }
}