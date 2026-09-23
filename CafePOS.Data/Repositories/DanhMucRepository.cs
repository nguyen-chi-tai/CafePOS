using Microsoft.Data.SqlClient;
using CafePOS.Data.Models;

namespace CafePOS.Data.Repositories;

public class DanhMucRepository
{
    public List<DanhMuc> LayTatCa()
    {
        var ketQua = new List<DanhMuc>();

        using var conn = new SqlConnection(Db.ConnectionString);
        using var cmd = new SqlCommand("SELECT Id, Ten FROM DanhMuc ORDER BY Ten", conn);
        conn.Open();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            ketQua.Add(new DanhMuc { Id = reader.GetInt32(0), Ten = reader.GetString(1) });

        return ketQua;
    }
}