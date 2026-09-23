using CafePOS.Business;
using CafePOS.Business.Services;

namespace CafePOS.WinForms;

public partial class FormTaoAdmin : Form
{
    private readonly NhanVienService _service = new();

    private readonly TextBox txtHoTen = new();
    private readonly TextBox txtTenDangNhap = new();
    private readonly TextBox txtMatKhau = new() { UseSystemPasswordChar = true };
    private readonly TextBox txtNhapLai = new() { UseSystemPasswordChar = true };
    private readonly Button btnTao = new();
    private readonly Button btnThoat = new();

    public FormTaoAdmin()
    {
        InitializeComponent();
        DungGiaoDien();
    }

    private void DungGiaoDien()
    {
        Text = "CafePOS – Thiết lập lần đầu";
        Font = GiaoDien.FontChu;
        ClientSize = new Size(440, 320);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        var lblGioiThieu = new Label
        {
            Text = "Chưa có tài khoản quản trị.\nHãy tạo tài khoản đầu tiên để bắt đầu sử dụng CafePOS.",
            Location = new Point(20, 15),
            Size = new Size(400, 45)
        };
        Controls.Add(lblGioiThieu);

        DatHang("Họ tên", txtHoTen, 75);
        DatHang("Tên đăng nhập", txtTenDangNhap, 115);
        DatHang("Mật khẩu", txtMatKhau, 155);
        DatHang("Nhập lại", txtNhapLai, 195);
        txtHoTen.MaxLength = 100;
        txtTenDangNhap.MaxLength = 50;

        btnTao.Text = "Tạo tài khoản";
        btnTao.SetBounds(200, 255, 120, 36);
        GiaoDien.NutChinh(btnTao);
        btnTao.Click += BtnTao_Click;

        btnThoat.Text = "Thoát";
        btnThoat.SetBounds(330, 255, 90, 36);
        btnThoat.DialogResult = DialogResult.Cancel;
        GiaoDien.NutPhu(btnThoat);

        Controls.Add(btnTao);
        Controls.Add(btnThoat);
        AcceptButton = btnTao;
        CancelButton = btnThoat;
    }

    private void DatHang(string nhan, Control oNhap, int y)
    {
        Controls.Add(new Label { Text = nhan, AutoSize = true, Location = new Point(20, y + 3) });
        oNhap.SetBounds(150, y, 270, 25);
        Controls.Add(oNhap);
    }

    private void BtnTao_Click(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            var nv = _service.TaoAdminDauTien(txtHoTen.Text, txtTenDangNhap.Text, txtMatKhau.Text, txtNhapLai.Text);
            Cursor = Cursors.Default;

            MessageBox.Show(this,
                $"Đã tạo tài khoản quản trị \"{nv.TenDangNhap}\".\nHãy đăng nhập bằng tài khoản này.",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (LoiNghiepVu ex)
        {
            MessageBox.Show(this, ex.Message, "Chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Lỗi hệ thống:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}