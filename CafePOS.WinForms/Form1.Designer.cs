namespace CafePOS.WinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvMenu = new DataGridView();
            pnlTimKiem = new Panel();
            btnSua = new Button();
            btnThem = new Button();
            btnTimKiem = new Button();
            txtTimKiem = new TextBox();
            btnNgungBan = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvMenu).BeginInit();
            pnlTimKiem.SuspendLayout();
            SuspendLayout();
            // 
            // dgvMenu
            // 
            dgvMenu.AllowUserToAddRows = false;
            dgvMenu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMenu.Dock = DockStyle.Fill;
            dgvMenu.Location = new Point(0, 50);
            dgvMenu.Name = "dgvMenu";
            dgvMenu.ReadOnly = true;
            dgvMenu.Size = new Size(800, 400);
            dgvMenu.TabIndex = 0;
            dgvMenu.CellContentClick += dgvMenu_CellContentClick;
            // 
            // pnlTimKiem
            // 
            pnlTimKiem.Controls.Add(btnNgungBan);
            pnlTimKiem.Controls.Add(btnSua);
            pnlTimKiem.Controls.Add(btnThem);
            pnlTimKiem.Controls.Add(btnTimKiem);
            pnlTimKiem.Controls.Add(txtTimKiem);
            pnlTimKiem.Dock = DockStyle.Top;
            pnlTimKiem.Location = new Point(0, 0);
            pnlTimKiem.Name = "pnlTimKiem";
            pnlTimKiem.Size = new Size(800, 50);
            pnlTimKiem.TabIndex = 1;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(629, 14);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(75, 23);
            btnSua.TabIndex = 3;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(535, 14);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(75, 23);
            btnThem.TabIndex = 2;
            btnThem.Text = "+ Thêm món";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(441, 14);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(75, 23);
            btnTimKiem.TabIndex = 1;
            btnTimKiem.Text = "Tìm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(117, 14);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(300, 23);
            txtTimKiem.TabIndex = 0;
            // 
            // btnNgungBan
            // 
            btnNgungBan.Location = new Point(21, 14);
            btnNgungBan.Name = "btnNgungBan";
            btnNgungBan.Size = new Size(75, 23);
            btnNgungBan.TabIndex = 2;
            btnNgungBan.Text = "button1";
            btnNgungBan.UseVisualStyleBackColor = true;
            btnNgungBan.Click += btnNgungBan_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvMenu);
            Controls.Add(pnlTimKiem);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMenu).EndInit();
            pnlTimKiem.ResumeLayout(false);
            pnlTimKiem.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvMenu;
        private Panel pnlTimKiem;
        private Button btnTimKiem;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnNgungBan;
    }
}
