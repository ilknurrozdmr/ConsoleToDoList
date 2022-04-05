using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoUygulaması
{
    public class Board
    {
        Dictionary<int, string> kisiler = new Dictionary<int, string>();

        List<Kart> TODO = new List<Kart>();
        List<Kart> INPROGRESS = new List<Kart>();
        List<Kart> DONE = new List<Kart>();

        public Board()
        {
            kisiler.Add(0, "İlknur Özdemir");
            kisiler.Add(1, "Zehra Kara");
            kisiler.Add(2, "Mahmut Dağlı");
            kisiler.Add(3, "Burhan Tepe");
            kisiler.Add(4, "Ceren Çakır");

            TODO.Add(new Kart("Proje", "Çalışmaya başla", "İlknur Özdemir", Kart.Buyukluk.M));
            INPROGRESS.Add(new Kart("Kitap", "Kitabı bitir", "Zehra Kara", Kart.Buyukluk.S));
            DONE.Add(new Kart("Spor", "Egzersiz tamamlandı", "Mahmut Dağlı", Kart.Buyukluk.L));
            DONE.Add(new Kart("Proje", "Canlıya çıkarıldı", "Burhan Tepe", Kart.Buyukluk.XS));
            INPROGRESS.Add(new Kart("Toplantı", "Toplantılara Katıl", "Zehra Çakır", Kart.Buyukluk.XL));
        }

        public void KartEkle()
        {
            string baslik;
            string icerik;
            int buyukluk;
            int kisi;

            Console.WriteLine("Başlık giriniz: ");
            baslik = Console.ReadLine();
            Console.WriteLine("İçerik giriniz: ");
            icerik = Console.ReadLine();
            Console.WriteLine("Büyüklük seciniz -> XS(1),S(2),M(3),L(4),XL(5): ");
            buyukluk = int.Parse(Console.ReadLine());
            Console.WriteLine("Kişi ID'sini giriniz: ");
            kisi = int.Parse(Console.ReadLine());

            if (kisiler.ContainsKey(kisi) && buyukluk > 0 && buyukluk <= 5)
                TODO.Add(new Kart(baslik, icerik, kisiler[kisi], (Kart.Buyukluk)buyukluk));
            else
                Console.WriteLine("Hatalı giriş yaptınız!");
        }

        public void KartGuncelle()
        {
            Console.WriteLine("Kart Güncelle çalışıyor.");
        }

        public void KartSil()
        {
            string baslik;
            string icerik;

            Console.WriteLine("Öncelikle silmek istediğiniz kartı seçmeniz gerekiyor.");
            Console.WriteLine("Lütfen kartın başlıgını yazınız: ");
            baslik = Console.ReadLine();
            Console.WriteLine("Lütfen kartın içeriği yazınız: ");
            icerik = Console.ReadLine();

            bool bulundu = false;

            foreach (var kart in TODO.ToArray())
            {
                if (kart.Baslik == baslik && kart.Icerik == icerik)
                {
                    TODO.Remove(kart);
                    Console.WriteLine("Kart silindi.");
                    bulundu = true;
                }
            }

            foreach (var kart in INPROGRESS.ToArray()) // -> ToArray kullanmamizin nedeni: islemler sirasinda listeden silme yapilirsa compiler kalan verilerin index adreslerini duzgun bulamiyor/kayma oluyor. ToArray kullandigimizda list'in icindeki verileri baska bir array'a tasiyoruz ve gerekli islemleri arrayde yapiyoruz ve tekrar list'e donduruyoruz.
            {
                if (kart.Baslik == baslik && kart.Icerik == icerik)
                {
                    INPROGRESS.Remove(kart);
                    Console.WriteLine("Kart silindi.");
                    bulundu = true;
                }
            }

            foreach (var kart in DONE.ToArray())
            {
                if (kart.Baslik == baslik && kart.Icerik == icerik)
                {
                    DONE.Remove(kart);
                    Console.WriteLine("Kart silindi.");
                    bulundu = true;
                }
            }

            if (!bulundu)
            {
                int secim;
                Console.WriteLine("Aradığınız kriterlere uygun kart bulunamadı.");
                Console.WriteLine("* Silmeyi sonlandirmak için (1)");
                Console.WriteLine("* Yeniden denemek için (2)");
                secim = int.Parse(Console.ReadLine());
                if (secim == 2)
                    KartSil();
                else
                    Console.WriteLine("Kart silme işlemi sonlandı.");
            }
        }

        private void KartEkle(Kart kart, ref List<Kart> addList, ref List<Kart> deleteList)
        {
            addList.Add(kart);
            deleteList.Remove(kart);
            Console.WriteLine("Taşıma işlemi basarılı!");
        }

        private void KartAra(string baslik, string icerik, ref List<Kart> kartListesi, ref bool bulundu, string listName)
        {
            foreach (var kart in kartListesi.ToArray())
            {
                if (kart.Baslik == baslik && kart.Icerik == icerik)
                {
                    bulundu = true;

                    Console.WriteLine("Bulunan Kart Bilgileri:");
                    Console.WriteLine("*******************************************");
                    Console.WriteLine("Başlik       :   {0}", kart.Baslik);
                    Console.WriteLine("İçerik       :   {0}", kart.Icerik);
                    Console.WriteLine("Atanan Kişi  :   {0}", kart.AtananKisi);
                    Console.WriteLine("Büyüklük     :   {0}", kart.Boyut);
                    Console.WriteLine("Line         :   {0}", listName);
                    Console.WriteLine();
                    Console.WriteLine("Lütfen taşımak istediğiniz Line'i secin:");
                    Console.WriteLine("(1) TODO");
                    Console.WriteLine("(2) IN PROGRESS");
                    Console.WriteLine("(3) DONE");
                    int secim = int.Parse(Console.ReadLine());
                    switch (secim)
                    {
                        case 1:
                            KartEkle(kart, ref TODO, ref kartListesi);
                            break;
                        case 2:
                            KartEkle(kart, ref INPROGRESS, ref kartListesi);
                            break;
                        case 3:
                            KartEkle(kart, ref DONE, ref kartListesi);
                            break;
                        default:
                            Console.WriteLine("Hatalı bir seçim yaptınız!");
                            break;
                    }
                }
            }
        }

        public void KartTasi()
        {
            string baslik;
            string icerik;
            bool bulundu = false;

            Console.WriteLine("Öncelikle taşımak istediğiniz kartı seçmeniz gerekiyor.");
            Console.WriteLine("Lütfen karton başlığını yazınız :    ");
            baslik = Console.ReadLine();
            Console.WriteLine("Lütfen kartın içeriğini yazınız :    ");
            icerik = Console.ReadLine();


            KartAra(baslik, icerik, ref TODO, ref bulundu, "TODO");              // List isimlerini string olarak göndermemin nedeni
            KartAra(baslik, icerik, ref INPROGRESS, ref bulundu, "INPROGRESS");  // referansları alındıktan sonra isimlerini yazdırmaya çalıştığımda
            KartAra(baslik, icerik, ref DONE, ref bulundu, "DONE");             // doğru değerleri vermemesidir.


            if (!bulundu)
            {
                int secim;
                Console.WriteLine("Aradığınız kriterlere uygun kart bulunamadı.");
                Console.WriteLine("* Taşımayı sonlandirmak için (1)");
                Console.WriteLine("* Yeniden denemek icin (2)");
                secim = int.Parse(Console.ReadLine());
                if (secim == 2)
                    KartTasi();
                else
                    Console.WriteLine("Kart silme işlemi sonlandırıldı.");
            }
        }

        public void BoardListele()
        {
            Console.WriteLine();
            Console.WriteLine("TODO Line");
            Console.WriteLine("*****************************");

            foreach (var kart in TODO)
            {
                Console.WriteLine("Başlık       : {0}", kart.Baslik);
                Console.WriteLine("İçerik       : {0}", kart.Icerik);
                Console.WriteLine("Atanan Kişi  : {0}", kart.AtananKisi);
                Console.WriteLine("Büyüklük     : {0}", kart.Boyut);
                Console.WriteLine("-");
            }

            Console.WriteLine();
            Console.WriteLine("IN PROGRESS Line");
            Console.WriteLine("*****************************");

            foreach (var kart in INPROGRESS)
            {
                Console.WriteLine("Başlık       : {0}", kart.Baslik);
                Console.WriteLine("İçerik       : {0}", kart.Icerik);
                Console.WriteLine("Atanan Kişi  : {0}", kart.AtananKisi);
                Console.WriteLine("Büyüklük     : {0}", kart.Boyut);
                Console.WriteLine("-");
            }

            Console.WriteLine();
            Console.WriteLine("DONE Line");
            Console.WriteLine("*****************************");

            foreach (var kart in DONE)
            {
                Console.WriteLine("Başlık       : {0}", kart.Baslik);
                Console.WriteLine("İçerik       : {0}", kart.Icerik);
                Console.WriteLine("Atanan Kişi  : {0}", kart.AtananKisi);
                Console.WriteLine("Büyüklük     : {0}", kart.Boyut);
                Console.WriteLine("-");
            }

        }

    }
}


