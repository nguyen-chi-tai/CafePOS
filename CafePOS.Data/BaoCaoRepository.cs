using System.Data;
using Microsoft.Data.SqlClient;
using CafePOS.Data.Models;

namespace CafePOS.Data.Repositories;

public class BaoCaoRepository
{
    // Điều kiện chung: hóa đơn đã thanh toán, trong khoảng [Tu, Den)
    // Viết dạng so sánh thẳng với cột NgayTao để dùng được index IX_HoaDon_NgayTao
    private const string DieuKien =
        "h.NgayTao >= @Tu AND h.NgayTao < @Den AND h.TrangThai = 'DaThanhToan'";

    public TongQuanDoanhThu LayTongQuan(DateTime tu, DateTime den)
    {
        string sql = $@"
            SELECT COUNT(*), ISNULL(SUM(h.TongTien), 0)
            FROM HoaDon h
            WHERE {DieuKien}";

        using var conn = Db.MoKetNoi();
        using var cmd = TaoLenh(sql, conn, tu, den);
        using var reader = cmd.ExecuteReader();
        reader.Read();
        return new TongQuanDoanhThu(reader.GetInt32(0), reader.GetDecimal(1));
    }

    public List<DoanhThuTheoMon> LayTheoMon(DateTime tu, DateTime den)
    {
        string sql = $@"
            SELECT m.Ten, SUM(c.SoLuong), SUM(c.SoLuong * c.DonGia)
            FROM ChiTietHoaDon c
            JOIN HoaDon h ON c.HoaDonId = h.Id
            JOIN Mon m    ON c.MonId = m.Id
            WHERE {DieuKien}
            GROUP BY m.Id, m.Ten
            ORDER BY SUM(c.SoLuong * c.DonGia) DESC";

        return Doc(sql, tu, den, r => new DoanhThuTheoMon(r.GetString(0), r.GetInt32(1), r.GetDecimal(2)));
    }

    public List<DoanhThuTheoNgay> LayTheoNgay(DateTime tu, DateTime den)
    {
        string sql = $@"
            SELECT CAST(h.NgayTao AS DATE) AS Ngay, COUNT(*), SUM(h.TongTien)
            FROM HoaDon h
            WHERE {DieuKien}
            GROUP BY CAST(h.NgayTao AS DATE)
            ORDER BY Ngay";

        return Doc(sql, tu, den, r => new DoanhThuTheoNgay(r.GetDateTime(0), r.GetInt32(1), r.GetDecimal(2)));
    }

    public List<DoanhThuTheoNhanVien> LayTheoNhanVien(DateTime tu, DateTime den)
    {
        string sql = $@"
            SELECT nv.HoTen, COUNT(*), SUM(h.TongTien)
            FROM HoaDon h
            JOIN NhanVien nv ON h.NhanVienId = nv.Id
            WHERE {DieuKien}
            GROUP BY nv.Id, nv.HoTen
            ORDER BY SUM(h.TongTien) DESC";

        return Doc(sql, tu, den, r => new DoanhThuTheoNhanVien(r.GetString(0), r.GetInt32(1), r.GetDecimal(2)));
    }

    // ===== Dùng chung =====

    private static SqlCommand TaoLenh(string sql, SqlConnection conn, DateTime tu, DateTime den)
    {
        var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@Tu", SqlDbType.DateTime2).Value = tu;
        cmd.Parameters.Add("@Den", SqlDbType.DateTime2).Value = den;
        return cmd;
    }

    // Chạy truy vấn, biến MỖI DÒNG kết quả thành một đối tượng bằng hàm "chuyen"
    private static List<T> Doc<T>(string sql, DateTime tu, DateTime den, Func<SqlDataReader, T> chuyen)
    {
        var ketQua = new List<T>();

        using var conn = Db.MoKetNoi();
        using var cmd = TaoLenh(sql, conn, tu, den);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            ketQua.Add(chuyen(reader));

        return ketQua;
    }
}