using CafePOS.Business;
using CafePOS.Business.Services;
using CafePOS.Data.Models;

namespace CafePOS.WinForms;

public partial class FormBaoCao : Form
{
    private readonly BaoCaoService _service = new();

    // Thanh chọn thời gian
    private readonly DateTimePicker dtpTu = new();
    private readonly DateTimePicker dtpDen = new();
    private readonly Button btnXem = new();
    private readonly Button btnHomNay = new();
    private readonly Button btn7Ngay = new();
    private readonly Button btnThangNay = new();

    // 3 con số tổng quan
    private readonly Label lblDoanhThu = new();
    private readonly Label lblSoHoaDon = new();
    private readonly Label lblTrungBinh = new();

    // 3 bảng chi tiết
    private readonly DataGridView dgvTheoMon = TaoBang();
    private readonly DataGridView dgvTheoNgay = TaoBang();
    private readonly DataGridView dgvTheoNhanVien = TaoBang();

    public FormBaoCao()
    {
        InitializeComponent();
        DungGiaoDien();
        Load += (s, e) => ChonKhoang(DateTime.Today, DateTime.Today);   // mở lên là xem "hôm nay"
    }

    // ===================== DỰNG GIAO DIỆN =====================

    private void DungGiaoDien()
    {
        Text = "CafePOS – Báo cáo doanh thu";
        Font = GiaoDien.FontChu;
        ClientSize = new Size(1000, 620);
        MinimumSize = new Size(900, 500);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;

        var pnlTren = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.FromArgb(245, 245, 245) };
        var pnlTongQuan = new Panel { Dock = DockStyle.Top, Height = 100 };
        var tab = new TabControl { Dock = DockStyle.Fill, Padding = new Point(12, 6) };

        tab.TabPages.Add(TaoTrang("Theo món", dgvTheoMon));
        tab.TabPages.Add(TaoTrang("Theo ngày", dgvTheoNgay));
        tab.TabPages.Add(TaoTrang("Theo nhân viên", dgvTheoNhanVien));

        // Vùng Fill thêm TRƯỚC, các vùng Top thêm sau (vùng thêm sau cùng nằm trên cùng)
        Controls.Add(tab);
        Controls.Add(pnlTongQuan);
        Controls.Add(pnlTren);

        // ── Bên trái thanh trên: chọn khoảng ngày ──
        pnlTren.Controls.Add(new Label { Text = "Từ", AutoSize = true, Location = new Point(15, 17) });
        dtpTu.Format = DateTimePickerFormat.Short;
        dtpTu.SetBounds(50, 14, 140, 27);

        pnlTren.Controls.Add(new Label { Text = "Đến", AutoSize = true, Location = new Point(205, 17) });
        dtpDen.Format = DateTimePickerFormat.Short;
        dtpDen.SetBounds(245, 14, 140, 27);

        btnXem.Text = "Xem";
        btnXem.SetBounds(400, 12, 90, 32);
        GiaoDien.NutChinh(btnXem);
        btnXem.Click += (s, e) => XemBaoCao();

        // ── Bên phải thanh trên: chọn nhanh ──
        int mepPhai = pnlTren.Width - 15;

        btnThangNay.Text = "Tháng này";
        btnThangNay.SetBounds(mepPhai - 100, 12, 100, 32);
        btnThangNay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutPhu(btnThangNay);
        btnThangNay.Click += (s, e) =>
            ChonKhoang(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);

        btn7Ngay.Text = "7 ngày";
        btn7Ngay.SetBounds(btnThangNay.Left - 10 - 90, 12, 90, 32);
        btn7Ngay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutPhu(btn7Ngay);
        btn7Ngay.Click += (s, e) => ChonKhoang(DateTime.Today.AddDays(-6), DateTime.Today);

        btnHomNay.Text = "Hôm nay";
        btnHomNay.SetBounds(btn7Ngay.Left - 10 - 90, 12, 90, 32);
        btnHomNay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.NutPhu(btnHomNay);
        btnHomNay.Click += (s, e) => ChonKhoang(DateTime.Today, DateTime.Today);

        pnlTren.Controls.AddRange(new Control[] { dtpTu, dtpDen, btnXem, btnHomNay, btn7Ngay, btnThangNay });
        AcceptButton = btnXem;

