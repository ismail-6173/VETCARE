
namespace VetCare
{
    partial class RandevuTakibiSayfasi
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
            this.dgvRandevular = new System.Windows.Forms.DataGridView();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.btnTemizle = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnGuncelle = new System.Windows.Forms.Button();
            this.btnEkle = new System.Windows.Forms.Button();
            this.txtAciklama = new System.Windows.Forms.TextBox();
            this.lblAciklama = new System.Windows.Forms.Label();
            this.dtpRandevu = new System.Windows.Forms.DateTimePicker();
            this.lblTarih = new System.Windows.Forms.Label();
            this.cmbDurum = new System.Windows.Forms.ComboBox();
            this.lblDurum = new System.Windows.Forms.Label();
            this.cmbHayvan = new System.Windows.Forms.ComboBox();
            this.lblHayvan = new System.Windows.Forms.Label();
            this.lblBFormBaslik = new System.Windows.Forms.Label();
            this.pnlFiltre.SuspendLayout();
            this.pnlListe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRandevular)).BeginInit();
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
            this.pnlFiltre.Size = new System.Drawing.Size(904, 56);
            this.pnlFiltre.TabIndex = 0;
            // 
            // btnYenile
            // 
            this.btnYenile.FlatAppearance.BorderSize = 2;
            this.btnYenile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYenile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYenile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(127)))), ((int)(((byte)(110)))));
            this.btnYenile.Location = new System.Drawing.Point(405, 11);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(110, 34);
            this.btnYenile.TabIndex = 2;
            this.btnYenile.Text = "Yenile";
            this.btnYenile.UseVisualStyleBackColor = true;
            // 
            // txtAra
            // 
            this.txtAra.Location = new System.Drawing.Point(70, 14);
            this.txtAra.Name = "txtAra";
            this.txtAra.Size = new System.Drawing.Size(300, 20);
            this.txtAra.TabIndex = 1;
            // 
            // lblAra
            // 
            this.lblAra.AutoSize = true;
            this.lblAra.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.lblAra.Location = new System.Drawing.Point(17, 14);
            this.lblAra.Name = "lblAra";
            this.lblAra.Size = new System.Drawing.Size(38, 20);
            this.lblAra.TabIndex = 0;
            this.lblAra.Text = "Ara:";
            // 
            // pnlListe
            // 
            this.pnlListe.BackColor = System.Drawing.Color.White;
            this.pnlListe.Controls.Add(this.dgvRandevular);
            this.pnlListe.Location = new System.Drawing.Point(20, 86);
            this.pnlListe.Name = "pnlListe";
            this.pnlListe.Size = new System.Drawing.Size(904, 250);
            this.pnlListe.TabIndex = 1;
            // 
            // dgvRandevular
            // 
            this.dgvRandevular.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRandevular.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRandevular.Location = new System.Drawing.Point(8, 8);
            this.dgvRandevular.Name = "dgvRandevular";
            this.dgvRandevular.Size = new System.Drawing.Size(888, 234);
            this.dgvRandevular.TabIndex = 0;
            // 
            // pnlForm
            // 
            this.pnlForm.BackColor = System.Drawing.Color.White;
            this.pnlForm.Controls.Add(this.lblBFormBaslik);
            this.pnlForm.Controls.Add(this.btnTemizle);
            this.pnlForm.Controls.Add(this.btnSil);
            this.pnlForm.Controls.Add(this.btnGuncelle);
            this.pnlForm.Controls.Add(this.btnEkle);
            this.pnlForm.Controls.Add(this.txtAciklama);
            this.pnlForm.Controls.Add(this.lblAciklama);
            this.pnlForm.Controls.Add(this.dtpRandevu);
            this.pnlForm.Controls.Add(this.lblTarih);
            this.pnlForm.Controls.Add(this.cmbDurum);
            this.pnlForm.Controls.Add(this.lblDurum);
            this.pnlForm.Controls.Add(this.cmbHayvan);
            this.pnlForm.Controls.Add(this.lblHayvan);
            this.pnlForm.Location = new System.Drawing.Point(20, 352);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(904, 220);
            this.pnlForm.TabIndex = 2;
            // 
            // btnTemizle
            // 
            this.btnTemizle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.btnTemizle.FlatAppearance.BorderSize = 2;
            this.btnTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTemizle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTemizle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.btnTemizle.Location = new System.Drawing.Point(817, 181);
            this.btnTemizle.Name = "btnTemizle";
            this.btnTemizle.Size = new System.Drawing.Size(79, 34);
            this.btnTemizle.TabIndex = 8;
            this.btnTemizle.Text = "Temizle";
            this.btnTemizle.UseVisualStyleBackColor = true;
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
            this.btnSil.FlatAppearance.BorderSize = 0;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(726, 181);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(85, 34);
            this.btnSil.TabIndex = 8;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            // 
            // btnGuncelle
            // 
            this.btnGuncelle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.btnGuncelle.FlatAppearance.BorderSize = 0;
            this.btnGuncelle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuncelle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGuncelle.ForeColor = System.Drawing.Color.White;
            this.btnGuncelle.Location = new System.Drawing.Point(751, 141);
            this.btnGuncelle.Name = "btnGuncelle";
            this.btnGuncelle.Size = new System.Drawing.Size(145, 34);
            this.btnGuncelle.TabIndex = 8;
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
            this.btnEkle.Location = new System.Drawing.Point(751, 101);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(145, 34);
            this.btnEkle.TabIndex = 8;
            this.btnEkle.Text = "Randevu Ekle";
            this.btnEkle.UseVisualStyleBackColor = false;
            // 
            // txtAciklama
            // 
            this.txtAciklama.Location = new System.Drawing.Point(21, 131);
            this.txtAciklama.Multiline = true;
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAciklama.Size = new System.Drawing.Size(710, 20);
            this.txtAciklama.TabIndex = 7;
            // 
            // lblAciklama
            // 
            this.lblAciklama.AutoSize = true;
            this.lblAciklama.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAciklama.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblAciklama.Location = new System.Drawing.Point(21, 107);
            this.lblAciklama.Name = "lblAciklama";
            this.lblAciklama.Size = new System.Drawing.Size(56, 15);
            this.lblAciklama.TabIndex = 6;
            this.lblAciklama.Text = "Açıklama";
            // 
            // dtpRandevu
            // 
            this.dtpRandevu.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpRandevu.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpRandevu.Location = new System.Drawing.Point(692, 68);
            this.dtpRandevu.Name = "dtpRandevu";
            this.dtpRandevu.Size = new System.Drawing.Size(169, 20);
            this.dtpRandevu.TabIndex = 5;
            // 
            // lblTarih
            // 
            this.lblTarih.AutoSize = true;
            this.lblTarih.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTarih.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblTarih.Location = new System.Drawing.Point(689, 43);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(71, 15);
            this.lblTarih.TabIndex = 4;
            this.lblTarih.Text = "Tarih ve saat";
            // 
            // cmbDurum
            // 
            this.cmbDurum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDurum.FormattingEnabled = true;
            this.cmbDurum.Location = new System.Drawing.Point(336, 67);
            this.cmbDurum.Name = "cmbDurum";
            this.cmbDurum.Size = new System.Drawing.Size(335, 21);
            this.cmbDurum.TabIndex = 3;
            // 
            // lblDurum
            // 
            this.lblDurum.AutoSize = true;
            this.lblDurum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDurum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblDurum.Location = new System.Drawing.Point(336, 43);
            this.lblDurum.Name = "lblDurum";
            this.lblDurum.Size = new System.Drawing.Size(44, 15);
            this.lblDurum.TabIndex = 2;
            this.lblDurum.Text = "Durum";
            // 
            // cmbHayvan
            // 
            this.cmbHayvan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHayvan.FormattingEnabled = true;
            this.cmbHayvan.Location = new System.Drawing.Point(21, 67);
            this.cmbHayvan.Name = "cmbHayvan";
            this.cmbHayvan.Size = new System.Drawing.Size(280, 21);
            this.cmbHayvan.TabIndex = 1;
            // 
            // lblHayvan
            // 
            this.lblHayvan.AutoSize = true;
            this.lblHayvan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblHayvan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.lblHayvan.Location = new System.Drawing.Point(21, 43);
            this.lblHayvan.Name = "lblHayvan";
            this.lblHayvan.Size = new System.Drawing.Size(96, 15);
            this.lblHayvan.TabIndex = 0;
            this.lblHayvan.Text = "Hayvan ve sahibi";
            // 
            // lblBFormBaslik
            // 
            this.lblBFormBaslik.AutoSize = true;
            this.lblBFormBaslik.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblBFormBaslik.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(80)))), ((int)(((byte)(95)))));
            this.lblBFormBaslik.Location = new System.Drawing.Point(17, 14);
            this.lblBFormBaslik.Name = "lblBFormBaslik";
            this.lblBFormBaslik.Size = new System.Drawing.Size(111, 20);
            this.lblBFormBaslik.TabIndex = 9;
            this.lblBFormBaslik.Text = "Randevu Takip";
            // 
            // RandevuTakibiSayfasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.pnlListe);
            this.Controls.Add(this.pnlFiltre);
            this.Name = "RandevuTakibiSayfasi";
            this.Size = new System.Drawing.Size(944, 621);
            this.pnlFiltre.ResumeLayout(false);
            this.pnlFiltre.PerformLayout();
            this.pnlListe.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRandevular)).EndInit();
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
        private System.Windows.Forms.DataGridView dgvRandevular;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Button btnTemizle;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnGuncelle;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.TextBox txtAciklama;
        private System.Windows.Forms.Label lblAciklama;
        private System.Windows.Forms.DateTimePicker dtpRandevu;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.ComboBox cmbDurum;
        private System.Windows.Forms.Label lblDurum;
        private System.Windows.Forms.ComboBox cmbHayvan;
        private System.Windows.Forms.Label lblHayvan;
        private System.Windows.Forms.Label lblBFormBaslik;
    }
}
