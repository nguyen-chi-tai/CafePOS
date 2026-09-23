using CafePOS.Business.Services;
using CafePOS.Data.Models;

namespace CafePOS.WinForms;

public partial class Form1 : Form
{
    private readonly MonService _monService = new();

    public Form1()
    {
        InitializeComponent();
        ThietKeGiaoDien();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Text = "CafePOS – Thực đơn";
        TaiLai();
    }

    private void TaiLai()
    {
        try
        {
            dgvMenu.DataSource = _monService.TimMon(txtTimKiem.Text);
            DinhDangBang();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Không tải được dữ liệu:\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ===== Các nút bấm =====

    private void btnTimKiem_Click(object sender, EventArgs e) => TaiLai();

    private void btnThem_Click(object sender, EventArgs e)
    {
        using var form = new FormMon(null);
        if (form.ShowDialog(this) == DialogResult.OK)
            TaiLai();
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (dgvMenu.CurrentRow?.DataBoundItem is not Mon mon)
        {
            MessageBox.Show(this, "Hãy chọn một món trong bảng để sửa.", "Chưa chọn món",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new FormMon(mon);
        if (form.ShowDialog(this) == DialogResult.OK)
            TaiLai();
    }

    // MỚI: ngừng bán món đang chọn
    private void btnNgungBan_Click(object sender, EventArgs e)
    {
        if (dgvMenu.CurrentRow?.DataBoundItem is not Mon mon)
        {
            MessageBox.Show(this, "Hãy chọn một món trong bảng.", "Chưa chọn món",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var xacNhan = MessageBox.Show(this,
            $"Ngừng bán \"{mon.Ten}\"?\n\nMón sẽ biến khỏi thực đơn, nhưng lịch sử bán hàng vẫn được giữ nguyên.",
            "Xác nhận ngừng bán", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (xacNhan != DialogResult.Yes) return;

        try
        {
            _monService.NgungBan(mon.Id);
            TaiLai();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ===== Định dạng bảng =====

    private DataGridViewColumn Cot(string ten) =>
        dgvMenu.Columns[ten] ?? throw new InvalidOperationException($"Bảng không có cột '{ten}'.");

    private void DinhDangBang()
    {
        Cot(nameof(Mon.Id)).Visible = false;
        Cot(nameof(Mon.DanhMucId)).Visible = false;
        Cot(nameof(Mon.Ten)).HeaderText = "Tên món";
        Cot(nameof(Mon.DanhMuc)).HeaderText = "Danh mục";
        Cot(nameof(Mon.SoLuongTon)).HeaderText = "Tồn kho";
        Cot(nameof(Mon.SoLuongTon)).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        Cot(nameof(Mon.GiaBan)).HeaderText = "Giá bán (đ)";
        Cot(nameof(Mon.GiaBan)).DefaultCellStyle.Format = "N0";
        Cot(nameof(Mon.GiaBan)).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMenu.RowHeadersVisible = false;
        dgvMenu.BackgroundColor = Color.White;
        dgvMenu.Font = new Font("Segoe UI", 11);
    }

    // ===== Bố cục giao diện =====

    private void ThietKeGiaoDien()
    {
        // Khung cửa sổ
        Font = GiaoDien.FontChu;
        ClientSize = new Size(900, 520);
        MinimumSize = new Size(750, 400);
        StartPosition = FormStartPosition.CenterScreen;

        // Thanh công cụ phía trên
        pnlTimKiem.Height = 56;
        pnlTimKiem.BackColor = Color.FromArgb(245, 245, 245);

        // Bên trái: tìm kiếm
        txtTimKiem.SetBounds(15, 14, 300, 27);
        txtTimKiem.PlaceholderText = "Tìm theo tên món...";
        btnTimKiem.Text = "Tìm";
        btnTimKiem.SetBounds(325, 12, 80, 32);
        GiaoDien.NutPhu(btnTimKiem);
        AcceptButton = btnTimKiem;

        // Bên phải, xếp từ phải sang trái: [+ Thêm món] [Sửa] [Ngừng bán]
        int mepPhai = ClientSize.Width - 15;

        btnNgungBan.Text = "Ngừng bán";
        btnNgungBan.SetBounds(mepPhai - 100, 12, 100, 32);
        btnNgungBan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutNguyHiem(btnNgungBan);

        btnSua.Text = "Sửa";
        btnSua.SetBounds(btnNgungBan.Left - 10 - 80, 12, 80, 32);
        btnSua.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutPhu(btnSua);

        btnThem.Text = "+ Thêm món";
        btnThem.SetBounds(btnSua.Left - 10 - 120, 12, 120, 32);
        btnThem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutChinh(btnThem);
    }

    // Được file Designer nối vào, giữ lại dù rỗng
    private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
}