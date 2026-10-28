using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VetCare
{
    public partial class AnaForm : Form
    {
        /* TASARIMCIDA OLUŞTURULACAK KONTROLLER (Name):
           pnlMenu, pnlUst, pnlIcerik, pnlAnaPanel
           picDoktor, lblDoktorAdi, lblRol
           btnAnaPanel, btnHastaKabul, btnRandevuTakibi, btnAsiPlani,
           btnMuayeneKaydi, btnAyarlar, btnCikis
           lblSayfaBaslik, lblTarih
           lblRandevuSayi, prgRandevu, dgvAsilar
           txtHayvanAdi, txtSahipAdi, cmbTur, txtIrk, txtTelefon, btnKaydet, btnTemizle
        */

        private readonly Color Aktif = ColorTranslator.FromHtml("#F47F6E");
        private readonly Color Normal = ColorTranslator.FromHtml("#25505F");
        private Button[] menuButonlari;
        private HastaKabulSayfasi hastaKabul;

        public AnaForm()
        {
            InitializeComponent();

            menuButonlari = new Button[]
            {
                btnAnaPanel, btnHastaKabul, btnRandevuTakibi, btnAsiPlani, btnMuayeneKaydi, btnAyarlar
            };

            this.Load += AnaForm_Load;
            btnAnaPanel.Click += (s, e) => SayfaGoster(btnAnaPanel, pnlAnaPanel);
            btnHastaKabul.Click += (s, e) => HastaKabulAc();
            btnRandevuTakibi.Click += (s, e) => Hazirlaniyor(btnRandevuTakibi);
            btnAsiPlani.Click += (s, e) => Hazirlaniyor(btnAsiPlani);
            btnMuayeneKaydi.Click += (s, e) => Hazirlaniyor(btnMuayeneKaydi);
            btnAyarlar.Click += (s, e) => Hazirlaniyor(btnAyarlar);

            btnKaydet.Click += BtnKaydet_Click;
            btnTemizle.Click += (s, e) => Temizle();
            btnCikis.Click += BtnCikis_Click;
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            // Giriş yapan kullanıcı bilgisi
            lblDoktorAdi.Text = Oturum.AdSoyad;
            lblRol.Text = Oturum.YoneticiMi ? "Yönetici" : "Personel";
            lblTarih.Text = DateTime.Now.ToString("dd MMMM yyyy, dddd");

            // Ayarlar sadece yöneticiye görünsün
            btnAyarlar.Visible = Oturum.YoneticiMi;

            // Doktor fotoğrafı yuvarlak olsun
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, picDoktor.Width - 1, picDoktor.Height - 1);
            picDoktor.Region = new Region(gp);

            // Tür listesi
            cmbTur.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTur.Items.Clear();
            cmbTur.Items.AddRange(new object[] { "Kedi", "Köpek", "Kuş", "Tavşan", "Diğer" });

            TabloStiliVer(dgvAsilar);
            SayfaGoster(btnAnaPanel, pnlAnaPanel);
        }

        // ---------- SAYFA GEÇİŞLERİ ----------
        private void SayfaGoster(Button aktifButon, Control sayfa)
        {
            foreach (Button b in menuButonlari)
                b.BackColor = Normal;
            aktifButon.BackColor = Aktif;

            sayfa.BringToFront();

            if (sayfa == pnlAnaPanel)
                PanoyuYukle();
        }

        private void HastaKabulAc()
        {
            if (hastaKabul == null)
            {
                hastaKabul = new HastaKabulSayfasi();
                hastaKabul.Dock = DockStyle.Fill;
                pnlIcerik.Controls.Add(hastaKabul);
            }
            SayfaGoster(btnHastaKabul, hastaKabul);
            hastaKabul.Yenile();
        }

        private void Hazirlaniyor(Button b)
        {
            MessageBox.Show(b.Text + " sayfası sonraki adımda eklenecek.",
                "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ---------- ANA PANEL VERİLERİ ----------
        private void PanoyuYukle()
        {
            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();

                    // Bugünkü randevular: toplam ve tamamlanan
                    string sqlRandevu = @"SELECT COUNT(*) AS Toplam,
                                                 ISNULL(SUM(CASE WHEN Durum = N'Tamamlandı' THEN 1 ELSE 0 END), 0) AS Biten
                                          FROM Randevular
                                          WHERE CAST(RandevuTarihi AS DATE) = CAST(GETDATE() AS DATE)";
                    using (SqlCommand komut = new SqlCommand(sqlRandevu, baglanti))
                    using (SqlDataReader dr = komut.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            int toplam = Convert.ToInt32(dr["Toplam"]);
                            int biten = Convert.ToInt32(dr["Biten"]);
                            lblRandevuSayi.Text = biten + "/" + toplam;
                            prgRandevu.Maximum = Math.Max(toplam, 1);
                            prgRandevu.Value = Math.Min(biten, prgRandevu.Maximum);
                        }
                    }

                    // Önümüzdeki 30 gün içindeki aşılar
                    string sqlAsi = @"SELECT h.HayvanAdi AS [Hayvan],
                                             h.Tur AS [Tür],
                                             s.AdSoyad AS [Sahip],
                                             a.AsiAdi AS [Aşı],
                                             a.SonrakiAsiTarihi AS [Tarih]
                                      FROM Asilar a
                                      JOIN Hayvanlar h ON h.HayvanID = a.HayvanID
                                      JOIN Sahipler s ON s.SahipID = h.SahipID
                                      WHERE a.SonrakiAsiTarihi BETWEEN CAST(GETDATE() AS DATE)
                                                                   AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE))
                                      ORDER BY a.SonrakiAsiTarihi";
                    SqlDataAdapter da = new SqlDataAdapter(sqlAsi, baglanti);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAsilar.DataSource = dt;

                    if (dgvAsilar.Columns.Contains("Tarih"))
                        dgvAsilar.Columns["Tarih"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Veriler yüklenemedi:\n" + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- YENİ HASTA KAYDI ----------
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            string hayvanAdi = txtHayvanAdi.Text.Trim();
            string sahipAdi = txtSahipAdi.Text.Trim();

            // BR-01: Hayvan adı boş bırakılamaz
            if (hayvanAdi == "")
            {
                MessageBox.Show("Hayvan adı boş bırakılamaz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHayvanAdi.Focus();
                return;
            }
            if (sahipAdi == "")
            {
                MessageBox.Show("Sahip adı boş bırakılamaz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSahipAdi.Focus();
                return;
            }
            if (cmbTur.SelectedIndex < 0)
            {
                MessageBox.Show("Lütfen hayvanın türünü seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    // İki kayıt birlikte eklenir; biri hata verirse hiçbiri eklenmez
                    using (SqlTransaction islem = baglanti.BeginTransaction())
                    {
                        // 1) Sahip
                        SqlCommand k1 = new SqlCommand(
                            @"INSERT INTO Sahipler (AdSoyad, Telefon) VALUES (@ad, @tel);
                              SELECT CAST(SCOPE_IDENTITY() AS INT);", baglanti, islem);
                        k1.Parameters.AddWithValue("@ad", sahipAdi);
                        k1.Parameters.AddWithValue("@tel",
                            txtTelefon.Text.Trim() == "" ? (object)DBNull.Value : txtTelefon.Text.Trim());
                        int sahipId = (int)k1.ExecuteScalar();

                        // 2) Hayvan
                        SqlCommand k2 = new SqlCommand(
                            @"INSERT INTO Hayvanlar (SahipID, HayvanAdi, Tur, Irk)
                              VALUES (@sahip, @hayvan, @tur, @irk);", baglanti, islem);
                        k2.Parameters.AddWithValue("@sahip", sahipId);
                        k2.Parameters.AddWithValue("@hayvan", hayvanAdi);
                        k2.Parameters.AddWithValue("@tur", cmbTur.SelectedItem.ToString());
                        k2.Parameters.AddWithValue("@irk",
                            txtIrk.Text.Trim() == "" ? (object)DBNull.Value : txtIrk.Text.Trim());
                        k2.ExecuteNonQuery();

                        islem.Commit();
                    }
                }

                MessageBox.Show("Hasta kaydı eklendi.", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Kayıt eklenemedi:\n" + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Temizle()
        {
            txtHayvanAdi.Clear();
            txtSahipAdi.Clear();
            txtIrk.Clear();
            txtTelefon.Clear();
            cmbTur.SelectedIndex = -1;
            txtHayvanAdi.Focus();
        }

        // ---------- ÇIKIŞ ----------
        private void BtnCikis_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Çıkış yapmak istiyor musunuz?", "Çıkış",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
                Application.Restart();   // Giriş ekranına döner
        }

        // ---------- TABLO GÖRÜNÜMÜ ----------
        private void TabloStiliVer(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Normal;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#FDE3DF");
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 30;
        }
    }
}