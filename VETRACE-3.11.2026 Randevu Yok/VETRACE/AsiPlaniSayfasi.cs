using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using VetCare;

namespace VETRACE
{
    public partial class AsiPlaniSayfasi : UserControl
    {

        private int seciliAsiId = 0;
        private bool yukleniyor = false;
        private readonly Color Lacivert = ColorTranslator.FromHtml("#25505F");
        public AsiPlaniSayfasi()
        {
            InitializeComponent();

            // Filtre
            cmbAsiFiltre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAsiFiltre.Items.AddRange(new object[] { "Tümü", "Gecikti", "Yaklaşıyor", "Planlı", "Yenilendi" });
            cmbAsiFiltre.SelectedIndex = 0;

            // Form
            cmbAsiHayvan.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbAsiAdi.DropDownStyle = ComboBoxStyle.DropDown;   // listeden seçilir ya da elle yazılır
            cmbAsiAdi.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbAsiAdi.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbAsiAdi.Items.AddRange(new object[]
            {
                "Karma Aşı", "Kuduz Aşısı", "Lösemi Aşısı", "Bronşin Aşısı",
                "Parvovirüs Aşısı", "İç Parazit Aşısı", "Dış Parazit Aşısı"
            });

            dtpYapilma.Format = DateTimePickerFormat.Short;
            dtpSonraki.Format = DateTimePickerFormat.Short;
            dtpSonraki.ShowCheckBox = true;     // işaretli değilse sonraki aşı tarihi boş kaydedilir

            TabloStiliVer(dgvAsilar2);

            txtAsiAra.TextChanged += (s, e) => { if (!yukleniyor) Yenile(); };
            cmbAsiFiltre.SelectedIndexChanged += (s, e) => { if (!yukleniyor) Yenile(); };
            btnAsiYenile.Click += (s, e) => { txtAsiAra.Clear(); cmbAsiFiltre.SelectedIndex = 0; Yenile(); };

            dgvAsilar2.SelectionChanged += DgvAsilar_SelectionChanged;
            btnAsiEkle.Click += BtnEkle_Click;
            btnAsiGuncelle.Click += BtnGuncelle_Click;
            btnAsiTekrar.Click += BtnTekrar_Click;
            btnAsiSil.Click += BtnSil_Click;
            btnAsiTemizle.Click += (s, e) => FormuTemizle();
        }

        // ---------- LİSTE ----------
        public void Yenile()
        {
            try
            {
                yukleniyor = true;
                HayvanlariYukle();

                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    string sql = @"SELECT * FROM (
                                     SELECT a.AsiID, a.HayvanID,
                                            h.HayvanAdi AS [Hayvan], h.Tur AS [Tür],
                                            s.AdSoyad AS [Sahip], s.Telefon AS [Telefon],
                                            a.AsiAdi AS [Aşı],
                                            a.YapilmaTarihi AS [Yapılma Tarihi],
                                            a.SonrakiAsiTarihi AS [Sonraki Aşı],
                                            CASE
                                              WHEN EXISTS (SELECT 1 FROM Asilar b
                                                           WHERE b.HayvanID = a.HayvanID AND b.AsiAdi = a.AsiAdi
                                                             AND b.YapilmaTarihi > a.YapilmaTarihi) THEN N'Yenilendi'
                                              WHEN a.SonrakiAsiTarihi IS NULL THEN N'Planlanmadı'
                                              WHEN a.SonrakiAsiTarihi < CAST(GETDATE() AS DATE) THEN N'Gecikti'
                                              WHEN a.SonrakiAsiTarihi <= DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) THEN N'Yaklaşıyor'
                                              ELSE N'Planlı'
                                            END AS [Durum]
                                     FROM Asilar a
                                     JOIN Hayvanlar h ON h.HayvanID = a.HayvanID
                                     JOIN Sahipler s ON s.SahipID = h.SahipID
                                     WHERE h.HayvanAdi LIKE @ara OR s.AdSoyad LIKE @ara OR a.AsiAdi LIKE @ara
                                   ) x
                                   WHERE (@durum IS NULL OR x.[Durum] = @durum)
                                   ORDER BY CASE WHEN x.[Sonraki Aşı] IS NULL THEN 1 ELSE 0 END, x.[Sonraki Aşı]";

