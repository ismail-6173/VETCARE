using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using VETRACE;
using VetCare;

namespace VetCare
{
    public partial class MuayeneKaydiSayfasi : UserControl
    {
        private int seciliMuayeneID = 0;
        private bool yukleniyor = false;

        public MuayeneKaydiSayfasi()
        {
            InitializeComponent();

            cmbHayvan.DropDownStyle = ComboBoxStyle.DropDownList;

            dtpMuayene.Format = DateTimePickerFormat.Custom;
            dtpMuayene.CustomFormat = "dd.MM.yyyy HH:mm";

            dgvMuayeneler.ReadOnly = true;
            dgvMuayeneler.AllowUserToAddRows = false;
            dgvMuayeneler.AllowUserToDeleteRows = false;
            dgvMuayeneler.MultiSelect = false;
            dgvMuayeneler.RowHeadersVisible = false;
            dgvMuayeneler.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvMuayeneler.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // Boş alanın gri görünmesini engelle ve ana ekranla aynı renkleri kullan.
            dgvMuayeneler.BackgroundColor = System.Drawing.Color.White;
            dgvMuayeneler.BorderStyle = BorderStyle.None;
            dgvMuayeneler.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvMuayeneler.GridColor = System.Drawing.Color.FromArgb(230, 235, 239);
            dgvMuayeneler.EnableHeadersVisualStyles = false;
            dgvMuayeneler.ColumnHeadersDefaultCellStyle.BackColor =
                System.Drawing.Color.FromArgb(37, 80, 95);
            dgvMuayeneler.ColumnHeadersDefaultCellStyle.ForeColor =
                System.Drawing.Color.White;
            dgvMuayeneler.ColumnHeadersDefaultCellStyle.Font =
                new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvMuayeneler.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvMuayeneler.DefaultCellStyle.ForeColor =
                System.Drawing.Color.FromArgb(49, 65, 81);
            dgvMuayeneler.DefaultCellStyle.SelectionBackColor =
                System.Drawing.Color.FromArgb(253, 227, 223);
            dgvMuayeneler.DefaultCellStyle.SelectionForeColor =
                System.Drawing.Color.FromArgb(37, 80, 95);
            dgvMuayeneler.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvMuayeneler.AlternatingRowsDefaultCellStyle.BackColor =
                System.Drawing.Color.FromArgb(246, 249, 251);
            dgvMuayeneler.RowTemplate.Height = 30;
            dgvMuayeneler.ColumnHeadersHeight = 34;
            dgvMuayeneler.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            txtAra.TextChanged += txtAra_TextChanged;
            btnYenile.Click += btnYenile_Click;
            btnEkle.Click += btnEkle_Click;
            btnGuncelle.Click += btnGuncelle_Click;
            btnSil.Click += btnSil_Click;
            btnTemizle.Click += btnTemizle_Click;
            dgvMuayeneler.SelectionChanged +=
                dgvMuayeneler_SelectionChanged;

            this.Load += (s, e) => Yenile();
        }

        public void Yenile()
        {
            if (yukleniyor)
                return;

            try
            {
                yukleniyor = true;

                // Ekran her açıldığında hayvan listesini SQL Server'dan yeniden çek.
                // Böylece Hasta Kabul'de sonradan eklenen hayvanlar da görünür.
                HayvanlariYukle();

                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();

                    string sql = @"
                        SELECT
                            m.MuayeneID,
                            m.HayvanID,
                            h.HayvanAdi AS Hayvan,
                            h.Tur AS Tür,
                            s.AdSoyad AS Sahip,
                            m.MuayeneTarihi AS Tarih,
                            m.Sikayet AS Şikayet,
                            m.Teshis AS Teşhis,
                            m.Notlar AS Notlar
                        FROM Muayeneler m
                        INNER JOIN Hayvanlar h
                            ON h.HayvanID = m.HayvanID
                        INNER JOIN Sahipler s
                            ON s.SahipID = h.SahipID
                        WHERE h.HayvanAdi LIKE @arama
                           OR s.AdSoyad LIKE @arama
                        ORDER BY m.MuayeneTarihi DESC";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, baglanti);

                    adapter.SelectCommand.Parameters.Add(
                        "@arama", SqlDbType.NVarChar, 200).Value =
                        "%" + txtAra.Text.Trim() + "%";

                    DataTable tablo = new DataTable();
                    adapter.Fill(tablo);

                    dgvMuayeneler.DataSource = tablo;

                    dgvMuayeneler.Columns["MuayeneID"].Visible = false;
                    dgvMuayeneler.Columns["HayvanID"].Visible = false;
                    dgvMuayeneler.Columns["Tarih"]
                        .DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Muayene kayıtları yüklenemedi:\n" + ex.Message,
                    "Veritabanı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                yukleniyor = false;
            }
        }

        private void HayvanlariYukle()
        {
            using (SqlConnection baglanti = Veritabani.BaglantiAl())
            {
                baglanti.Open();

                string sql = @"
                    SELECT
                        h.HayvanID,
                        h.HayvanAdi + N' (' + s.AdSoyad + N')' AS Gorunum
                    FROM Hayvanlar h
                    INNER JOIN Sahipler s
                        ON s.SahipID = h.SahipID
                    ORDER BY h.HayvanAdi";

                SqlDataAdapter adapter =
                    new SqlDataAdapter(sql, baglanti);

                DataTable hayvanlar = new DataTable();
                adapter.Fill(hayvanlar);

                cmbHayvan.DataSource = hayvanlar;
                cmbHayvan.DisplayMember = "Gorunum";
                cmbHayvan.ValueMember = "HayvanID";
                cmbHayvan.SelectedIndex = -1;
            }
        }

