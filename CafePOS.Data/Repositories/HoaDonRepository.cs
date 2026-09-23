using System.Data;
using Microsoft.Data.SqlClient;
using CafePOS.Data.Models;

namespace CafePOS.Data.Repositories;

public class HoaDonRepository
{
    public int TaoHoaDon(int nhanVienId, List<ChiTietBan> chiTiet)
    {
        using var conn = new SqlConnection(Db.ConnectionString);
        conn.Open();

        // Bắt đầu transaction: từ đây, mọi lệnh hoặc cùng thành công, hoặc cùng bị hủy
        using var tran = conn.BeginTransaction();

        try
        {
            // ── Bước 1: tạo hóa đơn, lấy Id vừa tạo ──
            int hoaDonId;
            using (var cmd = new SqlCommand(
                "INSERT INTO HoaDon (NhanVienId) OUTPUT INSERTED.Id VALUES (@NhanVienId)",
                conn, tran))
            {
                cmd.Parameters.Add("@NhanVienId", SqlDbType.Int).Value = nhanVienId;
                hoaDonId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            foreach (var dong in chiTiet)
            {
                // ── Bước 2: trừ kho CÓ ĐIỀU KIỆN (chỉ trừ nếu còn đủ hàng) ──
                using (var cmd = new SqlCommand(@"
                    UPDATE Mon
                    SET SoLuongTon = SoLuongTon - @SoLuong
                    WHERE Id = @MonId AND DangBan = 1 AND SoLuongTon >= @SoLuong",
                    conn, tran))
                {
                    cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = dong.SoLuong;
                    cmd.Parameters.Add("@MonId", SqlDbType.Int).Value = dong.MonId;

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new KhongDuHangException(dong.TenMon);
                }

                // ── Bước 3: ghi chi tiết, GIÁ LẤY TỪ DATABASE tại đúng thời điểm này ──
                using (var cmd = new SqlCommand(@"
                    INSERT INTO ChiTietHoaDon (HoaDonId, MonId, SoLuong, DonGia)
                    SELECT @HoaDonId, Id, @SoLuong, GiaBan FROM Mon WHERE Id = @MonId",
                    conn, tran))
                {
                    cmd.Parameters.Add("@HoaDonId", SqlDbType.Int).Value = hoaDonId;
                    cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = dong.SoLuong;
                    cmd.Parameters.Add("@MonId", SqlDbType.Int).Value = dong.MonId;
                    cmd.ExecuteNonQuery();
                }
            }

            // ── Bước 4: tính tổng tiền từ chính các dòng chi tiết vừa ghi ──
            using (var cmd = new SqlCommand(@"
                UPDATE HoaDon
                SET TongTien = (SELECT SUM(SoLuong * DonGia) FROM ChiTietHoaDon WHERE HoaDonId = @Id)
                WHERE Id = @Id",
                conn, tran))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = hoaDonId;
                cmd.ExecuteNonQuery();
            }

            // Mọi bước đều ổn: chốt lại
            tran.Commit();
            return hoaDonId;
        }
        catch
        {
            // Có bất kỳ lỗi gì: hủy TOÀN BỘ, như chưa từng xảy ra
            tran.Rollback();
            throw;
        }
    }
}