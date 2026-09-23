using System.Security.Cryptography;

namespace CafePOS.Business.BaoMat;

public static class MatKhau
{
    private const int SoVong = 600_000;   // hash chậm có chủ đích
    private const int DoDaiSalt = 16;     // byte
    private const int DoDaiHash = 32;     // byte

    // Tạo chuỗi để lưu vào database, dạng: PBKDF2$600000$<salt>$<hash>
    public static string BamMatKhau(string matKhau)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(DoDaiSalt);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(matKhau, salt, SoVong, HashAlgorithmName.SHA256, DoDaiHash);

        return $"PBKDF2${SoVong}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    // Kiểm tra mật khẩu người dùng gõ có khớp với chuỗi đã lưu không
    public static bool KiemTra(string matKhau, string chuoiDaLuu)
    {
        var phan = chuoiDaLuu.Split('$');
        if (phan.Length != 4 || phan[0] != "PBKDF2") return false;
        if (!int.TryParse(phan[1], out int soVong)) return false;

        byte[] salt, hashDaLuu;
        try
        {
            salt = Convert.FromBase64String(phan[2]);
            hashDaLuu = Convert.FromBase64String(phan[3]);
        }
        catch (FormatException)
        {
            return false;
        }

        byte[] hashMoi = Rfc2898DeriveBytes.Pbkdf2(matKhau, salt, soVong, HashAlgorithmName.SHA256, hashDaLuu.Length);

        // So sánh "thời gian cố định": không để lộ thông tin qua tốc độ so sánh
        return CryptographicOperations.FixedTimeEquals(hashMoi, hashDaLuu);
    }
}