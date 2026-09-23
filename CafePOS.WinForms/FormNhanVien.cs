using CafePOS.Business;
using CafePOS.Business.Services;
using CafePOS.Data.Models;

namespace CafePOS.WinForms;

public partial class FormNhanVien : Form
{
    private readonly NhanVienService _service = new();

    private readonly DataGridView dgvNhanVien = new();
    private readonly Button btnThem = new();
    private readonly Button btnDatMatKhau = new();
    private readonly Button btnDoiTrangThai = new();

    public FormNhanVien()
    {
        InitializeComponent();
        DungGiaoDien();
        Load += (s, e) => TaiLai();
    }

    // ===================== DỰNG GIAO DIỆN =====================

    private void DungGiaoDien()
    {
        Text = "CafePOS – Nhân viên";
        Font = GiaoDien.FontChu;
        ClientSize = new Size(820, 460);
        MinimumSize = new Size(760, 350);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;

        var pnlTren = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.FromArgb(245, 245, 245) };

        dgvNhanVien.Dock = DockStyle.Fill;
        dgvNhanVien.ReadOnly = true;
        dgvNhanVien.AllowUserToAddRows = false;
        dgvNhanVien.RowHeadersVisible = false;
        dgvNhanVien.MultiSelect = false;
        dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvNhanVien.BackgroundColor = Color.White;
        dgvNhanVien.BorderStyle = BorderStyle.None;
        dgvNhanVien.Font = new Font("Segoe UI", 11);
        dgvNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvNhanVien.CellFormatting += DgvNhanVien_CellFormatting;
        dgvNhanVien.SelectionChanged += (s, e) => CapNhatNutTrangThai();

        Controls.Add(dgvNhanVien);   // vùng Fill thêm trước
        Controls.Add(pnlTren);

        pnlTren.Controls.Add(new Label
        {
            Text = "Nhân viên",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 13)
        });

        // Bên phải, từ phải sang trái: [+ Thêm thu ngân] [Đặt lại mật khẩu] [Vô hiệu hóa]
        int mepPhai = pnlTren.Width - 15;

        btnDoiTrangThai.Text = "Vô hiệu hóa";
        btnDoiTrangThai.SetBounds(mepPhai - 130, 12, 130, 32);
        btnDoiTrangThai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutNguyHiem(btnDoiTrangThai);
        btnDoiTrangThai.Click += BtnDoiTrangThai_Click;

        btnDatMatKhau.Text = "Đặt lại mật khẩu";
        btnDatMatKhau.SetBounds(btnDoiTrangThai.Left - 10 - 150, 12, 150, 32);
        btnDatMatKhau.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutPhu(btnDatMatKhau);
        btnDatMatKhau.Click += BtnDatMatKhau_Click;

        btnThem.Text = "+ Thêm thu ngân";
        btnThem.SetBounds(btnDatMatKhau.Left - 10 - 150, 12, 150, 32);
        btnThem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutChinh(btnThem);
        btnThem.Click += BtnThem_Click;

        pnlTren.Controls.AddRange(new Control[] { btnThem, btnDatMatKhau, btnDoiTrangThai });
    }

    // ===================== DỮ LIỆU =====================

    private void TaiLai()
    {
        try
        {
            dgvNhanVien.DataSource = _service.LayDanhSach();
            DinhDangBang();
            CapNhatNutTrangThai();
        }
        catch (LoiNghiepVu ex)
        {
            MessageBox.Show(this, ex.Message, "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Lỗi hệ thống:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private DataGridViewColumn Cot(string ten) =>
        dgvNhanVien.Columns[ten] ?? throw new InvalidOperationException($"Bảng không có cột '{ten}'.");

    private void DinhDangBang()
    {
        if (dgvNhanVien.Columns.Count == 0) return;

        Cot(nameof(NhanVien.Id)).Visible = false;
        Cot(nameof(NhanVien.MatKhauHash)).Visible = false;
        Cot(nameof(NhanVien.HoTen)).HeaderText = "Họ tên";
        Cot(nameof(NhanVien.TenDangNhap)).HeaderText = "Tên đăng nhập";
        Cot(nameof(NhanVien.VaiTro)).HeaderText = "Vai trò";
        Cot(nameof(NhanVien.DangLamViec)).HeaderText = "Đang làm việc";

        // Tài khoản đã vô hiệu hóa: chữ xám
        foreach (DataGridViewRow dong in dgvNhanVien.Rows)
            if (dong.DataBoundItem is NhanVien { DangLamViec: false })
                dong.DefaultCellStyle.ForeColor = Color.Gray;
    }

    // Hiển thị vai trò bằng tiếng Việt, dữ liệu gốc trong database vẫn giữ nguyên
    private void DgvNhanVien_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvNhanVien.Columns[e.ColumnIndex].Name == nameof(NhanVien.VaiTro) && e.Value is string vaiTro)
        {
            e.Value = vaiTro == "Admin" ? "Quản trị" : "Thu ngân";
            e.FormattingApplied = true;
        }
    }

    private NhanVien? NhanVienDangChon() => dgvNhanVien.CurrentRow?.DataBoundItem as NhanVien;

    // Nút đổi chữ theo trạng thái nhân viên đang chọn
    private void CapNhatNutTrangThai()
    {
        if (NhanVienDangChon() is { } nv)
            btnDoiTrangThai.Text = nv.DangLamViec ? "Vô hiệu hóa" : "Kích hoạt lại";
    }

    // ===================== CÁC NÚT BẤM =====================

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        using var form = new FormTaiKhoan(null);
        if (form.ShowDialog(this) != DialogResult.OK) return;

        TaiLai();
        MessageBox.Show(this, "Đã tạo tài khoản thu ngân.", "Thành công",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnDatMatKhau_Click(object? sender, EventArgs e)
    {
        if (NhanVienDangChon() is not { } nv)
        {
            MessageBox.Show(this, "Hãy chọn một nhân viên.", "Chưa chọn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new FormTaiKhoan(nv);
        if (form.ShowDialog(this) != DialogResult.OK) return;

        MessageBox.Show(this, $"Đã đặt mật khẩu mới cho \"{nv.HoTen}\".", "Thành công",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnDoiTrangThai_Click(object? sender, EventArgs e)
    {
        if (NhanVienDangChon() is not { } nv)
        {
            MessageBox.Show(this, "Hãy chọn một nhân viên.", "Chưa chọn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        bool trangThaiMoi = !nv.DangLamViec;
        string hanhDong = trangThaiMoi ? "Kích hoạt lại" : "Vô hiệu hóa";

        var xacNhan = MessageBox.Show(this, $"{hanhDong} tài khoản \"{nv.HoTen}\"?", "Xác nhận",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        if (xacNhan != DialogResult.Yes) return;

        try
        {
            _service.DoiTrangThai(nv.Id, trangThaiMoi);
            TaiLai();
        }
        catch (LoiNghiepVu ex)
        {
            MessageBox.Show(this, ex.Message, "Không thể thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}