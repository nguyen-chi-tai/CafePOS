using CafePOS.Business;
using CafePOS.Business.Services;
using CafePOS.Data.Models;

namespace CafePOS.WinForms;

public partial class FormTaiKhoan : Form
{
    private readonly NhanVienService _service = new();
    private readonly NhanVien? _nhanVien;   // null = tạo thu ngân mới; có giá trị = đặt lại mật khẩu

    private readonly TextBox txtHoTen = new();
    private readonly TextBox txtTenDangNhap = new();
    private readonly TextBox txtMatKhau = new() { UseSystemPasswordChar = true };
    private readonly TextBox txtNhapLai = new() { UseSystemPasswordChar = true };
    private readonly Button btnLuu = new();
    private readonly Button btnHuy = new();

    public FormTaiKhoan(NhanVien? nhanVien)
    {
        InitializeComponent();
        _nhanVien = nhanVien;
        DungGiaoDien();
    }

    private void DungGiaoDien()
    {
        bool taoMoi = _nhanVien == null;

        Text = taoMoi ? "Thêm thu ngân" : $"Đặt lại mật khẩu – {_nhanVien!.HoTen}";
        Font = GiaoDien.FontChu;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        int y = 20;
        if (taoMoi)
        {
            DatHang("Họ tên", txtHoTen, y); y += 40;
            DatHang("Tên đăng nhập", txtTenDangNhap, y); y += 40;
            txtHoTen.MaxLength = 100;
            txtTenDangNhap.MaxLength = 50;
        }
        DatHang(taoMoi ? "Mật khẩu" : "Mật khẩu mới", txtMatKhau, y); y += 40;
        DatHang("Nhập lại", txtNhapLai, y); y += 40;

        btnLuu.Text = "Lưu";
        btnLuu.SetBounds(240, y + 15, 85, 34);
        GiaoDien.NutChinh(btnLuu);
        btnLuu.Click += BtnLuu_Click;

        btnHuy.Text = "Hủy";
        btnHuy.SetBounds(335, y + 15, 85, 34);
        btnHuy.DialogResult = DialogResult.Cancel;
        GiaoDien.NutPhu(btnHuy);

        Controls.Add(btnLuu);
        Controls.Add(btnHuy);
        AcceptButton = btnLuu;
        CancelButton = btnHuy;

        ClientSize = new Size(440, y + 15 + 34 + 20);   // chiều cao tự tính theo số hàng
    }

    private void DatHang(string nhan, Control oNhap, int y)
    {
        Controls.Add(new Label { Text = nhan, AutoSize = true, Location = new Point(20, y + 3) });
        oNhap.SetBounds(150, y, 270, 25);
        Controls.Add(oNhap);
    }

    private void BtnLuu_Click(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            if (_nhanVien == null)
                _service.TaoThuNgan(txtHoTen.Text, txtTenDangNhap.Text, txtMatKhau.Text, txtNhapLai.Text);
            else
                _service.DatLaiMatKhau(_nhanVien.Id, txtMatKhau.Text, txtNhapLai.Text);

            DialogResult = DialogResult.OK;
        }
        catch (LoiNghiepVu ex)
        {
            Cursor = Cursors.Default;
            MessageBox.Show(this, ex.Message, "Chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            Cursor = Cursors.Default;
            MessageBox.Show(this, "Lỗi hệ thống:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}