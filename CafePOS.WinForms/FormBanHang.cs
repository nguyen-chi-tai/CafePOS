using CafePOS.Business;
using CafePOS.Business.Models;
using CafePOS.Business.Services;
using CafePOS.Data.Models;

namespace CafePOS.WinForms;

public partial class FormBanHang : Form
{
    private readonly MonService _monService = new();
    private readonly BanHangService _banHangService = new();
    private readonly DonHang _donHang = new();

    // Các điều khiển được tạo bằng code
    private readonly FlowLayoutPanel flpMon = new();
    private readonly DataGridView dgvDonHang = new();
    private readonly Label lblTong = new();
    private readonly Button btnThanhToan = new();
    private readonly Button btnHuyDon = new();
    private readonly Button btnBotMot = new();
    private readonly Button btnXoaMon = new();
    private readonly Button btnQuanLyThucDon = new();

    public FormBanHang()
    {
        InitializeComponent();
        DungGiaoDien();
        Load += FormBanHang_Load;
    }

    private void FormBanHang_Load(object? sender, EventArgs e)
    {
        TaiMenu();
        CapNhatDonHang();
    }

    // ===================== DỰNG GIAO DIỆN =====================

    private void DungGiaoDien()
    {
        Text = "CafePOS – Bán hàng";
        Font = GiaoDien.FontChu;
        ClientSize = new Size(1100, 650);
        MinimumSize = new Size(900, 550);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.White;

        // 3 vùng lớn. Thứ tự Add quan trọng: vùng Fill phải được thêm TRƯỚC
        var pnlTren = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.FromArgb(245, 245, 245) };
        var pnlDonHang = new Panel { Dock = DockStyle.Right, Width = 400, Padding = new Padding(15), BackColor = Color.White };
        flpMon.Dock = DockStyle.Fill;
        flpMon.AutoScroll = true;
        flpMon.Padding = new Padding(10);
        flpMon.BackColor = Color.FromArgb(250, 250, 250);

        Controls.Add(flpMon);
        Controls.Add(pnlDonHang);
        Controls.Add(pnlTren);

