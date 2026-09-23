using CafePOS.Business;
using CafePOS.Business.Services;
using CafePOS.Data.Models;

namespace CafePOS.WinForms;

public partial class FormDangNhap : Form
{
    private readonly NhanVienService _service = new();

    private readonly TextBox txtTenDangNhap = new();
    private readonly TextBox txtMatKhau = new() { UseSystemPasswordChar = true };
    private readonly Button btnDangNhap = new();
    private readonly Button btnThoat = new();

    // Sau khi đăng nhập thành công, Program.cs lấy người dùng từ đây
    public NhanVien? NguoiDangNhap { get; private set; }

    public FormDangNhap()
    {
        InitializeComponent();
        DungGiaoDien();
        Shown += (s, e) => txtTenDangNhap.Focus();
    }

    private void DungGiaoDien()
    {
        Text = "CafePOS – Đăng nhập";
        Font = GiaoDien.FontChu;
        ClientSize = new Size(400, 250);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        Controls.Add(new Label
        {
            Text = "CafePOS",
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = GiaoDien.MauChinh,
            AutoSize = true,
            Location = new Point(18, 12)
        });
        Controls.Add(new Label
        {
            Text = "Đăng nhập để bắt đầu ca làm việc",
            ForeColor = Color.Gray,
            AutoSize = true,
            Location = new Point(22, 58)
        });

        DatHang("Tên đăng nhập", txtTenDangNhap, 100);
        DatHang("Mật khẩu", txtMatKhau, 140);

        btnDangNhap.Text = "Đăng nhập";
        btnDangNhap.SetBounds(190, 195, 110, 36);
        GiaoDien.NutChinh(btnDangNhap);
        btnDangNhap.Click += BtnDangNhap_Click;

        btnThoat.Text = "Thoát";
        btnThoat.SetBounds(310, 195, 70, 36);
        btnThoat.DialogResult = DialogResult.Cancel;
        GiaoDien.NutPhu(btnThoat);

        Controls.Add(btnDangNhap);
        Controls.Add(btnThoat);
        AcceptButton = btnDangNhap;
        CancelButton = btnThoat;
    }

    private void DatHang(string nhan, Control oNhap, int y)
    {
        Controls.Add(new Label { Text = nhan, AutoSize = true, Location = new Point(20, y + 3) });
        oNhap.SetBounds(140, y, 240, 25);
        Controls.Add(oNhap);
    }

    private void BtnDangNhap_Click(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            NguoiDangNhap = _service.DangNhap(txtTenDangNhap.Text, txtMatKhau.Text);
            DialogResult = DialogResult.OK;
        }
        catch (LoiNghiepVu ex)
        {
            Cursor = Cursors.Default;
            MessageBox.Show(this, ex.Message, "Không đăng nhập được",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtMatKhau.Clear();
            txtMatKhau.Focus();
        }
        catch (Exception ex)
        {
            Cursor = Cursors.Default;
            MessageBox.Show(this, "Lỗi hệ thống:\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}