using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace VetCare
{
    public partial class HastaKabulSayfasi : UserControl
    {
        /* TASARIMCIDA OLUŞTURULACAK KONTROLLER (Name):
           txtAra, lblSonuc, btnYenile
           dgvHastalar
           txtHAdi, cmbHTur, txtHIrk, cmbCinsiyet, dtpDogum
           txtHSahip, txtHTelefon, txtHEposta, txtHAdres
           btnGuncelle, btnSahibeEkle, btnSil, btnTemizleH
        */

        private int seciliHayvanId = 0;
        private int seciliSahipId = 0;
        private bool yukleniyor = false;
        private readonly Color Lacivert = ColorTranslator.FromHtml("#25505F");

        public HastaKabulSayfasi()
        {

            InitializeComponent();

            cmbHTur.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHTur.Items.AddRange(new object[] { "Kedi", "Köpek", "Kuş", "Tavşan", "Diğer" });

            cmbCinsiyet.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCinsiyet.Items.AddRange(new object[] { "Erkek", "Dişi", "Bilinmiyor" });

            dtpDogum.Format = DateTimePickerFormat.Short;
            dtpDogum.ShowCheckBox = true;     // işaretli değilse doğum tarihi boş kaydedilir
            dtpDogum.Checked = false;

            TabloStiliVer(dgvHastalar);

            txtAra.TextChanged += (s, e) => Yenile();
            btnYenile.Click += (s, e) => { txtAra.Clear(); Yenile(); };
            dgvHastalar.SelectionChanged += DgvHastalar_SelectionChanged;
            btnGuncelle.Click += BtnGuncelle_Click;
            btnSahibeEkle.Click += BtnSahibeEkle_Click;
            btnSil.Click += BtnSil_Click;
            btnTemizleH.Click += (s, e) => FormuTemizle();
        }

        // ---------- LİSTE ----------
        public void Yenile()
        {
            try
            {
                yukleniyor = true;
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    string sql = @"SELECT h.HayvanID, h.SahipID,
                                          h.HayvanAdi AS [Hayvan], h.Tur AS [Tür], h.Irk AS [Irk],
                                          h.Cinsiyet AS [Cinsiyet], h.DogumTarihi AS [Doğum Tarihi],
                                          s.AdSoyad AS [Sahip], s.Telefon AS [Telefon],
                                          s.Eposta AS [E-posta], s.Adres AS [Adres]
                                   FROM Hayvanlar h
                                   JOIN Sahipler s ON s.SahipID = h.SahipID
                                   WHERE h.HayvanAdi LIKE @a OR s.AdSoyad LIKE @a OR s.Telefon LIKE @a
                                   ORDER BY h.HayvanID DESC";
                    SqlDataAdapter da = new SqlDataAdapter(sql, baglanti);
                    da.SelectCommand.Parameters.AddWithValue("@a", "%" + txtAra.Text.Trim() + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvHastalar.DataSource = dt;

                    dgvHastalar.Columns["HayvanID"].Visible = false;
                    dgvHastalar.Columns["SahipID"].Visible = false;
                    dgvHastalar.Columns["Doğum Tarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    lblSonuc.Text = "Toplam: " + dt.Rows.Count + " kayıt";
                }
                dgvHastalar.ClearSelection();
                FormuTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Liste yüklenemedi:\n" + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                yukleniyor = false;
            }
        }

        // ---------- SATIR SEÇİNCE FORMU DOLDUR ----------
        private void DgvHastalar_SelectionChanged(object sender, EventArgs e)
        {
            if (yukleniyor || dgvHastalar.SelectedRows.Count == 0) return;

            DataGridViewRow r = dgvHastalar.SelectedRows[0];
            seciliHayvanId = Convert.ToInt32(r.Cells["HayvanID"].Value);
            seciliSahipId = Convert.ToInt32(r.Cells["SahipID"].Value);

            txtHAdi.Text = r.Cells["Hayvan"].Value.ToString();
            cmbHTur.SelectedItem = r.Cells["Tür"].Value.ToString();
            txtHIrk.Text = r.Cells["Irk"].Value.ToString();
            cmbCinsiyet.SelectedItem = r.Cells["Cinsiyet"].Value.ToString();

            object dogum = r.Cells["Doğum Tarihi"].Value;
            if (dogum == null || dogum == DBNull.Value)
            {
                dtpDogum.Checked = false;
            }
            else
            {
                dtpDogum.Value = Convert.ToDateTime(dogum);
                dtpDogum.Checked = true;
            }

            txtHSahip.Text = r.Cells["Sahip"].Value.ToString();
            txtHTelefon.Text = r.Cells["Telefon"].Value.ToString();
            txtHEposta.Text = r.Cells["E-posta"].Value.ToString();
            txtHAdres.Text = r.Cells["Adres"].Value.ToString();
        }

        // ---------- GÜNCELLE ----------
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (seciliHayvanId == 0)
            {
                Uyari("Önce listeden bir kayıt seçin.");
                return;
            }
            if (!AlanlariKontrolEt()) return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    using (SqlTransaction islem = baglanti.BeginTransaction())
                    {
                        SqlCommand k1 = new SqlCommand(
                            @"UPDATE Sahipler
                              SET AdSoyad = @ad, Telefon = @tel, Eposta = @mail, Adres = @adres
                              WHERE SahipID = @id", baglanti, islem);
                        SahipParametreleri(k1);
                        k1.Parameters.AddWithValue("@id", seciliSahipId);
                        k1.ExecuteNonQuery();

                        SqlCommand k2 = new SqlCommand(
                            @"UPDATE Hayvanlar
                              SET HayvanAdi = @hayvan, Tur = @tur, Irk = @irk,
                                  Cinsiyet = @cins, DogumTarihi = @dogum
                              WHERE HayvanID = @id", baglanti, islem);
                        HayvanParametreleri(k2);
                        k2.Parameters.AddWithValue("@id", seciliHayvanId);
                        k2.ExecuteNonQuery();

                        islem.Commit();
                    }
                }
                Bilgi("Kayıt güncellendi.");
                Yenile();
            }
            catch (SqlException ex)
            {
                Hata(ex);
            }
        }

        // ---------- SEÇİLİ SAHİBE YENİ HAYVAN EKLE ----------
        private void BtnSahibeEkle_Click(object sender, EventArgs e)
        {
            if (seciliSahipId == 0)
            {
                Uyari("Önce listeden hayvanı eklenecek sahibi (herhangi bir satırı) seçin.");
                return;
            }
            if (!AlanlariKontrolEt()) return;

            DialogResult onay = MessageBox.Show(
                "'" + txtHAdi.Text.Trim() + "' adlı yeni hayvan, '" + txtHSahip.Text.Trim() +
                "' adlı sahibe eklenecek.\n\nDevam edilsin mi?",
                "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay != DialogResult.Yes) return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    SqlCommand k = new SqlCommand(
                        @"INSERT INTO Hayvanlar (SahipID, HayvanAdi, Tur, Irk, Cinsiyet, DogumTarihi)
                          VALUES (@sahip, @hayvan, @tur, @irk, @cins, @dogum)", baglanti);
                    HayvanParametreleri(k);
                    k.Parameters.AddWithValue("@sahip", seciliSahipId);
                    k.ExecuteNonQuery();
                }
                Bilgi("Yeni hayvan sahibe eklendi.");
                Yenile();
            }
            catch (SqlException ex)
            {
                Hata(ex);
            }
        }

        // ---------- SİL ----------
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (seciliHayvanId == 0)
            {
                Uyari("Önce listeden silinecek kaydı seçin.");
                return;
            }

            DialogResult onay = MessageBox.Show(
                "'" + txtHAdi.Text.Trim() + "' adlı hayvan silinecek.\n\n" +
                "Bu hayvana ait TÜM randevu, muayene, aşı ve tedavi kayıtları da silinir.\n" +
                "Bu işlem geri alınamaz. Emin misiniz?",
                "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (onay != DialogResult.Yes) return;

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    using (SqlTransaction islem = baglanti.BeginTransaction())
                    {
                        // Tedaviler tablosunda Hayvanlar'a bağlı cascade olmadığı için önce elle silinir
                        SqlCommand k1 = new SqlCommand(
                            "DELETE FROM Tedaviler WHERE HayvanID = @id", baglanti, islem);
                        k1.Parameters.AddWithValue("@id", seciliHayvanId);
                        k1.ExecuteNonQuery();

                        // Diğer tablolar (Randevu, Muayene, Aşı) ON DELETE CASCADE ile silinir
                        SqlCommand k2 = new SqlCommand(
                            "DELETE FROM Hayvanlar WHERE HayvanID = @id", baglanti, islem);
                        k2.Parameters.AddWithValue("@id", seciliHayvanId);
                        k2.ExecuteNonQuery();

                        // Sahibin başka hayvanı kalmadıysa sahibi de sil
                        SqlCommand k3 = new SqlCommand(
                            @"DELETE FROM Sahipler
                              WHERE SahipID = @sahip
                                AND NOT EXISTS (SELECT 1 FROM Hayvanlar WHERE SahipID = @sahip)",
                            baglanti, islem);
                        k3.Parameters.AddWithValue("@sahip", seciliSahipId);
                        k3.ExecuteNonQuery();

                        islem.Commit();
                    }
                }
                Bilgi("Kayıt silindi.");
                Yenile();
            }
            catch (SqlException ex)
            {
                Hata(ex);
            }
        }

        // ---------- YARDIMCILAR ----------
        private bool AlanlariKontrolEt()
        {
            if (txtHAdi.Text.Trim() == "")      // BR-01
            {
                Uyari("Hayvan adı boş bırakılamaz.");
                txtHAdi.Focus();
                return false;
            }
            if (cmbHTur.SelectedIndex < 0)
            {
                Uyari("Lütfen hayvanın türünü seçin.");
                return false;
            }
            if (txtHSahip.Text.Trim() == "")
            {
                Uyari("Sahip adı boş bırakılamaz.");
                txtHSahip.Focus();
                return false;
            }
            return true;
        }

        private void HayvanParametreleri(SqlCommand k)
        {
            k.Parameters.AddWithValue("@hayvan", txtHAdi.Text.Trim());
            k.Parameters.AddWithValue("@tur", cmbHTur.SelectedItem.ToString());
            k.Parameters.AddWithValue("@irk", Bos(txtHIrk.Text));
            k.Parameters.AddWithValue("@cins",
                cmbCinsiyet.SelectedIndex < 0 ? (object)DBNull.Value : cmbCinsiyet.SelectedItem.ToString());
            k.Parameters.AddWithValue("@dogum",
                dtpDogum.Checked ? (object)dtpDogum.Value.Date : DBNull.Value);
        }

        private void SahipParametreleri(SqlCommand k)
        {
            k.Parameters.AddWithValue("@ad", txtHSahip.Text.Trim());
            k.Parameters.AddWithValue("@tel", Bos(txtHTelefon.Text));
            k.Parameters.AddWithValue("@mail", Bos(txtHEposta.Text));
            k.Parameters.AddWithValue("@adres", Bos(txtHAdres.Text));
        }

        // Boş metni veritabanına NULL olarak gönder
        private object Bos(string metin)
        {
            metin = metin.Trim();
            return metin == "" ? (object)DBNull.Value : metin;
        }

        private void FormuTemizle()
        {
            seciliHayvanId = 0;
            seciliSahipId = 0;
            txtHAdi.Clear();
            cmbHTur.SelectedIndex = -1;
            txtHIrk.Clear();
            cmbCinsiyet.SelectedIndex = -1;
            dtpDogum.Checked = false;
            txtHSahip.Clear();
            txtHTelefon.Clear();
            txtHEposta.Clear();
            txtHAdres.Clear();
            dgvHastalar.ClearSelection();
        }

        private void Uyari(string m) { MessageBox.Show(m, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        private void Bilgi(string m) { MessageBox.Show(m, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); }
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
