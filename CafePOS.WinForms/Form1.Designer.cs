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
            btnTimKiem = new Button();
            txtTimKiem = new TextBox();
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
            pnlTimKiem.Controls.Add(btnTimKiem);
            pnlTimKiem.Controls.Add(txtTimKiem);
            pnlTimKiem.Dock = DockStyle.Top;
            pnlTimKiem.Location = new Point(0, 0);
            pnlTimKiem.Name = "pnlTimKiem";
            pnlTimKiem.Size = new Size(800, 50);
            pnlTimKiem.TabIndex = 1;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(401, 16);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(75, 23);
            btnTimKiem.TabIndex = 1;
            btnTimKiem.Text = "Tìm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(77, 16);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(300, 23);
            txtTimKiem.TabIndex = 0;
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
    }
}