                    SqlDataAdapter da = new SqlDataAdapter(sql, baglanti);
                    da.SelectCommand.Parameters.AddWithValue("@ara", "%" + txtAsiAra.Text.Trim() + "%");
                    SqlParameter pDurum = da.SelectCommand.Parameters.Add("@durum", SqlDbType.NVarChar, 30);
                    pDurum.Value = cmbAsiFiltre.SelectedIndex <= 0
                        ? (object)DBNull.Value : cmbAsiFiltre.SelectedItem.ToString();

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAsilar2.DataSource = dt;

                    dgvAsilar2.Columns["AsiID"].Visible = false;
                    dgvAsilar2.Columns["HayvanID"].Visible = false;
                    dgvAsilar2.Columns["Yapılma Tarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvAsilar2.Columns["Sonraki Aşı"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    lblAsiSonuc.Text = "Toplam: " + dt.Rows.Count + " aşı";
                }

                DurumRenkleriniUygula();
                FormuTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Aşılar yüklenemedi:\n" + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string sql = @"SELECT h.HayvanID,
                                      h.HayvanAdi + N' (' + s.AdSoyad + N')' AS Gorunum
                               FROM Hayvanlar h
                               JOIN Sahipler s ON s.SahipID = h.SahipID
                               ORDER BY h.HayvanAdi";
                SqlDataAdapter da = new SqlDataAdapter(sql, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbAsiHayvan.DataSource = dt;
                cmbAsiHayvan.DisplayMember = "Gorunum";
                cmbAsiHayvan.ValueMember = "HayvanID";
                cmbAsiHayvan.SelectedIndex = -1;
            }
        }

