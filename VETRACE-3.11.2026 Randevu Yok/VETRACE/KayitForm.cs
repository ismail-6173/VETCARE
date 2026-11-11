using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VetCare
{
    public partial class KayitForm : Form
    {
        /* TASARIMCIDA OLUŞTURULACAK KONTROLLER:
           txtAdSoyad          : TextBox
           txtKullaniciAdi     : TextBox
           txtSifre            : TextBox (UseSystemPasswordChar = true)
           txtSifreTekrar      : TextBox (UseSystemPasswordChar = true)
           btnKayitOl          : Button  Text = "Kayıt Ol"
           btnGeriDon          : Button  Text = "Girişe Dön"
        */

        public string YeniKullaniciAdi { get; private set; }

        public KayitForm()
        {
            InitializeComponent();

            this.AcceptButton = btnKayitOl;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            btnKayitOl.Click += BtnKayitOl_Click;
            btnGeriDon.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void BtnKayitOl_Click(object sender, EventArgs e)
        {
            string adSoyad = txtAdSoyad.Text.Trim();
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            if (adSoyad == "" || kullaniciAdi == "" || sifre == "")
            {
                Uyari("Tüm alanları doldurun.");
                return;
            }
            if (kullaniciAdi.Length < 3)
            {
                Uyari("Kullanıcı adı en az 3 karakter olmalı.");
                return;
            }
            if (sifre.Length < 4)
            {
                Uyari("Şifre en az 4 karakter olmalı.");
                return;
            }
            if (sifre != txtSifreTekrar.Text)
            {
                Uyari("Şifreler birbiriyle uyuşmuyor.");
                return;
            }

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();

                    // Rol DAİMA 'Personel' - dışarıdan yönetici kaydı açılamaz
                    string sql = @"INSERT INTO Kullanicilar (KullaniciAdi, Sifre, AdSoyad, Rol)
                                   VALUES (@ad, @sifre, @adsoyad, 'Personel')";

                    using (SqlCommand komut = new SqlCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@ad", kullaniciAdi);
                        komut.Parameters.AddWithValue("@sifre", sifre);
                        komut.Parameters.AddWithValue("@adsoyad", adSoyad);
                        komut.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kayıt başarılı! Şimdi giriş yapabilirsiniz.",
                    "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                YeniKullaniciAdi = kullaniciAdi;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SqlException ex)
            {
                // 2627 / 2601 = UNIQUE kısıtı ihlali (kullanıcı adı zaten var)
                if (ex.Number == 2627 || ex.Number == 2601)
                    Uyari("Bu kullanıcı adı zaten kullanılıyor.");
                else
                    MessageBox.Show("Veritabanı hatası:\n" + ex.Message,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Uyari(string mesaj)
        {
            MessageBox.Show(mesaj, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
