
namespace VetCare
{
    partial class MuayeneKaydiSayfasi
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlFiltre = new System.Windows.Forms.Panel();
            this.btnYenile = new System.Windows.Forms.Button();
            this.txtAra = new System.Windows.Forms.TextBox();
            this.lblAra = new System.Windows.Forms.Label();
            this.pnlListe = new System.Windows.Forms.Panel();
            this.dgvMuayeneler = new System.Windows.Forms.DataGridView();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnTemizle = new System.Windows.Forms.Button();
            this.btnGuncelle = new System.Windows.Forms.Button();
            this.btnEkle = new System.Windows.Forms.Button();
            this.dtpMuayene = new System.Windows.Forms.DateTimePicker();
            this.txtNotlar = new System.Windows.Forms.TextBox();
            this.txtTeshis = new System.Windows.Forms.TextBox();
            this.txtSikayet = new System.Windows.Forms.TextBox();
            this.lblNotlar = new System.Windows.Forms.Label();
            this.lblTeshis = new System.Windows.Forms.Label();
            this.lblSikayet = new System.Windows.Forms.Label();
            this.lblTarih = new System.Windows.Forms.Label();
            this.cmbHayvan = new System.Windows.Forms.ComboBox();
            this.lblHayvan = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlFiltre.SuspendLayout();
            this.pnlListe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMuayeneler)).BeginInit();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFiltre
            // 
            this.pnlFiltre.BackColor = System.Drawing.Color.White;
            this.pnlFiltre.Controls.Add(this.btnYenile);
            this.pnlFiltre.Controls.Add(this.txtAra);
            this.pnlFiltre.Controls.Add(this.lblAra);
            this.pnlFiltre.Location = new System.Drawing.Point(20, 18);
            this.pnlFiltre.Name = "pnlFiltre";
            this.pnlFiltre.Size = new System.Drawing.Size(904, 54);
            this.pnlFiltre.TabIndex = 0;
            // 
            // btnYenile
            // 
            this.btnYenile.BackColor = System.Drawing.Color.White;
            this.btnYenile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(127)))), ((int)(((byte)(110)))));
            this.btnYenile.FlatAppearance.BorderSize = 2;
            this.btnYenile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYenile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYenile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(127)))), ((int)(((byte)(110)))));
            this.btnYenile.Location = new System.Drawing.Point(495, 10);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(105, 34);
            this.btnYenile.TabIndex = 2;
            this.btnYenile.Text = "Yenile";
            this.btnYenile.UseVisualStyleBackColor = false;
            // 
            // txtAra
            // 
            this.txtAra.Location = new System.Drawing.Point(165, 13);
            this.txtAra.Name = "txtAra";
            this.txtAra.Size = new System.Drawing.Size(310, 20);
            this.txtAra.TabIndex = 1;
            // 
            // lblAra
            // 
            this.lblAra.AutoSize = true;
            this.lblAra.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.lblAra.Location = new System.Drawing.Point(15, 18);
            this.lblAra.Name = "lblAra";
            this.lblAra.Size = new System.Drawing.Size(128, 17);
            this.lblAra.TabIndex = 0;
            this.lblAra.Text = "Hayvan / sahip ara:";
            // 
            // pnlListe
            // 
            this.pnlListe.BackColor = System.Drawing.Color.White;
            this.pnlListe.Controls.Add(this.dgvMuayeneler);
            this.pnlListe.Location = new System.Drawing.Point(20, 82);
            this.pnlListe.Name = "pnlListe";
            this.pnlListe.Size = new System.Drawing.Size(904, 230);
            this.pnlListe.TabIndex = 1;
            // 
            // dgvMuayeneler
            // 
            this.dgvMuayeneler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMuayeneler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMuayeneler.Location = new System.Drawing.Point(0, 0);
            this.dgvMuayeneler.Name = "dgvMuayeneler";
            this.dgvMuayeneler.Size = new System.Drawing.Size(904, 230);
            this.dgvMuayeneler.TabIndex = 0;
            // 
            // pnlForm
            // 
            this.pnlForm.BackColor = System.Drawing.Color.White;
            this.pnlForm.Controls.Add(this.label1);
            this.pnlForm.Controls.Add(this.btnSil);
            this.pnlForm.Controls.Add(this.btnTemizle);
            this.pnlForm.Controls.Add(this.btnGuncelle);
            this.pnlForm.Controls.Add(this.btnEkle);
            this.pnlForm.Controls.Add(this.dtpMuayene);
            this.pnlForm.Controls.Add(this.txtNotlar);
            this.pnlForm.Controls.Add(this.txtTeshis);
            this.pnlForm.Controls.Add(this.txtSikayet);
            this.pnlForm.Controls.Add(this.lblNotlar);
            this.pnlForm.Controls.Add(this.lblTeshis);
            this.pnlForm.Controls.Add(this.lblSikayet);
            this.pnlForm.Controls.Add(this.lblTarih);
            this.pnlForm.Controls.Add(this.cmbHayvan);
            this.pnlForm.Controls.Add(this.lblHayvan);
            this.pnlForm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlForm.Location = new System.Drawing.Point(20, 322);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(904, 275);
            this.pnlForm.TabIndex = 2;
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
            this.btnSil.FlatAppearance.BorderSize = 0;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(588, 217);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(85, 29);
            this.btnSil.TabIndex = 5;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            // 
            // btnTemizle
            // 
            this.btnTemizle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.btnTemizle.FlatAppearance.BorderSize = 2;
            this.btnTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTemizle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTemizle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.btnTemizle.Location = new System.Drawing.Point(688, 217);
            this.btnTemizle.Name = "btnTemizle";
            this.btnTemizle.Size = new System.Drawing.Size(100, 29);
            this.btnTemizle.TabIndex = 5;
            this.btnTemizle.Text = "Temizle";
            this.btnTemizle.UseVisualStyleBackColor = true;
            // 
            // btnGuncelle
            // 
            this.btnGuncelle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.btnGuncelle.FlatAppearance.BorderSize = 0;
            this.btnGuncelle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuncelle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGuncelle.ForeColor = System.Drawing.Color.White;
            this.btnGuncelle.Location = new System.Drawing.Point(748, 177);
            this.btnGuncelle.Name = "btnGuncelle";
            this.btnGuncelle.Size = new System.Drawing.Size(120, 34);
            this.btnGuncelle.TabIndex = 5;
            this.btnGuncelle.Text = "Güncelle";
            this.btnGuncelle.UseVisualStyleBackColor = false;
            // 
            // btnEkle
            // 
            this.btnEkle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(127)))), ((int)(((byte)(110)))));
            this.btnEkle.FlatAppearance.BorderSize = 0;
            this.btnEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEkle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEkle.ForeColor = System.Drawing.Color.White;
            this.btnEkle.Location = new System.Drawing.Point(588, 177);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(150, 34);
            this.btnEkle.TabIndex = 5;
            this.btnEkle.Text = "Muayene Ekle";
            this.btnEkle.UseVisualStyleBackColor = false;
            // 
            // dtpMuayene
            // 
            this.dtpMuayene.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dtpMuayene.Location = new System.Drawing.Point(360, 78);
            this.dtpMuayene.Name = "dtpMuayene";
            this.dtpMuayene.Size = new System.Drawing.Size(190, 25);
            this.dtpMuayene.TabIndex = 4;
            // 
            // txtNotlar
            // 
            this.txtNotlar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtNotlar.Location = new System.Drawing.Point(20, 222);
            this.txtNotlar.Name = "txtNotlar";
            this.txtNotlar.Size = new System.Drawing.Size(500, 25);
            this.txtNotlar.TabIndex = 3;
            // 
            // txtTeshis
            // 
            this.txtTeshis.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTeshis.Location = new System.Drawing.Point(375, 151);
            this.txtTeshis.Name = "txtTeshis";
            this.txtTeshis.Size = new System.Drawing.Size(130, 25);
            this.txtTeshis.TabIndex = 3;
            // 
            // txtSikayet
            // 
            this.txtSikayet.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSikayet.Location = new System.Drawing.Point(20, 151);
            this.txtSikayet.Name = "txtSikayet";
            this.txtSikayet.Size = new System.Drawing.Size(330, 25);
            this.txtSikayet.TabIndex = 3;
            // 
            // lblNotlar
            // 
            this.lblNotlar.AutoSize = true;
            this.lblNotlar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblNotlar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblNotlar.Location = new System.Drawing.Point(20, 198);
            this.lblNotlar.Name = "lblNotlar";
            this.lblNotlar.Size = new System.Drawing.Size(92, 15);
            this.lblNotlar.TabIndex = 2;
            this.lblNotlar.Text = "Veteriner Notları";
            // 
            // lblTeshis
            // 
            this.lblTeshis.AutoSize = true;
            this.lblTeshis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTeshis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblTeshis.Location = new System.Drawing.Point(375, 126);
            this.lblTeshis.Name = "lblTeshis";
            this.lblTeshis.Size = new System.Drawing.Size(38, 15);
            this.lblTeshis.TabIndex = 2;
            this.lblTeshis.Text = "Teşhis";
            // 
            // lblSikayet
            // 
            this.lblSikayet.AutoSize = true;
            this.lblSikayet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSikayet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblSikayet.Location = new System.Drawing.Point(20, 126);
            this.lblSikayet.Name = "lblSikayet";
            this.lblSikayet.Size = new System.Drawing.Size(44, 15);
            this.lblSikayet.TabIndex = 2;
            this.lblSikayet.Text = "Şikayet";
            // 
            // lblTarih
            // 
            this.lblTarih.AutoSize = true;
            this.lblTarih.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTarih.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblTarih.Location = new System.Drawing.Point(360, 53);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(86, 15);
            this.lblTarih.TabIndex = 2;
            this.lblTarih.Text = "Muayene tarihi";
            // 
            // cmbHayvan
            // 
            this.cmbHayvan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHayvan.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cmbHayvan.FormattingEnabled = true;
            this.cmbHayvan.Location = new System.Drawing.Point(20, 78);
            this.cmbHayvan.Name = "cmbHayvan";
            this.cmbHayvan.Size = new System.Drawing.Size(300, 25);
            this.cmbHayvan.TabIndex = 1;
            // 
            // lblHayvan
            // 
            this.lblHayvan.AutoSize = true;
            this.lblHayvan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblHayvan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblHayvan.Location = new System.Drawing.Point(20, 53);
            this.lblHayvan.Name = "lblHayvan";
            this.lblHayvan.Size = new System.Drawing.Size(47, 15);
            this.lblHayvan.TabIndex = 0;
            this.lblHayvan.Text = "Hayvan";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.label1.Location = new System.Drawing.Point(16, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Muayene Kaydı";
            // 
            // MuayeneKaydiSayfasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.pnlListe);
            this.Controls.Add(this.pnlFiltre);
            this.Name = "MuayeneKaydiSayfasi";
            this.Size = new System.Drawing.Size(944, 621);
            this.pnlFiltre.ResumeLayout(false);
            this.pnlFiltre.PerformLayout();
            this.pnlListe.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMuayeneler)).EndInit();
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFiltre;
        private System.Windows.Forms.Button btnYenile;
        private System.Windows.Forms.TextBox txtAra;
        private System.Windows.Forms.Label lblAra;
        private System.Windows.Forms.Panel pnlListe;
        private System.Windows.Forms.DataGridView dgvMuayeneler;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnTemizle;
        private System.Windows.Forms.Button btnGuncelle;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.DateTimePicker dtpMuayene;
        private System.Windows.Forms.TextBox txtNotlar;
        private System.Windows.Forms.TextBox txtTeshis;
        private System.Windows.Forms.TextBox txtSikayet;
        private System.Windows.Forms.Label lblNotlar;
        private System.Windows.Forms.Label lblTeshis;
        private System.Windows.Forms.Label lblSikayet;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.ComboBox cmbHayvan;
        private System.Windows.Forms.Label lblHayvan;
        private System.Windows.Forms.Label label1;
    }
}
