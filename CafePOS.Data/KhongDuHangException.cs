namespace CafePOS.Data;

public class KhongDuHangException : Exception
{
    public string TenMon { get; }

    public KhongDuHangException(string tenMon)
        : base($"Món \"{tenMon}\" không đủ số lượng trong kho.")
    {
        TenMon = tenMon;
    }
}