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
        try
        {
            dgvMenu.DataSource = new MonService().LayMenu();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không tải được menu:\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}