        // ── 3 thẻ tổng quan ──
        pnlTongQuan.Controls.Add(TaoThe("Doanh thu", lblDoanhThu, 15));
        pnlTongQuan.Controls.Add(TaoThe("Số hóa đơn", lblSoHoaDon, 280));
        pnlTongQuan.Controls.Add(TaoThe("Trung bình / hóa đơn", lblTrungBinh, 545));
    }

    private static Panel TaoThe(string tieuDe, Label lblGiaTri, int x)
    {
        var the = new Panel
        {
            Location = new Point(x, 15),
            Size = new Size(250, 72),
            BackColor = Color.FromArgb(240, 246, 255)
        };
        the.Controls.Add(new Label { Text = tieuDe, ForeColor = Color.Gray, AutoSize = true, Location = new Point(12, 8) });

        lblGiaTri.Text = "–";
        lblGiaTri.Font = new Font("Segoe UI", 18, FontStyle.Bold);
        lblGiaTri.ForeColor = GiaoDien.MauChinh;
        lblGiaTri.AutoSize = true;
        lblGiaTri.Location = new Point(10, 30);
        the.Controls.Add(lblGiaTri);

        return the;
    }

    private static TabPage TaoTrang(string ten, DataGridView dgv)
    {
        var trang = new TabPage(ten) { Padding = new Padding(8), BackColor = Color.White };
        trang.Controls.Add(dgv);
        return trang;
    }

    private static DataGridView TaoBang() => new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        RowHeadersVisible = false,
        MultiSelect = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = Color.White,
        BorderStyle = BorderStyle.None,
        Font = new Font("Segoe UI", 11),
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
    };

    // ===================== DỮ LIỆU =====================

    private void ChonKhoang(DateTime tu, DateTime den)
    {
        dtpTu.Value = tu;
        dtpDen.Value = den;
        XemBaoCao();
    }

    private void XemBaoCao()
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            var bc = _service.LayBaoCao(dtpTu.Value, dtpDen.Value);

            lblDoanhThu.Text = $"{bc.TongQuan.DoanhThu:N0} đ";
            lblSoHoaDon.Text = $"{bc.TongQuan.SoHoaDon:N0}";
            lblTrungBinh.Text = $"{bc.TongQuan.TrungBinhMoiHoaDon:N0} đ";

            dgvTheoMon.DataSource = bc.TheoMon;
            DinhDang(dgvTheoMon,
                (nameof(DoanhThuTheoMon.TenMon), "Món", null),
                (nameof(DoanhThuTheoMon.SoLuong), "Số lượng", "N0"),
                (nameof(DoanhThuTheoMon.DoanhThu), "Doanh thu (đ)", "N0"));

            dgvTheoNgay.DataSource = bc.TheoNgay;
            DinhDang(dgvTheoNgay,
                (nameof(DoanhThuTheoNgay.Ngay), "Ngày", "dd/MM/yyyy"),
                (nameof(DoanhThuTheoNgay.SoHoaDon), "Số hóa đơn", "N0"),
                (nameof(DoanhThuTheoNgay.DoanhThu), "Doanh thu (đ)", "N0"));

            dgvTheoNhanVien.DataSource = bc.TheoNhanVien;
            DinhDang(dgvTheoNhanVien,
                (nameof(DoanhThuTheoNhanVien.HoTen), "Nhân viên", null),
                (nameof(DoanhThuTheoNhanVien.SoHoaDon), "Số hóa đơn", "N0"),
                (nameof(DoanhThuTheoNhanVien.DoanhThu), "Doanh thu (đ)", "N0"));
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

    // Đặt tiêu đề và định dạng cho các cột. Cột số (N0) được căn phải.
    private static void DinhDang(DataGridView dgv, params (string Cot, string TieuDe, string? DinhDangSo)[] cacCot)
    {
        foreach (var (cot, tieuDe, dinhDangSo) in cacCot)
        {
            if (dgv.Columns[cot] is not { } c) continue;

            c.HeaderText = tieuDe;
            if (dinhDangSo != null)
                c.DefaultCellStyle.Format = dinhDangSo;
            if (dinhDangSo == "N0")
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }
}