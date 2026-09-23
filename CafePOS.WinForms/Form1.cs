using CafePOS.Business.Services;

namespace CafePOS.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Text = "CafePOS – Thực đơn";
        try
        {
            dgvMenu.DataSource = new MonService().LayMenu();
            DinhDangBang();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không tải được menu:\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    private void DinhDangBang()
    {
        dgvMenu.Columns["Id"].Visible = false;
        dgvMenu.Columns["Ten"].HeaderText = "Tên món";
        dgvMenu.Columns["DanhMuc"].HeaderText = "Danh mục";
        dgvMenu.Columns["SoLuongTon"].HeaderText = "Tồn kho";
        dgvMenu.Columns["SoLuongTon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvMenu.Columns["GiaBan"].HeaderText = "Giá bán (đ)";
        dgvMenu.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
        dgvMenu.Columns["GiaBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMenu.RowHeadersVisible = false;
        dgvMenu.BackgroundColor = Color.White;
        dgvMenu.Font = new Font("Segoe UI", 11);
    }
}