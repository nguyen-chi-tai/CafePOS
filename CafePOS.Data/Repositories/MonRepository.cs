using Microsoft.Data.SqlClient;
using CafePOS.Data.Models;

namespace CafePOS.Data.Repositories;

public class MonRepository
{
    public List<Mon> LayMonDangBan()
    {
        const string sql = @"
            SELECT m.Id, m.Ten, d.Ten AS DanhMuc, m.GiaBan, m.SoLuongTon
            FROM Mon m JOIN DanhMuc d ON m.DanhMucId = d.Id
            WHERE m.DangBan = 1
            ORDER BY d.Ten, m.Ten";

        var ketQua = new List<Mon>();

        using var conn = new SqlConnection(Db.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        conn.Open();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ketQua.Add(new Mon
            {
                Id = reader.GetInt32(0),
                Ten = reader.GetString(1),
                DanhMuc = reader.GetString(2),
                GiaBan = reader.GetDecimal(3),
                SoLuongTon = reader.GetInt32(4)
            });
        }
        return ketQua;
    }
}