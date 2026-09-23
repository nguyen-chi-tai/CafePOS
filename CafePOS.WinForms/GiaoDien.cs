namespace CafePOS.WinForms;

public static class GiaoDien
{
    public static readonly Color MauChinh = Color.FromArgb(0, 120, 215);
    public static readonly Color MauNguyHiem = Color.FromArgb(200, 40, 40);
    public static readonly Font FontChu = new("Segoe UI", 10);

    // Nút chính: hành động quan trọng nhất của màn hình (tô màu)
    public static void NutChinh(Button nut)
    {
        nut.FlatStyle = FlatStyle.Flat;
        nut.FlatAppearance.BorderSize = 0;
        nut.BackColor = MauChinh;
        nut.ForeColor = Color.White;
        nut.Cursor = Cursors.Hand;
    }

    // Nút phụ: các hành động còn lại (nền trắng, viền xám)
    public static void NutPhu(Button nut)
    {
        nut.FlatStyle = FlatStyle.Flat;
        nut.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
        nut.BackColor = Color.White;
        nut.ForeColor = Color.FromArgb(40, 40, 40);
        nut.Cursor = Cursors.Hand;
    }

    // MỚI: nút nguy hiểm, hành động làm mất hoặc ẩn dữ liệu (chữ đỏ)
    public static void NutNguyHiem(Button nut)
    {
        NutPhu(nut);
        nut.ForeColor = MauNguyHiem;
    }
}