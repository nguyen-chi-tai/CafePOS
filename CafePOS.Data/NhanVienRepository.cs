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
}