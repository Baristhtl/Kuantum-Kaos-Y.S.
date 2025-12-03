import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.Scanner;

//Özel hata yönetimi
class KuantumCokusuException extends RuntimeException {
    public KuantumCokusuException(String id) {
        super("SİSTEM ÇÖKTÜ! Nesne"+ id+ "kararsız hale geldi ve patladı!");

    }
}
// Kritik sistemler için arayüz
interface IKritik{
    void acilDurumSogutmasi();
}
// Temel Kuantum Nesnesi Sınıfı
abstract class KuantumNesnesi {
    protected String id;
    protected double stabilite;
    protected int tehlikeSeviyesi;

    public KuantumNesnesi(String id , int tehlike) {
        this.id = id;
        setTehlikeSeviyesi(tehlike); 
        this.stabilite = 100;
    }
    public String getId() {
        return id;
    }

    public void setStabilite(double value) {
       if(value>100){
        this.stabilite=100;
        }
         else if(value<0){
            this.stabilite=0;}
        else{
            this.stabilite = value;
        }
    }
    public double getStabilite() {
        return stabilite;
    }

    public int getTehlikeSeviyesi() {
        return tehlikeSeviyesi;
    }
    public void setTehlikeSeviyesi(int value) {
        if(value<1){
            this.tehlikeSeviyesi=1;
        }
        else if(value>10){
            this.tehlikeSeviyesi=10;
        }
        else{
            this.tehlikeSeviyesi = value;
        }
    }
    public abstract void analizEt();
    public String DurumBilgisi(){
        return "Nesne ID: " + id + ", Stabilite: " + stabilite + ", Tehlike Seviyesi: " + tehlikeSeviyesi;
    }
}
//Somut sınıflar
class VeriPaketi extends KuantumNesnesi
{
    public VeriPaketi(String id) {
        super(id,1);
    }
    @Override
    public void analizEt() {
        setStabilite(getStabilite()-5);
        System.out.println(">>>"+id+" veri paketi analiz ediliyor. Stabilite -5 azaldı.");
        if(getStabilite()<=0){
            throw new KuantumCokusuException(id);
        }
    }
}
class KaranlikMadde extends KuantumNesnesi implements IKritik{
    public KaranlikMadde(String id ){
        super(id,5);
    }
    @Override
    public void analizEt() {
        setStabilite(getStabilite()-15);
        System.out.println(">>>"+id+" karanlık madde analiz ediliyor. Stabilite -15 azaldı. ");
        if (getStabilite()<=0){
            throw new KuantumCokusuException(id);
        }
    }
    @Override
    public void acilDurumSogutmasi(){
        setStabilite(getStabilite()+50);
        System.out.println(">>>"+id+" için acil durum soğutması uygulandı. Stabilite +50 arttı."    );
    }
}
class AntiMadde extends KuantumNesnesi implements IKritik{
    public AntiMadde(String id){
        super(id,9);
    }
    @Override
    public void analizEt() {
        setStabilite(getStabilite()-25);
        System.out.println(">>>"+id+" anti madde analiz ediliyor, Evrenin dokusu titriyor... (-25 Stabilite)");
        if(getStabilite()<=0){
            throw new KuantumCokusuException(id);
        }
    }
    @Override
    public void acilDurumSogutmasi(){
        setStabilite(getStabilite()+50);
        System.out.println(">>>"+id+" için yoğun soğutması uygulandı. Stabilite +70 arttı. \nYeni Stabilite:" + getStabilite()  );
    }
}
public class Main {
    public static void main(String[] args) {
        List<KuantumNesnesi> envanter = new ArrayList<>();
        Scanner scanner = new Scanner(System.in);
        Random rnd = new Random();
        boolean oyunDevam = true;

        try {
            while (oyunDevam) {
                System.out.println("\n=====================================");
                System.out.println("--- KUANTUM AMBARI KONTROL PANELİ ---");
                System.out.println("1. Yeni Nesne Ekle");
                System.out.println("2. Tüm Envanteri Listele");
                System.out.println("3. Nesneyi Analiz Et");
                System.out.println("4. Acil Durum Soğutması Yap");
                System.out.println("5. Çıkış");
                System.out.println("=====================================");
                System.out.print("Seçiminiz: ");
                
                String secim = scanner.nextLine().trim();

                switch (secim) {
                    case "1":
                        int tur = rnd.nextInt(3) + 1; // 1-3 arası rastgele
                        String yeniId = "NESNE-" + (rnd.nextInt(9000) + 1000);
                        
                        if (tur == 1) envanter.add(new VeriPaketi(yeniId));
                        else if (tur == 2) envanter.add(new KaranlikMadde(yeniId));
                        else envanter.add(new AntiMadde(yeniId));
                        
                        System.out.println("\n[+] Yeni nesne eklendi: " + yeniId);
                        break;

                    case "2":
                        System.out.println("\n--- ENVANTER RAPORU ---");
                        if (envanter.isEmpty()) System.out.println("Envanter boş.");
                        for (KuantumNesnesi nesne : envanter) {
                            System.out.println(nesne.DurumBilgisi());
                        }
                        break;

                    case "3":
                        System.out.print("Analiz edilecek ID: ");
                        String aId = scanner.nextLine().trim();
                        
                        KuantumNesnesi aNesne = null;
                        for(KuantumNesnesi n : envanter) {
                            if(n.getId().equals(aId)) {
                                aNesne = n;
                                break;
                            }
                        }

                        if (aNesne != null) {
                            System.out.println("\n--- ANALİZ BAŞLIYOR ---");
                            aNesne.analizEt();
                        } else {
                            System.out.println("\n[!] HATA: Nesne bulunamadı.");
                        }
                        break;

                    case "4":
                        System.out.print("Soğutulacak ID: ");
                        String sId = scanner.nextLine().trim();
                        
                        KuantumNesnesi sNesne = null;
                        for(KuantumNesnesi n : envanter) {
                            if(n.getId().equals(sId)) {
                                sNesne = n;
                                break;
                            }
                        }

                        if (sNesne != null) {
                            if (sNesne instanceof IKritik) {
                                System.out.println("\n--- SOĞUTMA BAŞLATILDI ---");
                                ((IKritik) sNesne).acilDurumSogutmasi();
                            } else {
                                System.out.println("\n[!] HATA: Bu nesne soğutulamaz! (IKritik değil)");
                            }
                        } else {
                            System.out.println("\n[!] HATA: Nesne bulunamadı.");
                        }
                        break;

                    case "5":
                        oyunDevam = false;
                        break;
                        
                    default:
                        System.out.println("Geçersiz seçim.");
                }
            }
            // DÜZELTME BURADA: Döngü bitince scanner kapatılıyor.
            scanner.close();

        } catch (KuantumCokusuException ex) {
            System.out.println("\n*********************************");
            System.out.println("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
            System.out.println(ex.getMessage());
            System.out.println("*********************************");
        }
    }
}