        private bool AlanlariKontrolEt()
        {
            if (cmbHayvan.SelectedValue == null ||
                cmbHayvan.SelectedValue is DataRowView)
            {
                MessageBox.Show("Lütfen hayvan seçin.");
                cmbHayvan.Focus();
                return false;
            }

            if (dtpMuayene.Value > DateTime.Now)
            {
                MessageBox.Show("Muayene tarihi gelecekte olamaz.");
                dtpMuayene.Focus();
                return false;
            }

            return true;
        }

        private object BosVeyaNull(string metin)
        {
            if (string.IsNullOrWhiteSpace(metin))
                return DBNull.Value;

            return metin.Trim();
        }

        private void ParametreleriEkle(SqlCommand komut)
        {
            komut.Parameters.Add("@hayvanID", SqlDbType.Int).Value =
                Convert.ToInt32(cmbHayvan.SelectedValue);

            komut.Parameters.Add("@tarih", SqlDbType.DateTime).Value =
                dtpMuayene.Value;

            komut.Parameters.Add("@sikayet", SqlDbType.NVarChar, 500).Value =
                BosVeyaNull(txtSikayet.Text);

            komut.Parameters.Add("@teshis", SqlDbType.NVarChar, 500).Value =
                BosVeyaNull(txtTeshis.Text);

            komut.Parameters.Add("@notlar", SqlDbType.NVarChar, 1000).Value =
                BosVeyaNull(txtNotlar.Text);
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (!AlanlariKontrolEt())
                return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();

                    string sql = @"
                        INSERT INTO Muayeneler
                            (HayvanID, MuayeneTarihi, Sikayet, Teshis, Notlar)
                        VALUES
                            (@hayvanID, @tarih, @sikayet, @teshis, @notlar)";

                    using (SqlCommand komut =
                        new SqlCommand(sql, baglanti))
                    {
                        ParametreleriEkle(komut);
                        komut.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Muayene kaydı eklendi.");
                Yenile();
                FormuTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Muayene kaydı eklenemedi:\n" + ex.Message,
                    "Veritabanı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (seciliMuayeneID == 0)
            {
                MessageBox.Show("Güncellemek için tablodan muayene seçin.");
                return;
            }

            if (!AlanlariKontrolEt())
                return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();

                    string sql = @"
                        UPDATE Muayeneler
                        SET HayvanID = @hayvanID,
                            MuayeneTarihi = @tarih,
                            Sikayet = @sikayet,
                            Teshis = @teshis,
                            Notlar = @notlar
                        WHERE MuayeneID = @muayeneID";

                    using (SqlCommand komut =
                        new SqlCommand(sql, baglanti))
                    {
                        ParametreleriEkle(komut);
                        komut.Parameters.Add("@muayeneID",
                            SqlDbType.Int).Value = seciliMuayeneID;

                        komut.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Muayene kaydı güncellendi.");
                Yenile();
                FormuTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Muayene kaydı güncellenemedi:\n" + ex.Message,
                    "Veritabanı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (seciliMuayeneID == 0)
            {
                MessageBox.Show("Silmek için tablodan muayene seçin.");
                return;
            }

            DialogResult cevap = MessageBox.Show(
                "Seçili muayene kaydını silmek istiyor musunuz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (cevap != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();

                    string sql =
                        "DELETE FROM Muayeneler WHERE MuayeneID = @muayeneID";

                    using (SqlCommand komut =
                        new SqlCommand(sql, baglanti))
                    {
                        komut.Parameters.Add("@muayeneID",
                            SqlDbType.Int).Value = seciliMuayeneID;

                        komut.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Muayene kaydı silindi.");
                Yenile();
                FormuTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Muayene kaydı silinemedi:\n" + ex.Message,
                    "Veritabanı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvMuayeneler_SelectionChanged(
            object sender, EventArgs e)
        {
            if (yukleniyor ||
                dgvMuayeneler.SelectedRows.Count == 0)
                return;

            DataGridViewRow satir =
                dgvMuayeneler.SelectedRows[0];

            if (satir.Cells["MuayeneID"].Value == null)
                return;

            seciliMuayeneID =
                Convert.ToInt32(satir.Cells["MuayeneID"].Value);

            cmbHayvan.SelectedValue =
                Convert.ToInt32(satir.Cells["HayvanID"].Value);

            dtpMuayene.Value =
                Convert.ToDateTime(satir.Cells["Tarih"].Value);

            txtSikayet.Text =
                Convert.ToString(satir.Cells["Şikayet"].Value);

            txtTeshis.Text =
                Convert.ToString(satir.Cells["Teşhis"].Value);

            txtNotlar.Text =
                Convert.ToString(satir.Cells["Notlar"].Value);
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            Yenile();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            txtAra.Clear();
            Yenile();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            FormuTemizle();
        }

        private void FormuTemizle()
        {
            seciliMuayeneID = 0;
            txtSikayet.Clear();
            txtTeshis.Clear();
            txtNotlar.Clear();
            dtpMuayene.Value = DateTime.Now;

            if (cmbHayvan.DataSource != null)
                cmbHayvan.SelectedIndex = -1;

            dgvMuayeneler.ClearSelection();
        }
    }
}