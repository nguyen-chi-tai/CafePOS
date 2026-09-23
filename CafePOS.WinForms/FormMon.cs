using CafePOS.Business;
using CafePOS.Business.Services;
using CafePOS.Data.Models;

namespace CafePOS.WinForms;

public partial class FormMon : Form
{
    private readonly Mon? _mon;
    private readonly MonService _monService = new();
    private readonly DanhMucService _danhMucService = new();

    public FormMon(Mon? mon)
    {
        InitializeComponent();
        _mon = mon;
        ThietKeGiaoDien();
    }

    private void FormMon_Load(object sender, EventArgs e)
    {
        Text = _mon == null ? "Thêm món mới" : "Sửa món";

        cboDanhMuc.DisplayMember = nameof(DanhMuc.Ten);
        cboDanhMuc.ValueMember = nameof(DanhMuc.Id);
        cboDanhMuc.DataSource = _danhMucService.LayTatCa();

        if (_mon != null)
        {
            txtTen.Text = _mon.Ten;
            cboDanhMuc.SelectedValue = _mon.DanhMucId;
            nudGiaBan.Value = _mon.GiaBan;
            nudSoLuong.Value = _mon.SoLuongTon;
        }
    }

    private void btnLuu_Click(object sender, EventArgs e)
    {
        var mon = new Mon
        {
            Id = _mon?.Id ?? 0,
            Ten = txtTen.Text,
            DanhMucId = cboDanhMuc.SelectedValue is int id ? id : 0,
            GiaBan = nudGiaBan.Value,
            SoLuongTon = (int)nudSoLuong.Value
        };

        try
        {
            _monService.Luu(mon);
            DialogResult = DialogResult.OK;
        }
        catch (LoiNghiepVu ex)
        {
            MessageBox.Show(ex.Message, "Dữ liệu chưa hợp lệ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không lưu được:\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void ThietKeGiaoDien()
    {
        // Khung form
        Font = GiaoDien.FontChu;
        ClientSize = new Size(410, 250);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        // 4 hàng nhập liệu, mỗi hàng cách nhau 40 pixel
        DatHang("Tên món", txtTen, 25);
        DatHang("Danh mục", cboDanhMuc, 65);
        DatHang("Giá bán (đ)", nudGiaBan, 105);
        DatHang("Tồn kho", nudSoLuong, 145);

        // Thiết lập từng ô
        txtTen.Text = "";
        txtTen.MaxLength = 100;
        cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
        nudGiaBan.Minimum = 0;
        nudGiaBan.Maximum = 10_000_000;
        nudGiaBan.Increment = 1000;
        nudGiaBan.ThousandsSeparator = true;
        nudGiaBan.Value = 0;
        nudSoLuong.Minimum = 0;
        nudSoLuong.Maximum = 100_000;
        nudSoLuong.ThousandsSeparator = true;

        // 2 nút, căn phải
        btnLuu.Text = "Lưu";
        btnLuu.SetBounds(200, 195, 90, 34);
        GiaoDien.NutChinh(btnLuu);

        btnHuy.Text = "Hủy";
        btnHuy.SetBounds(300, 195, 90, 34);
        btnHuy.DialogResult = DialogResult.Cancel;
        GiaoDien.NutPhu(btnHuy);

        // Enter = Lưu, Esc = Hủy
        AcceptButton = btnLuu;
        CancelButton = btnHuy;
    }

    // Tạo một hàng: nhãn bên trái (X = 20), ô nhập bên phải (X = 130)
    private void DatHang(string nhan, Control oNhap, int y)
    {
        var lbl = new Label { Text = nhan, AutoSize = true, Location = new Point(20, y + 3) };
        Controls.Add(lbl);
        oNhap.MaximumSize = Size.Empty;
        oNhap.MinimumSize = Size.Empty;
        oNhap.SetBounds(130, y, 260, 25);
    }
}