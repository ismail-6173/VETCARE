using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VetCare
{
    public partial class LoginForm : Form
    {
        /* TASARIMCIDA OLUŞTURULACAK KONTROLLER (Name özelliği):
           txtKullaniciAdi   : TextBox
           txtSifre          : TextBox  (UseSystemPasswordChar = true)
           btnUyeGiris       : Button   Text = "Üye Girişi"
           btnYoneticiGiris  : Button   Text = "Yönetici Girişi"
           lnkYeniKayit      : LinkLabel Text = "Hesabın yok mu? Yeni Kayıt"
           (isteğe bağlı) chkSifreGoster : CheckBox
        */

        public LoginForm()
        {
            InitializeComponent();

            this.AcceptButton = btnUyeGiris;               // Enter = üye girişi
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            btnUyeGiris.Click += (s, e) => GirisYap("Personel");
            btnYoneticiGiris.Click += (s, e) => GirisYap("Yonetici");
            lnkYeniKayit.LinkClicked += LnkYeniKayit_LinkClicked;
        }

        private void GirisYap(string beklenenRol)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            if (kullaniciAdi == "" || sifre == "")
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection baglanti = Veritabani.BaglantiAl())
                {
                    baglanti.Open();
                    string sql = @"SELECT KullaniciID, AdSoyad, Rol
                                   FROM Kullanicilar
                                   WHERE KullaniciAdi = @ad AND Sifre = @sifre";

                    using (SqlCommand komut = new SqlCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@ad", kullaniciAdi);
                        komut.Parameters.AddWithValue("@sifre", sifre);

                        using (SqlDataReader dr = komut.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                MessageBox.Show("Kullanıcı adı veya şifre hatalı.",
                                    "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string rol = dr["Rol"].ToString();

                            // Yönetici butonuna basan kişi yönetici olmalı
                            if (beklenenRol == "Yonetici" && rol != "Yonetici")
                            {
                                MessageBox.Show("Bu hesap yönetici yetkisine sahip değil.",
                                    "Yetkisiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // Üye butonuna basan kişi üye (Personel) olmalı
                            if (beklenenRol == "Personel" && rol == "Yonetici")
                            {
                                MessageBox.Show("Yönetici hesabı için 'Yönetici Girişi' butonunu kullanın.",
                                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            Oturum.KullaniciID = Convert.ToInt32(dr["KullaniciID"]);
                            Oturum.AdSoyad = dr["AdSoyad"].ToString();
                            Oturum.Rol = rol;
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;   // Program.cs ana formu açar
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Veritabanına bağlanılamadı:\n" + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void chkSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            txtSifre.UseSystemPasswordChar = !chkSifreGoster.Checked;
        }

        private void LnkYeniKayit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (KayitForm kayit = new KayitForm())
            {
                if (kayit.ShowDialog() == DialogResult.OK)
                {
                    txtKullaniciAdi.Text = kayit.YeniKullaniciAdi;
                    txtSifre.Clear();
                    txtSifre.Focus();
                }
            }
        }

        private void btnYoneticiGiris_Click(object sender, EventArgs e)
        {

        }
    }
}