        // ── Thanh trên ──
        var lblTieuDe = new Label
        {
            Text = "Bán hàng",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 13)
        };
        pnlTren.Controls.Add(lblTieuDe);

        btnQuanLyThucDon.Text = "Quản lý thực đơn";
        btnQuanLyThucDon.SetBounds(pnlTren.Width - 15 - 160, 12, 160, 32);
        btnQuanLyThucDon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutPhu(btnQuanLyThucDon);
        btnQuanLyThucDon.Click += BtnQuanLyThucDon_Click;
        pnlTren.Controls.Add(btnQuanLyThucDon);

        // ── Cột đơn hàng bên phải ──
        var lblDonHang = new Label
        {
            Text = "ĐƠN HÀNG",
            Dock = DockStyle.Top,
            Height = 36,
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };

        dgvDonHang.Dock = DockStyle.Fill;
        dgvDonHang.ReadOnly = true;
        dgvDonHang.AllowUserToAddRows = false;
        dgvDonHang.RowHeadersVisible = false;
        dgvDonHang.MultiSelect = false;
        dgvDonHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDonHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDonHang.BackgroundColor = Color.White;
        dgvDonHang.BorderStyle = BorderStyle.None;
        dgvDonHang.Font = new Font("Segoe UI", 11);
        dgvDonHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

        var pnlDuoi = new Panel { Dock = DockStyle.Bottom, Height = 155 };

        pnlDonHang.Controls.Add(dgvDonHang);
        pnlDonHang.Controls.Add(pnlDuoi);
        pnlDonHang.Controls.Add(lblDonHang);

        // ── Các nút và tổng tiền dưới đơn hàng ──
        btnBotMot.Text = "− Bớt 1";
        btnBotMot.SetBounds(0, 10, 110, 34);
        GiaoDien.NutPhu(btnBotMot);
        btnBotMot.Click += BtnBotMot_Click;

        btnXoaMon.Text = "Xóa món";
        btnXoaMon.SetBounds(120, 10, 110, 34);
        GiaoDien.NutNguyHiem(btnXoaMon);
        btnXoaMon.Click += BtnXoaMon_Click;

        lblTong.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        lblTong.TextAlign = ContentAlignment.MiddleRight;
        lblTong.SetBounds(0, 55, 370, 40);

        btnHuyDon.Text = "Hủy đơn";
        btnHuyDon.SetBounds(0, 108, 120, 42);
        GiaoDien.NutPhu(btnHuyDon);
        btnHuyDon.Click += BtnHuyDon_Click;

        btnThanhToan.Text = "THANH TOÁN";
        btnThanhToan.SetBounds(130, 108, 240, 42);
        btnThanhToan.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        GiaoDien.NutChinh(btnThanhToan);
        btnThanhToan.Click += BtnThanhToan_Click;

        pnlDuoi.Controls.AddRange(new Control[] { btnBotMot, btnXoaMon, lblTong, btnHuyDon, btnThanhToan });
    }

    // ===================== THỰC ĐƠN (Ô MÓN BÊN TRÁI) =====================

    private void TaiMenu()
    {
        try
        {
            flpMon.SuspendLayout();

            // Hủy các ô cũ trước khi tạo ô mới (giải phóng bộ nhớ)
            foreach (var o in flpMon.Controls.Cast<Control>().ToList())
                o.Dispose();

            foreach (var mon in _monService.TimMon(""))
                flpMon.Controls.Add(TaoOMon(mon));
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Không tải được thực đơn:\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            flpMon.ResumeLayout();
        }
    }

    private Button TaoOMon(Mon mon)
    {
        var nut = new Button
        {
            Text = $"{mon.Ten}\n{mon.GiaBan:N0} đ\nCòn {mon.SoLuongTon}",
            Size = new Size(160, 100),
            Margin = new Padding(8),
            Enabled = mon.SoLuongTon > 0
        };
        GiaoDien.NutPhu(nut);
        nut.Click += (s, e) => ThemVaoDon(mon);
        return nut;
    }

    private void ThemVaoDon(Mon mon)
    {
        try
        {
            _donHang.ThemMon(mon);
            CapNhatDonHang();
        }
        catch (LoiNghiepVu ex)
        {
            MessageBox.Show(this, ex.Message, "Không thể thêm",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ===================== ĐƠN HÀNG (BÊN PHẢI) =====================

    private void CapNhatDonHang()
    {
        dgvDonHang.DataSource = _donHang.CacDong.ToList();
        DinhDangBangDon();

        lblTong.Text = $"TỔNG:  {_donHang.TongTien:N0} đ";

        bool coMon = !_donHang.Trong;
        btnThanhToan.Enabled = coMon;
        btnHuyDon.Enabled = coMon;
        btnBotMot.Enabled = coMon;
        btnXoaMon.Enabled = coMon;
        btnThanhToan.BackColor = coMon ? GiaoDien.MauChinh : Color.FromArgb(210, 210, 210);
    }

    private DataGridViewColumn Cot(string ten) =>
        dgvDonHang.Columns[ten] ?? throw new InvalidOperationException($"Bảng không có cột '{ten}'.");

    private void DinhDangBangDon()
    {
        if (dgvDonHang.Columns.Count == 0) return;

        Cot(nameof(DongDonHang.MonId)).Visible = false;
        Cot(nameof(DongDonHang.DonGia)).Visible = false;

        Cot(nameof(DongDonHang.TenMon)).HeaderText = "Món";
        Cot(nameof(DongDonHang.TenMon)).FillWeight = 55;

        Cot(nameof(DongDonHang.SoLuong)).HeaderText = "SL";
        Cot(nameof(DongDonHang.SoLuong)).FillWeight = 15;
        Cot(nameof(DongDonHang.SoLuong)).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        Cot(nameof(DongDonHang.ThanhTien)).HeaderText = "Thành tiền";
        Cot(nameof(DongDonHang.ThanhTien)).FillWeight = 30;
        Cot(nameof(DongDonHang.ThanhTien)).DefaultCellStyle.Format = "N0";
        Cot(nameof(DongDonHang.ThanhTien)).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    private DongDonHang? DongDangChon() => dgvDonHang.CurrentRow?.DataBoundItem as DongDonHang;

    // ===================== CÁC NÚT BẤM =====================

    private void BtnBotMot_Click(object? sender, EventArgs e)
    {
        if (DongDangChon() is not { } dong) return;
        _donHang.BotMot(dong.MonId);
        CapNhatDonHang();
    }

    private void BtnXoaMon_Click(object? sender, EventArgs e)
    {
        if (DongDangChon() is not { } dong) return;
        _donHang.XoaMon(dong.MonId);
        CapNhatDonHang();
    }

    private void BtnHuyDon_Click(object? sender, EventArgs e)
    {
        var xacNhan = MessageBox.Show(this, "Hủy toàn bộ đơn hàng đang gọi?", "Xác nhận hủy đơn",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        if (xacNhan != DialogResult.Yes) return;

        _donHang.XoaHet();
        CapNhatDonHang();
    }

    private void BtnThanhToan_Click(object? sender, EventArgs e)
    {
        try
        {
            var ketQua = _banHangService.ThanhToan(_donHang);

            MessageBox.Show(this,
                $"Đã thanh toán hóa đơn #{ketQua.HoaDonId}\n\nTổng tiền: {ketQua.TongTien:N0} đ",
                "Thanh toán thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _donHang.XoaHet();
        }
        catch (LoiNghiepVu ex)
        {
            MessageBox.Show(this, ex.Message, "Không thể thanh toán",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Lỗi hệ thống:\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // Dù thành công hay thất bại, tải lại thực đơn để thấy tồn kho mới nhất
            TaiMenu();
            CapNhatDonHang();
        }
    }

    private void BtnQuanLyThucDon_Click(object? sender, EventArgs e)
    {
        using var form = new Form1();
        form.ShowDialog(this);
        TaiMenu();
    }
}