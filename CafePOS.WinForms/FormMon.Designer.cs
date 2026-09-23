namespace CafePOS.WinForms
{
    partial class FormMon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            nudGiaBan = new NumericUpDown();
            nudSoLuong = new NumericUpDown();
            txtTen = new TextBox();
            btnHuy = new Button();
            btnLuu = new Button();
            cboDanhMuc = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)nudGiaBan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).BeginInit();
            SuspendLayout();
            // 
            // nudGiaBan
            // 
            nudGiaBan.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            nudGiaBan.Location = new Point(64, 66);
            nudGiaBan.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            nudGiaBan.MaximumSize = new Size(121, 0);
            nudGiaBan.Minimum = new decimal(new int[] { 10000000, 0, 0, 0 });
            nudGiaBan.Name = "nudGiaBan";
            nudGiaBan.Size = new Size(121, 23);
            nudGiaBan.TabIndex = 5;
            nudGiaBan.Tag = "Giá bán (đ)";
            nudGiaBan.ThousandsSeparator = true;
            nudGiaBan.Value = new decimal(new int[] { 10000000, 0, 0, 0 });
            // 
            // nudSoLuong
            // 
            nudSoLuong.AccessibleName = "";
            nudSoLuong.Location = new Point(73, 176);
            nudSoLuong.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudSoLuong.Name = "nudSoLuong";
            nudSoLuong.Size = new Size(220, 23);
            nudSoLuong.TabIndex = 6;
            nudSoLuong.Tag = "Tồn kho";
            // 
            // txtTen
            // 
            txtTen.Location = new Point(24, 24);
            txtTen.MaximumSize = new Size(200, 0);
            txtTen.MaxLength = 100;
            txtTen.Name = "txtTen";
            txtTen.Size = new Size(100, 23);
            txtTen.TabIndex = 3;
            txtTen.Text = "Tên món";
            // 
            // btnHuy
            // 
            btnHuy.AccessibleName = "";
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.Location = new Point(288, 136);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(75, 23);
            btnHuy.TabIndex = 8;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.AccessibleName = "";
            btnLuu.Location = new Point(302, 91);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(75, 23);
            btnLuu.TabIndex = 7;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            btnLuu.Click += btnLuu_Click;
            // 
            // cboDanhMuc
            // 
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDanhMuc.DropDownWidth = 220;
            cboDanhMuc.FormattingEnabled = true;
            cboDanhMuc.Location = new Point(171, 24);
            cboDanhMuc.Name = "cboDanhMuc";
            cboDanhMuc.Size = new Size(220, 23);
            cboDanhMuc.TabIndex = 4;
            // 
            // FormMon
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(404, 261);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(nudSoLuong);
            Controls.Add(nudGiaBan);
            Controls.Add(cboDanhMuc);
            Controls.Add(txtTen);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormMon";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FormMon";
            Load += FormMon_Load;
            ((System.ComponentModel.ISupportInitialize)nudGiaBan).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private NumericUpDown nudGiaBan;
        private NumericUpDown nudSoLuong;
        private TextBox txtTen;
        private Button btnHuy;
        private Button btnLuu;
        private ComboBox cboDanhMuc;
    }
}