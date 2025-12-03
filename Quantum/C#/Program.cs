using System;
using System.Collections.Generic;

// 1. ÖZEL HATA YÖNETİMİ
public class KuantumCokusuException : Exception
{
    public KuantumCokusuException(string id) : base($"SİSTEM ÇÖKTÜÜÜÜ! Nesne {id} id'li nesne kararsız hale geldi ve patladı (stabilite 0 veya aşağısına düştü).") { }
}

// 2. ARAYÜZ
interface IKritik
{
    void AcilDurumSogutmasi();
}

// 3. SOYUT SINIF
public abstract class KuantumNesnesi
{
    public string ID { get; set; } = "";
    
    private double _stabilite;
    private int _tehlikeSeviyesi;

    // Stabilite Kapsülleme (Otomatik düzeltme: 0-100 arası)
    public double Stabilite
    {
        get { return _stabilite; }
        set
        {
            if (value > 100) _stabilite = 100;
            else if (value < 0) _stabilite = 0;
            else _stabilite = value;
        }
    }

    // Tehlike Kapsülleme (Otomatik düzeltme: 1-10 arası)
    public int TehlikeSeviyesi
    {
        get { return _tehlikeSeviyesi; }
        set
        {
            if (value < 1) _tehlikeSeviyesi = 1;
            else if (value > 10) _tehlikeSeviyesi = 10;
            else _tehlikeSeviyesi = value;
        }
    }

    public KuantumNesnesi(string id, int tehlikeSeviyesi)
    {
        ID = id;
        Stabilite = 100;
        TehlikeSeviyesi = tehlikeSeviyesi;
    }

    public abstract void AnalizEt();

    public string DurumBilgisi()
    {
        return $"ID: {ID} | Stabilite: %{Stabilite} | Tehlike: {TehlikeSeviyesi}";
    }
}

// 4. SOMUT SINIFLAR

public class VeriPaketi : KuantumNesnesi
{
    public VeriPaketi(string id) : base(id, 1) { }
    
    public override void AnalizEt()
    {
        Stabilite -= 5;
        Console.WriteLine($">>> {ID} Veri içeriği okundu. (-5 Stabilite)"); // Mesaj Garantilendi
        if (Stabilite <= 0) throw new KuantumCokusuException(ID);
    }
}

public class KaranlikMadde : KuantumNesnesi, IKritik
{
    public KaranlikMadde(string id) : base(id, 5) { }
    
    public override void AnalizEt()
    {
        Stabilite -= 15;
        Console.WriteLine($">>> {ID} Karanlık madde analizi tamamlandı. (-15 Stabilite)"); // Mesaj Garantilendi
        if (Stabilite <= 0) throw new KuantumCokusuException(ID);
    }
    
    public void AcilDurumSogutmasi()
    {
        Stabilite += 50;
        Console.WriteLine($">>> {ID} soğutuldu. Yeni Stabilite: {Stabilite}");
    }
}

public class AntiMadde : KuantumNesnesi, IKritik
{
    public AntiMadde(string id) : base(id, 9) { }

    public override void AnalizEt()
    {
        Stabilite -= 25;
        Console.WriteLine($">>> {ID} Evrenin dokusu titriyor... (-25 Stabilite)"); // Mesaj Garantilendi
        if (Stabilite <= 0) throw new KuantumCokusuException(ID);
    }
    
    public void AcilDurumSogutmasi()
    {
        Stabilite += 50;
        Console.WriteLine($">>> {ID} yoğun soğutma uygulandı. Yeni Stabilite: {Stabilite}");
    }
}

// 5. MAIN LOOP
class Program
{
    static void Main(string[] args)
    {
        List<KuantumNesnesi> envanter = new List<KuantumNesnesi>();
        Random rnd = new Random();
        bool oyunDevam = true;

        try
        {
            while (oyunDevam)
            {
                Console.WriteLine("\n=====================================");
                Console.WriteLine("--- KUANTUM AMBARI KONTROL PANELİ ---");
                Console.WriteLine("1. Yeni Nesne Ekle");
                Console.WriteLine("2. Tüm Envanteri Listele");
                Console.WriteLine("3. Nesneyi Analiz Et");
                Console.WriteLine("4. Acil Durum Soğutması Yap");
                Console.WriteLine("5. Çıkış");
                Console.WriteLine("=====================================");
                Console.Write("Seçiminiz: ");
                
                string secim = (Console.ReadLine() ?? "").Trim(); // Boşlukları temizle

                switch (secim)
                {
                    case "1":
                        int tur = rnd.Next(1, 4);
                        string yeniId = "NESNE-" + rnd.Next(1000, 9999);
                        if (tur == 1) envanter.Add(new VeriPaketi(yeniId));
                        else if (tur == 2) envanter.Add(new KaranlikMadde(yeniId));
                        else envanter.Add(new AntiMadde(yeniId));
                        Console.WriteLine($"\n[+] Yeni nesne eklendi: {yeniId}");
                        break;

                    case "2":
                        Console.WriteLine("\n--- ENVANTER RAPORU ---");
                        if (envanter.Count == 0) Console.WriteLine("Envanter boş.");
                        foreach (var nesne in envanter)
                        {
                            Console.WriteLine(nesne.DurumBilgisi());
                        }
                        break;

                    case "3":
                        Console.Write("Analiz edilecek ID: ");
                        // DÜZELTME: .Trim() eklendi (Boşluk hatasını çözer)
                        string aId = (Console.ReadLine() ?? "").Trim();
                        
                        var aNesne = envanter.Find(x => x.ID == aId);
                        
                        if (aNesne != null) 
                        {
                            Console.WriteLine("\n--- ANALİZ BAŞLIYOR ---");
                            aNesne.AnalizEt();
                        }
                        else 
                        {
                            Console.WriteLine("\n[!] HATA: Nesne bulunamadı.");
                        }
                        break;

                    case "4":
                        Console.Write("Soğutulacak ID: ");
                        // DÜZELTME: .Trim() eklendi
                        string sId = (Console.ReadLine() ?? "").Trim();
                        
                        var sNesne = envanter.Find(x => x.ID == sId);
                        
                        if (sNesne != null)
                        {
                            if (sNesne is IKritik kritikNesne)
                            {
                                Console.WriteLine("\n--- SOĞUTMA BAŞLATILDI ---");
                                kritikNesne.AcilDurumSogutmasi();
                            }
                            else
                            {
                                Console.WriteLine("\n[!] HATA: Bu nesne soğutulamaz! (IKritik değil)");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n[!] HATA: Nesne bulunamadı.");
                        }
                        break;

                    case "5":
                        oyunDevam = false;
                        break;
                    
                    default:
                        Console.WriteLine("Geçersiz seçim.");
                        break;
                }
            }
        }
        catch (KuantumCokusuException ex)
        {
            Console.WriteLine("\n*********************************");
            Console.WriteLine("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
            Console.WriteLine(ex.Message);
            Console.WriteLine("*********************************");
        }
    }
}