using System.Data.SqlClient;

namespace VetCare
{
    // Tüm formlar bağlantıyı buradan alır
    public static class Veritabani
    {
        // SQL Server Express kullanıyorsanız Server=.\SQLEXPRESS yapın
        public const string BaglantiMetni =
            "Server=WEBLAB05\\SQLEXPRESS; Database=VetCareDB; Integrated Security=True;";

        public static SqlConnection BaglantiAl()
        {
            return new SqlConnection(BaglantiMetni);
        }
    }

    // Giriş yapan kullanıcıyı tüm formlarda kullanmak için
    public static class Oturum
    {
        public static int KullaniciID { get; set; }
        public static string AdSoyad { get; set; }
        public static string Rol { get; set; }   // "Yonetici" veya "Personel"

        public static bool YoneticiMi
        {
            get { return Rol == "Yonetici"; }
        }
    }
}
