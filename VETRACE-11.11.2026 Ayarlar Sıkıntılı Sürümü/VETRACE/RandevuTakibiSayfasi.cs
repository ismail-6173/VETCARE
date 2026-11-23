using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using VETRACE;

namespace VetCare
{
    public partial class RandevuTakibiSayfasi : UserControl
    {
        private int seciliRandevuId = 0;
        private bool yukleniyor = false;

        public RandevuTakibiSayfasi()
        {
            InitializeComponent();
            TabloStiliVer(dgvRandevular);

            cmbHayvan.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDurum.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDurum.Items.Clear();
            cmbDurum.Items.AddRange(new object[]
            {
                "Bekliyor",
                "Tamamlandı",
                "İptal"
            });
            cmbDurum.SelectedIndex = 0;

            dtpRandevu.Format = DateTimePickerFormat.Custom;
            dtpRandevu.CustomFormat = "dd.MM.yyyy HH:mm";

            dgvRandevular.ReadOnly = true;
            dgvRandevular.AllowUserToAddRows = false;
            dgvRandevular.AllowUserToDeleteRows = false;
            dgvRandevular.MultiSelect = false;
            dgvRandevular.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvRandevular.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvRandevular.RowHeadersVisible = false;

            btnYenile.Click += btnYenile_Click;
            btnEkle.Click += btnEkle_Click;
            btnGuncelle.Click += btnGuncelle_Click;
            btnSil.Click += btnSil_Click;
            btnTemizle.Click += btnTemizle_Click;
            txtAra.TextChanged += txtAra_TextChanged;
            dgvRandevular.SelectionChanged +=
                dgvRandevular_SelectionChanged;

            Yenile();
        }
        private void TabloStiliVer(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Beyaz tablo alanı ve ince beyaz ayraçlar
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.GridColor = Color.White;

            // Koyu turkuaz başlık satırı
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersVisible = true;
            dgv.ColumnHeadersHeight = 42;
            dgv.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.ColumnHeadersDefaultCellStyle.BackColor =
                ColorTranslator.FromHtml("#25505F");
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode =
                DataGridViewTriState.True;
            dgv.ColumnHeadersDefaultCellStyle.Padding =
                new Padding(6, 0, 0, 0);

            // Satırlar
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(45, 55, 60);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor =
                ColorTranslator.FromHtml("#FDE3DF");
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Padding = new Padding(5, 2, 5, 2);
            dgv.RowTemplate.Height = 32;
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
                            r.RandevuID,
                            r.HayvanID,
                            h.HayvanAdi AS Hayvan,
                            s.AdSoyad AS Sahip,
                            s.Telefon AS Telefon,
                            r.RandevuTarihi AS [Tarih ve Saat],
                            r.Aciklama AS Açıklama,
                            r.Durum AS Durum
                        FROM Randevular r
                        INNER JOIN Hayvanlar h ON h.HayvanID = r.HayvanID
                        INNER JOIN Sahipler s ON s.SahipID = h.SahipID
                        WHERE h.HayvanAdi LIKE @arama
                           OR s.AdSoyad LIKE @arama
                        ORDER BY r.RandevuTarihi DESC";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(sql, baglanti);

                    adapter.SelectCommand.Parameters.Add(
                        "@arama", SqlDbType.NVarChar, 200).Value =
                        "%" + txtAra.Text.Trim() + "%";

                    DataTable tablo = new DataTable();
                    adapter.Fill(tablo);
                    dgvRandevular.DataSource = tablo;

                    dgvRandevular.Columns["RandevuID"].Visible = false;
                    dgvRandevular.Columns["HayvanID"].Visible = false;
                    dgvRandevular.Columns["Tarih ve Saat"]
                        .DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Randevular yüklenemedi:\n" + ex.Message,
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
                    INNER JOIN Sahipler s ON s.SahipID = h.SahipID
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

        private bool AlanlariKontrolEt(bool yeniKayit)
        {
            if (cmbHayvan.SelectedValue == null ||
                cmbHayvan.SelectedValue is DataRowView)
            {
                MessageBox.Show("Lütfen hayvan seçin.");
                cmbHayvan.Focus();
                return false;
            }

            if (cmbDurum.SelectedItem == null)
            {
                MessageBox.Show("Lütfen randevu durumunu seçin.");
                cmbDurum.Focus();
                return false;
            }

            if (yeniKayit && dtpRandevu.Value < DateTime.Now)
            {
                MessageBox.Show("Yeni randevu tarihi geçmiş bir tarih olamaz.");
                dtpRandevu.Focus();
                return false;
            }

            return true;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (!AlanlariKontrolEt(true))
                return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();

                    string sql = @"
                        INSERT INTO Randevular
                            (HayvanID, RandevuTarihi, Aciklama, Durum)
                        VALUES
                            (@hayvanID, @tarih, @aciklama, @durum)";

                    using (SqlCommand komut =
                        new SqlCommand(sql, baglanti))
                    {
                        ParametreleriEkle(komut);
                        komut.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Randevu kaydedildi.");
                Yenile();
                FormuTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Randevu kaydedilemedi:\n" + ex.Message,
                    "Veritabanı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (seciliRandevuId == 0)
            {
                MessageBox.Show("Önce listeden bir randevu seçin.");
                return;
            }

            if (!AlanlariKontrolEt(false))
                return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();

                    string sql = @"
                        UPDATE Randevular
                        SET HayvanID = @hayvanID,
                            RandevuTarihi = @tarih,
                            Aciklama = @aciklama,
                            Durum = @durum
                        WHERE RandevuID = @randevuID";

                    using (SqlCommand komut =
                        new SqlCommand(sql, baglanti))
                    {
                        ParametreleriEkle(komut);
                        komut.Parameters.Add("@randevuID",
                            SqlDbType.Int).Value = seciliRandevuId;
                        komut.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Randevu güncellendi.");
                Yenile();
                FormuTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Randevu güncellenemedi:\n" + ex.Message,
                    "Veritabanı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (seciliRandevuId == 0)
            {
                MessageBox.Show("Önce listeden silinecek randevuyu seçin.");
                return;
            }

            DialogResult cevap = MessageBox.Show(
                "Seçili randevu silinsin mi?",
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
                        "DELETE FROM Randevular WHERE RandevuID = @randevuID";

                    using (SqlCommand komut =
                        new SqlCommand(sql, baglanti))
                    {
                        komut.Parameters.Add("@randevuID",
                            SqlDbType.Int).Value = seciliRandevuId;
                        komut.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Randevu silindi.");
                Yenile();
                FormuTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Randevu silinemedi:\n" + ex.Message,
                    "Veritabanı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ParametreleriEkle(SqlCommand komut)
        {
            komut.Parameters.Add("@hayvanID", SqlDbType.Int).Value =
                Convert.ToInt32(cmbHayvan.SelectedValue);

            komut.Parameters.Add("@tarih", SqlDbType.DateTime).Value =
                dtpRandevu.Value;

            komut.Parameters.Add("@aciklama", SqlDbType.NVarChar, 500).Value =
                string.IsNullOrWhiteSpace(txtAciklama.Text)
                    ? (object)DBNull.Value
                    : txtAciklama.Text.Trim();

            komut.Parameters.Add("@durum", SqlDbType.NVarChar, 30).Value =
                cmbDurum.SelectedItem.ToString();
        }

        private void dgvRandevular_SelectionChanged(
            object sender, EventArgs e)
        {
            if (yukleniyor || dgvRandevular.SelectedRows.Count == 0)
                return;

            DataGridViewRow satir = dgvRandevular.SelectedRows[0];

            if (satir.Cells["RandevuID"].Value == null)
                return;

            seciliRandevuId =
                Convert.ToInt32(satir.Cells["RandevuID"].Value);

            cmbHayvan.SelectedValue =
                Convert.ToInt32(satir.Cells["HayvanID"].Value);

            dtpRandevu.Value =
                Convert.ToDateTime(satir.Cells["Tarih ve Saat"].Value);

            txtAciklama.Text =
                Convert.ToString(satir.Cells["Açıklama"].Value);

            cmbDurum.SelectedItem =
                Convert.ToString(satir.Cells["Durum"].Value);
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
            seciliRandevuId = 0;
            txtAciklama.Clear();
            cmbDurum.SelectedIndex = 0;
            dtpRandevu.Value = DateTime.Now.AddMinutes(30);

            if (cmbHayvan.DataSource != null)
                cmbHayvan.SelectedIndex = -1;

            dgvRandevular.ClearSelection();
        }
    }
}