using System.Data;
using Microsoft.Data.SqlClient;
using CafePOS.Data.Models;

namespace CafePOS.Data.Repositories;

public class MonRepository
{
    private const string CauSelect = @"
        SELECT m.Id, m.Ten, d.Ten AS DanhMuc, m.GiaBan, m.SoLuongTon, m.DanhMucId
        FROM Mon m JOIN DanhMuc d ON m.DanhMucId = d.Id
        WHERE m.DangBan = 1";

    public List<Mon> LayMonDangBan() =>
        ChayTruyVan(CauSelect + " ORDER BY d.Ten, m.Ten");

    public List<Mon> TimKiem(string tuKhoa) =>
        ChayTruyVan(CauSelect + " AND m.Ten LIKE @tuKhoa ORDER BY d.Ten, m.Ten",
            new SqlParameter("@tuKhoa", "%" + tuKhoa + "%"));

    public int Them(Mon mon)
    {
        const string sql = @"
            INSERT INTO Mon (Ten, DanhMucId, GiaBan, SoLuongTon)
            OUTPUT INSERTED.Id
            VALUES (@Ten, @DanhMucId, @GiaBan, @SoLuongTon)";

        using var conn = Db.MoKetNoi();
        using var cmd = new SqlCommand(sql, conn);
        GanThamSo(cmd, mon);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public bool CapNhat(Mon mon)
    {
        const string sql = @"
            UPDATE Mon
            SET Ten = @Ten, DanhMucId = @DanhMucId, GiaBan = @GiaBan, SoLuongTon = @SoLuongTon
            WHERE Id = @Id";

        using var conn = Db.MoKetNoi();
        using var cmd = new SqlCommand(sql, conn);
        GanThamSo(cmd, mon);
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = mon.Id;
        return cmd.ExecuteNonQuery() == 1;
    }

    public bool NgungBan(int id)
    {
        const string sql = "UPDATE Mon SET DangBan = 0 WHERE Id = @Id";

        using var conn = Db.MoKetNoi();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        return cmd.ExecuteNonQuery() == 1;
    }

    private static void GanThamSo(SqlCommand cmd, Mon mon)
    {
        cmd.Parameters.Add("@Ten", SqlDbType.NVarChar, 100).Value = mon.Ten;
        cmd.Parameters.Add("@DanhMucId", SqlDbType.Int).Value = mon.DanhMucId;
        cmd.Parameters.Add("@GiaBan", SqlDbType.Decimal).Value = mon.GiaBan;
        cmd.Parameters.Add("@SoLuongTon", SqlDbType.Int).Value = mon.SoLuongTon;
    }

    private static List<Mon> ChayTruyVan(string sql, params SqlParameter[] thamSo)
    {
        var ketQua = new List<Mon>();

        using var conn = Db.MoKetNoi();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddRange(thamSo);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ketQua.Add(new Mon
            {
                Id = reader.GetInt32(0),
                Ten = reader.GetString(1),
                DanhMuc = reader.GetString(2),
                GiaBan = reader.GetDecimal(3),
                SoLuongTon = reader.GetInt32(4),
                DanhMucId = reader.GetInt32(5)
            });
        }
        return ketQua;
    }
}