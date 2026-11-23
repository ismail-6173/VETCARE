using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VetCare
{
    public class AyarlarSayfasi : UserControl
    {
        private readonly Color Petrol = Color.FromArgb(37, 80, 95);
        private readonly Color Mercan = Color.FromArgb(244, 127, 110);
        private readonly Color ArkaPlan = Color.FromArgb(245, 247, 249);
        private readonly Color Yazi = Color.FromArgb(49, 65, 81);
        private readonly Color Kenarlik = Color.FromArgb(220, 228, 233);

        private DataGridView dgvKullanicilar;
        private TextBox txtAdSoyad, txtKullaniciAdi, txtSifre, txtAra;
        private ComboBox cmbRol;
        private Button btnEkle, btnGuncelle, btnSil, btnTemizle, btnYenile;
        private Panel kartForm, kartListe;
        private Label lblBaslik;
        private PictureBox picProfil;
        private Button btnFotografSec, btnFotografSil;
        private byte[] seciliFotograf;
        private int seciliKullaniciID = 0;
        private bool tabloYukleniyor = false;

        public AyarlarSayfasi()
        {
            if (!Oturum.YoneticiMi)
                throw new InvalidOperationException("Ayarlar sayfasına yalnızca yöneticiler erişebilir.");

            Dock = DockStyle.Fill;
            BackColor = ArkaPlan;
            AutoScroll = true;
            MinimumSize = new Size(500, 400);
            ArayuzuOlustur();
            Load += (s, e) => Yenile();
            Resize += (s, e) => BoyutlariAyarla();
        }

        private void ArayuzuOlustur()
        {
            lblBaslik = new Label
            {
                Text = "Klinik Ayarları", AutoSize = true, Location = new Point(22, 13),
                Font = new Font("Segoe UI", 17, FontStyle.Bold), ForeColor = Petrol
            };
            Label alt = new Label
            {
                Text = "Personel profillerini, kullanıcı hesaplarını ve yetkileri yönetin.",
                AutoSize = true, Location = new Point(25, 48),
                Font = new Font("Segoe UI", 9), ForeColor = Color.Gray
            };
            Controls.Add(lblBaslik);
            Controls.Add(alt);

            // Kullanıcı formu artık tam genişlikte; dar ekranda sağa taşmaz.
            kartForm = new Panel { Location = new Point(22, 82), BackColor = Color.White };
            Controls.Add(kartForm);
            Label formBaslik = new Label
            {
                Text = "Profil / Kullanıcı Bilgileri", Location = new Point(18, 12), AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Petrol
            };
            kartForm.Controls.Add(formBaslik);

            txtAdSoyad = AlanOlustur(kartForm, "Ad Soyad", 18, 48);
            txtKullaniciAdi = AlanOlustur(kartForm, "Kullanıcı Adı", 250, 48);
            txtSifre = AlanOlustur(kartForm, "Şifre (güncellemede boş bırakılabilir)", 18, 112);
            txtSifre.UseSystemPasswordChar = true;

            Label rolEtiket = new Label
            {
                Text = "Yetki / Rol", Location = new Point(250, 112), AutoSize = true,
                Font = new Font("Segoe UI", 9), ForeColor = Yazi
            };
            cmbRol = new ComboBox
            {
                Location = new Point(250, 134), Size = new Size(210, 29),
                DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10)
            };
            cmbRol.Items.AddRange(new object[] { "Personel", "Yonetici" });
            cmbRol.SelectedIndex = 0;
            kartForm.Controls.Add(rolEtiket);
            kartForm.Controls.Add(cmbRol);

            btnEkle = Buton("Profil Ekle", Mercan, Color.White, 0, 0, 125);
            btnGuncelle = Buton("Güncelle", Petrol, Color.White, 0, 0, 105);
            btnSil = Buton("Profili Sil", Color.FromArgb(218, 83, 83), Color.White, 0, 0, 105);
            btnTemizle = Buton("Temizle", Color.White, Petrol, 0, 0, 95);
            btnTemizle.FlatAppearance.BorderSize = 1;
            btnTemizle.FlatAppearance.BorderColor = Petrol;

            picProfil = new PictureBox
            {
                Size = new Size(92, 92), SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(242, 245, 247), BorderStyle = BorderStyle.FixedSingle
            };
            btnFotografSec = Buton("Fotoğraf Seç", Petrol, Color.White, 0, 0, 105);
            btnFotografSil = Buton("Fotoğrafı Sil", Color.White, Petrol, 0, 0, 105);
            btnFotografSil.FlatAppearance.BorderSize = 1;
            btnFotografSil.FlatAppearance.BorderColor = Petrol;
            kartForm.Controls.Add(picProfil);
            kartForm.Controls.Add(btnFotografSec);
            kartForm.Controls.Add(btnFotografSil);
            btnFotografSec.Click += BtnFotografSec_Click;
            btnFotografSil.Click += (s, e) => FotografAyarla(null);

            kartForm.Controls.AddRange(new Control[] { btnEkle, btnGuncelle, btnSil, btnTemizle });

            kartListe = new Panel { Location = new Point(22, 310), BackColor = Color.White };
            Controls.Add(kartListe);
            Label listeBaslik = new Label
            {
                Text = "Kayıtlı Kullanıcılar", Location = new Point(18, 14), AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Petrol
            };
            kartListe.Controls.Add(listeBaslik);

            txtAra = new TextBox
            {
                Location = new Point(210, 14), Size = new Size(220, 27),
                Font = new Font("Segoe UI", 9), BorderStyle = BorderStyle.FixedSingle
            };
            kartListe.Controls.Add(txtAra);
            btnYenile = Buton("Yenile", Color.White, Petrol, 0, 10, 85);
            btnYenile.FlatAppearance.BorderSize = 1;
            btnYenile.FlatAppearance.BorderColor = Petrol;
            kartListe.Controls.Add(btnYenile);

            dgvKullanicilar = new DataGridView
            {
                Location = new Point(18, 52),
                ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                MultiSelect = false, RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.FixedSingle,
                CellBorderStyle = DataGridViewCellBorderStyle.Single,
                GridColor = Kenarlik, EnableHeadersVisualStyles = false,
                AllowUserToResizeRows = false
            };
            dgvKullanicilar.ColumnHeadersDefaultCellStyle.BackColor = Petrol;
            dgvKullanicilar.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvKullanicilar.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvKullanicilar.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvKullanicilar.DefaultCellStyle.ForeColor = Yazi;
            dgvKullanicilar.DefaultCellStyle.SelectionBackColor = Color.FromArgb(253, 227, 223);
            dgvKullanicilar.DefaultCellStyle.SelectionForeColor = Petrol;
            dgvKullanicilar.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 249, 251);
            dgvKullanicilar.RowTemplate.Height = 32;
            dgvKullanicilar.ColumnHeadersHeight = 36;
            dgvKullanicilar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            kartListe.Controls.Add(dgvKullanicilar);

            btnEkle.Click += BtnEkle_Click;
            btnGuncelle.Click += BtnGuncelle_Click;
            btnSil.Click += BtnSil_Click;
            btnTemizle.Click += (s, e) => FormuTemizle();
            btnYenile.Click += (s, e) => Yenile();
            txtAra.TextChanged += (s, e) => Filtrele();
            dgvKullanicilar.SelectionChanged += DgvKullanicilar_SelectionChanged;
            BoyutlariAyarla();
            FormuTemizle();
        }

        private TextBox AlanOlustur(Control parent, string etiket, int x, int y)
        {
            Label label = new Label
            {
                Text = etiket, Location = new Point(x, y), AutoSize = true,
                Font = new Font("Segoe UI", 9), ForeColor = Yazi
            };
            TextBox text = new TextBox
            {
                Location = new Point(x, y + 22), Size = new Size(210, 29),
                Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(label);
            parent.Controls.Add(text);
            return text;
        }

        private Button Buton(string yazi, Color arka, Color on, int x, int y, int genislik)
        {
            Button b = new Button
            {
                Text = yazi, Location = new Point(x, y), Size = new Size(genislik, 34),
                BackColor = arka, ForeColor = on, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void BoyutlariAyarla()
        {
            if (kartForm == null || kartListe == null || dgvKullanicilar == null) return;
            int genislik = Math.Max(460, ClientSize.Width - 44);
            kartForm.Width = genislik;
            kartForm.Height = 205;
            kartListe.Left = 22;
            kartListe.Top = 300;
            kartListe.Width = genislik;
            kartListe.Height = Math.Max(300, ClientSize.Height - 322);
            kartForm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            kartListe.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            int butonTop = 166;
            picProfil.Location = new Point(Math.Max(470, genislik - 250), 48);
            btnFotografSec.Location = new Point(picProfil.Right + 10, 55);
            btnFotografSil.Location = new Point(picProfil.Right + 10, 96);
            int sag = genislik - 18;
            btnTemizle.Location = new Point(sag - btnTemizle.Width, butonTop);
            btnSil.Location = new Point(btnTemizle.Left - 10 - btnSil.Width, butonTop);
            btnGuncelle.Location = new Point(btnSil.Left - 10 - btnGuncelle.Width, butonTop);
            btnEkle.Location = new Point(btnGuncelle.Left - 10 - btnEkle.Width, butonTop);
            btnYenile.Location = new Point(genislik - 103, 10);
            txtAra.Width = Math.Max(130, genislik - 560);
            dgvKullanicilar.Width = genislik - 36;
            dgvKullanicilar.Height = Math.Max(220, kartListe.Height - 68);
        }

        public void Yenile()
        {
            try
            {
                tabloYukleniyor = true;
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                using (SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT KullaniciID, AdSoyad, KullaniciAdi, Rol, KayitTarihi, ProfilFotografi FROM Kullanicilar ORDER BY AdSoyad", baglanti))
                {
                    DataTable tablo = new DataTable();
                    adapter.Fill(tablo);
                    dgvKullanicilar.DataSource = tablo;
                }
                if (dgvKullanicilar.Columns.Contains("KullaniciID")) dgvKullanicilar.Columns["KullaniciID"].Visible = false;
                if (dgvKullanicilar.Columns.Contains("ProfilFotografi")) dgvKullanicilar.Columns["ProfilFotografi"].Visible = false;
                if (dgvKullanicilar.Columns.Contains("AdSoyad")) dgvKullanicilar.Columns["AdSoyad"].HeaderText = "Ad Soyad";
                if (dgvKullanicilar.Columns.Contains("KullaniciAdi")) dgvKullanicilar.Columns["KullaniciAdi"].HeaderText = "Kullanıcı Adı";
                if (dgvKullanicilar.Columns.Contains("Rol")) dgvKullanicilar.Columns["Rol"].HeaderText = "Yetki";
                if (dgvKullanicilar.Columns.Contains("KayitTarihi"))
                {
                    dgvKullanicilar.Columns["KayitTarihi"].HeaderText = "Kayıt Tarihi";
                    dgvKullanicilar.Columns["KayitTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
                Filtrele();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Kullanıcılar yüklenemedi:\n" + ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { tabloYukleniyor = false; }
        }

        private void Filtrele()
        {
            DataTable tablo = dgvKullanicilar.DataSource as DataTable;
            if (tablo == null) return;
            string arama = (txtAra.Text ?? "").Trim().Replace("'", "''");
            tablo.DefaultView.RowFilter = string.IsNullOrEmpty(arama)
                ? ""
                : "AdSoyad LIKE '%" + arama + "%' OR KullaniciAdi LIKE '%" + arama + "%' OR Rol LIKE '%" + arama + "%'";
        }

        private void DgvKullanicilar_SelectionChanged(object sender, EventArgs e)
        {
            if (tabloYukleniyor || dgvKullanicilar.CurrentRow == null || !dgvKullanicilar.CurrentRow.Selected) return;
            DataGridViewRow satir = dgvKullanicilar.CurrentRow;
            if (satir.Cells["KullaniciID"].Value == null) return;
            seciliKullaniciID = Convert.ToInt32(satir.Cells["KullaniciID"].Value);
            txtAdSoyad.Text = Convert.ToString(satir.Cells["AdSoyad"].Value);
            txtKullaniciAdi.Text = Convert.ToString(satir.Cells["KullaniciAdi"].Value);
            cmbRol.SelectedItem = Convert.ToString(satir.Cells["Rol"].Value);
            txtSifre.Clear();
            object foto = satir.Cells["ProfilFotografi"].Value;
            FotografAyarla(foto == null || foto == DBNull.Value ? null : (byte[])foto);
            btnEkle.Enabled = false;
            btnGuncelle.Enabled = true;
        }

        private bool AlanlariKontrolEt(bool yeniKayit)
        {
            if (string.IsNullOrWhiteSpace(txtAdSoyad.Text) || string.IsNullOrWhiteSpace(txtKullaniciAdi.Text))
            { MessageBox.Show("Ad soyad ve kullanıcı adı boş bırakılamaz."); return false; }
            if (txtKullaniciAdi.Text.Trim().Length < 3)
            { MessageBox.Show("Kullanıcı adı en az 3 karakter olmalı."); return false; }
            if (yeniKayit && txtSifre.Text.Length < 4)
            { MessageBox.Show("Yeni kullanıcının şifresi en az 4 karakter olmalı."); return false; }
            if (!string.IsNullOrEmpty(txtSifre.Text) && txtSifre.Text.Length < 4)
            { MessageBox.Show("Şifre en az 4 karakter olmalı."); return false; }
            return true;
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (!AlanlariKontrolEt(true)) return;
            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                using (SqlCommand komut = new SqlCommand(@"INSERT INTO Kullanicilar (AdSoyad, KullaniciAdi, Sifre, Rol, ProfilFotografi)
                    VALUES (@ad, @kullanici, @sifre, @rol, @foto)", baglanti))
                {
                    komut.Parameters.Add("@ad", SqlDbType.NVarChar, 100).Value = txtAdSoyad.Text.Trim();
                    komut.Parameters.Add("@kullanici", SqlDbType.NVarChar, 50).Value = txtKullaniciAdi.Text.Trim();
                    komut.Parameters.Add("@sifre", SqlDbType.NVarChar, 100).Value = txtSifre.Text;
                    komut.Parameters.Add("@rol", SqlDbType.NVarChar, 30).Value = cmbRol.SelectedItem.ToString();
                    komut.Parameters.Add("@foto", SqlDbType.VarBinary, -1).Value = (object)seciliFotograf ?? DBNull.Value;
                    baglanti.Open(); komut.ExecuteNonQuery();
                }
                MessageBox.Show("Yeni profil oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Yenile(); FormuTemizle();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) MessageBox.Show("Bu kullanıcı adı zaten kullanılıyor.");
                else MessageBox.Show("Profil eklenemedi:\n" + ex.Message);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (seciliKullaniciID == 0) { MessageBox.Show("Önce tablodan kullanıcı seçin."); return; }
            if (!AlanlariKontrolEt(false)) return;
            if (seciliKullaniciID == Oturum.KullaniciID && cmbRol.SelectedItem.ToString() != Oturum.Rol)
            { MessageBox.Show("Kendi yönetici yetkinizi bu ekrandan değiştiremezsiniz."); return; }
            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    if (cmbRol.SelectedItem.ToString() != "Yonetici" && SonYoneticiMi(seciliKullaniciID, baglanti))
                    { MessageBox.Show("Sistemde en az bir yönetici bulunmalıdır."); return; }
                    string sql = @"UPDATE Kullanicilar SET AdSoyad=@ad, KullaniciAdi=@kullanici, Rol=@rol,
                                   Sifre=CASE WHEN @sifre=N'' THEN Sifre ELSE @sifre END, ProfilFotografi=@foto WHERE KullaniciID=@id";
                    using (SqlCommand komut = new SqlCommand(sql, baglanti))
                    {
                        komut.Parameters.Add("@ad", SqlDbType.NVarChar, 100).Value = txtAdSoyad.Text.Trim();
                        komut.Parameters.Add("@kullanici", SqlDbType.NVarChar, 50).Value = txtKullaniciAdi.Text.Trim();
                        komut.Parameters.Add("@rol", SqlDbType.NVarChar, 30).Value = cmbRol.SelectedItem.ToString();
                        komut.Parameters.Add("@sifre", SqlDbType.NVarChar, 100).Value = txtSifre.Text;
                        komut.Parameters.Add("@foto", SqlDbType.VarBinary, -1).Value = (object)seciliFotograf ?? DBNull.Value;
                        komut.Parameters.Add("@id", SqlDbType.Int).Value = seciliKullaniciID;
                        komut.ExecuteNonQuery();
                    }
                }
                if (seciliKullaniciID == Oturum.KullaniciID)
                {
                    Oturum.AdSoyad = txtAdSoyad.Text.Trim();
                    Oturum.Rol = cmbRol.SelectedItem.ToString();
                    Oturum.ProfilFotografi = seciliFotograf;
                    AnaForm ana = FindForm() as AnaForm;
                    if (ana != null) ana.ProfilFotografiniGuncelle(seciliFotograf);
                }
                MessageBox.Show("Profil bilgileri güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Yenile(); FormuTemizle();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) MessageBox.Show("Bu kullanıcı adı zaten kullanılıyor.");
                else MessageBox.Show("Profil güncellenemedi:\n" + ex.Message);
            }
        }

        private bool SonYoneticiMi(int id, SqlConnection baglanti)
        {
            using (SqlCommand komut = new SqlCommand("SELECT COUNT(*) FROM Kullanicilar WHERE Rol=N'Yonetici' AND KullaniciID<>@id", baglanti))
            { komut.Parameters.Add("@id", SqlDbType.Int).Value = id; return Convert.ToInt32(komut.ExecuteScalar()) == 0; }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (seciliKullaniciID == 0) { MessageBox.Show("Silmek için tablodan kullanıcı seçin."); return; }
            if (seciliKullaniciID == Oturum.KullaniciID) { MessageBox.Show("Giriş yaptığınız yönetici hesabını silemezsiniz."); return; }
            if (MessageBox.Show("Seçili profil silinsin mi?", "Profil Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    using (SqlCommand kontrol = new SqlCommand("SELECT Rol FROM Kullanicilar WHERE KullaniciID=@id", baglanti))
                    {
                        kontrol.Parameters.Add("@id", SqlDbType.Int).Value = seciliKullaniciID;
                        object rol = kontrol.ExecuteScalar();
                        if (rol != null && rol.ToString() == "Yonetici" && SonYoneticiMi(seciliKullaniciID, baglanti))
                        { MessageBox.Show("Sistemde en az bir yönetici bulunmalıdır."); return; }
                    }
                    using (SqlCommand komut = new SqlCommand("DELETE FROM Kullanicilar WHERE KullaniciID=@id", baglanti))
                    { komut.Parameters.Add("@id", SqlDbType.Int).Value = seciliKullaniciID; komut.ExecuteNonQuery(); }
                }
                MessageBox.Show("Profil silindi."); Yenile(); FormuTemizle();
            }
            catch (SqlException ex) { MessageBox.Show("Profil silinemedi:\n" + ex.Message); }
        }

        private void BtnFotografSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog sec = new OpenFileDialog())
            {
                sec.Title = "Profil fotoğrafı seç";
                sec.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (sec.ShowDialog() != DialogResult.OK) return;
                try
                {
                    using (Image kaynak = Image.FromFile(sec.FileName))
                    using (Bitmap kopya = new Bitmap(kaynak))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        kopya.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        FotografAyarla(ms.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fotoğraf açılamadı: " + ex.Message);
                }
            }
        }

        private void FotografAyarla(byte[] veri)
        {
            seciliFotograf = veri;
            if (picProfil == null) return;
            Image eski = picProfil.Image;
            picProfil.Image = null;
            if (eski != null) eski.Dispose();
            if (veri != null && veri.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(veri))
                using (Image kaynak = Image.FromStream(ms))
                    picProfil.Image = new Bitmap(kaynak);
            }
        }

        private void FormuTemizle()
        {
            seciliKullaniciID = 0;
            txtAdSoyad.Clear(); txtKullaniciAdi.Clear(); txtSifre.Clear();
            FotografAyarla(null);
            cmbRol.SelectedIndex = 0;
            dgvKullanicilar.ClearSelection();
            btnEkle.Enabled = true; btnGuncelle.Enabled = false;
        }
    }
}