        private void DurumRenkleriniUygula()
        {
            foreach (DataGridViewRow satir in dgvAsilar2.Rows)
            {
                string durum = Convert.ToString(satir.Cells["Durum"].Value);
                if (durum == "Gecikti")
                {
                    satir.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#D9534F");
                    satir.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
                else if (durum == "Yaklaşıyor")
                    satir.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#E67E22");
                else if (durum == "Yenilendi")
                    satir.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#9E9E9E");
            }
        }

        // ---------- SATIR SEÇİNCE FORMU DOLDUR ----------
        private void DgvAsilar_SelectionChanged(object sender, EventArgs e)
        {
            if (yukleniyor || dgvAsilar2.SelectedRows.Count == 0) return;

            DataGridViewRow r = dgvAsilar2.SelectedRows[0];
            seciliAsiId = Convert.ToInt32(r.Cells["AsiID"].Value);
            cmbAsiHayvan.SelectedValue = Convert.ToInt32(r.Cells["HayvanID"].Value);
            cmbAsiAdi.Text = Convert.ToString(r.Cells["Aşı"].Value);
            dtpYapilma.Value = Convert.ToDateTime(r.Cells["Yapılma Tarihi"].Value);

            object sonraki = r.Cells["Sonraki Aşı"].Value;
            if (sonraki == null || sonraki == DBNull.Value)
            {
                dtpSonraki.Checked = false;
            }
            else
            {
                dtpSonraki.Value = Convert.ToDateTime(sonraki);
                dtpSonraki.Checked = true;
            }
        }

        // ---------- EKLE ----------
        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (!AlanlariKontrolEt()) return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    if (AyniKayitVarMi(baglanti, 0)) return;

                    SqlCommand k = new SqlCommand(
                        @"INSERT INTO Asilar (HayvanID, AsiAdi, YapilmaTarihi, SonrakiAsiTarihi)
                          VALUES (@hayvan, @asi, @yapilma, @sonraki)", baglanti);
                    Parametreler(k);
                    k.ExecuteNonQuery();
                }
                Bilgi("Aşı kaydı eklendi.");
                Yenile();
            }
            catch (SqlException ex) { Hata(ex); }
        }

        // ---------- GÜNCELLE ----------
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (seciliAsiId == 0) { Uyari("Önce listeden bir aşı kaydı seçin."); return; }
            if (!AlanlariKontrolEt()) return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    if (AyniKayitVarMi(baglanti, seciliAsiId)) return;

                    SqlCommand k = new SqlCommand(
                        @"UPDATE Asilar
                          SET HayvanID = @hayvan, AsiAdi = @asi,
                              YapilmaTarihi = @yapilma, SonrakiAsiTarihi = @sonraki
                          WHERE AsiID = @id", baglanti);
                    Parametreler(k);
                    k.Parameters.AddWithValue("@id", seciliAsiId);
                    k.ExecuteNonQuery();
                }
                Bilgi("Aşı kaydı güncellendi.");
                Yenile();
            }
            catch (SqlException ex) { Hata(ex); }
        }

        // ---------- AYNI AŞIYI TEKRARLA ----------
        private void BtnTekrar_Click(object sender, EventArgs e)
        {
            if (seciliAsiId == 0) { Uyari("Önce listeden tekrarlanacak aşıyı seçin."); return; }

            seciliAsiId = 0;                        
            dtpYapilma.Value = DateTime.Today;
            dtpSonraki.Value = DateTime.Today.AddYears(1);
            dtpSonraki.Checked = true;

            Bilgi("Form yeni aşı için hazırlandı.\nTarihleri kontrol edip 'Aşı Ekle' butonuna basın.");
        }

        // ---------- SİL ----------
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (seciliAsiId == 0) { Uyari("Önce listeden silinecek kaydı seçin."); return; }

            DialogResult onay = MessageBox.Show("Seçili aşı kaydı kalıcı olarak silinecek. Emin misiniz?",
                "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (onay != DialogResult.Yes) return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    SqlCommand k = new SqlCommand("DELETE FROM Asilar WHERE AsiID = @id", baglanti);
                    k.Parameters.AddWithValue("@id", seciliAsiId);
                    k.ExecuteNonQuery();
                }
                Bilgi("Aşı kaydı silindi.");
                Yenile();
            }
            catch (SqlException ex) { Hata(ex); }
        }

        // ---------- YARDIMCILAR ----------
        private bool AlanlariKontrolEt()
        {
            if (cmbAsiHayvan.SelectedIndex < 0 || cmbAsiHayvan.SelectedValue == null)
            {
                Uyari("Lütfen aşının yapıldığı hayvanı seçin.");
                return false;
            }
            if (cmbAsiAdi.Text.Trim() == "")
            {
                Uyari("Aşı adı boş bırakılamaz.");
                cmbAsiAdi.Focus();
                return false;
            }
            if (dtpYapilma.Value.Date > DateTime.Today)
            {
                Uyari("Yapılma tarihi gelecekte olamaz.");
                return false;
            }
            if (dtpSonraki.Checked && dtpSonraki.Value.Date < dtpYapilma.Value.Date)
            {
                Uyari("Sonraki aşı tarihi, yapılma tarihinden önce olamaz.");
                return false;
            }
            return true;
        }

        // Aynı hayvana, aynı aşı, aynı gün iki kez girilmesin
        private bool AyniKayitVarMi(SqlConnection baglanti, int haricId)
        {
            SqlCommand k = new SqlCommand(
                @"SELECT COUNT(*) FROM Asilar
                  WHERE HayvanID = @h AND AsiAdi = @a AND YapilmaTarihi = @y AND AsiID <> @id", baglanti);
            k.Parameters.AddWithValue("@h", Convert.ToInt32(cmbAsiHayvan.SelectedValue));
            k.Parameters.AddWithValue("@a", cmbAsiAdi.Text.Trim());
            k.Parameters.Add("@y", SqlDbType.Date).Value = dtpYapilma.Value.Date;
            k.Parameters.AddWithValue("@id", haricId);
            if ((int)k.ExecuteScalar() > 0)
            {
                Uyari("Bu hayvan için aynı aşı aynı tarihte zaten kayıtlı.");
                return true;
            }
            return false;
        }

        private void Parametreler(SqlCommand k)
        {
            k.Parameters.AddWithValue("@hayvan", Convert.ToInt32(cmbAsiHayvan.SelectedValue));
            k.Parameters.AddWithValue("@asi", cmbAsiAdi.Text.Trim());
            k.Parameters.Add("@yapilma", SqlDbType.Date).Value = dtpYapilma.Value.Date;
            k.Parameters.Add("@sonraki", SqlDbType.Date).Value =
                dtpSonraki.Checked ? (object)dtpSonraki.Value.Date : DBNull.Value;
        }

        private void FormuTemizle()
        {
            seciliAsiId = 0;
            cmbAsiHayvan.SelectedIndex = -1;
            cmbAsiAdi.SelectedIndex = -1;
            cmbAsiAdi.Text = "";
            dtpYapilma.Value = DateTime.Today;
            dtpSonraki.Value = DateTime.Today.AddYears(1);
            dtpSonraki.Checked = true;
            dgvAsilar2.ClearSelection();
        }

        private void Uyari(string m) { MessageBox.Show(m, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        private void Bilgi(string m) { MessageBox.Show(m, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Hata(SqlException ex) { MessageBox.Show("Veritabanı hatası:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        private void TabloStiliVer(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Lacivert;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#FDE3DF");
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 30;
        }
    }
}
        