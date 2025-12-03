import random
import time
from abc import ABC, abstractmethod

# 1. ÖZEL HATA YÖNETİMİ
class KuantumCokusuException(Exception):
    def __init__(self, id_val):
        self.message = f"SİSTEM ÇÖKTÜ! Nesne {id_val} kararsız hale geldi ve patladı!"
        super().__init__(self.message)

# 2. ARAYÜZ (INTERFACE) - Python'da Abstract Class olarak simüle edilir
class IKritik(ABC):
    @abstractmethod
    def acil_durum_sogutmasi(self):
        pass

# 3. SOYUT SINIF (ABSTRACT CLASS)
class KuantumNesnesi(ABC):
    def __init__(self, id_val, tehlike):
        self.id = id_val
        self._stabilite = 100.0
        # Setter metodunu tetiklemek için doğrudan atama yapıyoruz
        self.tehlike_seviyesi = tehlike

    @property
    def stabilite(self):
        return self._stabilite

    @stabilite.setter
    def stabilite(self, value):
        # Kapsülleme: Hata vermek yerine değerleri sınırlara çekiyoruz (Clamp)
        if value > 100: self._stabilite = 100
        elif value < 0: self._stabilite = 0
        else: self._stabilite = value

    @property
    def tehlike_seviyesi(self):
        return self._tehlike_seviyesi

    @tehlike_seviyesi.setter
    def tehlike_seviyesi(self, value):
        # Kapsülleme: 1-10 arası sabitleme
        if value < 1: self._tehlike_seviyesi = 1
        elif value > 10: self._tehlike_seviyesi = 10
        else: self._tehlike_seviyesi = value

    @abstractmethod
    def analiz_et(self):
        pass

    def durum_bilgisi(self):
        return f"ID: {self.id} | Stabilite: %{self.stabilite} | Tehlike: {self.tehlike_seviyesi}"

# 4. SOMUT SINIFLAR

class VeriPaketi(KuantumNesnesi):
    def __init__(self, id_val):
        super().__init__(id_val, 1)

    def analiz_et(self):
        self.stabilite -= 5
        print(f">>> {self.id} Veri içeriği okundu. (-5 Stabilite)")
        if self.stabilite <= 0: raise KuantumCokusuException(self.id)

# Çoklu Kalıtım ile Interface Uygulaması
class KaranlikMadde(KuantumNesnesi, IKritik):
    def __init__(self, id_val):
        super().__init__(id_val, 5)

    def analiz_et(self):
        self.stabilite -= 15
        print(f">>> {self.id} Karanlık madde analizi tamamlandı. (-15 Stabilite)")
        if self.stabilite <= 0: raise KuantumCokusuException(self.id)

    def acil_durum_sogutmasi(self):
        self.stabilite += 50
        print(f">>> {self.id} soğutuldu. Yeni Stabilite: {self.stabilite}")

class AntiMadde(KuantumNesnesi, IKritik):
    def __init__(self, id_val):
        super().__init__(id_val, 9)

    def analiz_et(self):
        self.stabilite -= 25
        print(f">>> {self.id} Evrenin dokusu titriyor... (-25 Stabilite)")
        if self.stabilite <= 0: raise KuantumCokusuException(self.id)
    
    def acil_durum_sogutmasi(self):
        self.stabilite += 50
        print(f">>> {self.id} yoğun soğutma uygulandı. Yeni Stabilite: {self.stabilite}")

# 5. MAIN LOOP
def main():
    envanter = []
    
    try:
        while True:
            print("\n=====================================")
            print("--- KUANTUM AMBARI KONTROL PANELİ ---")
            print("1. Yeni Nesne Ekle")
            print("2. Tüm Envanteri Listele")
            print("3. Nesneyi Analiz Et")
            print("4. Acil Durum Soğutması Yap")
            print("5. Çıkış")
            print("=====================================")
            
            # .strip() ile boşlukları temizliyoruz
            secim = input("Seçiminiz: ").strip()

            if secim == "1":
                yeni_id = f"NESNE-{random.randint(1000, 9999)}"
                tur = random.randint(1, 3)
                
                if tur == 1: envanter.append(VeriPaketi(yeni_id))
                elif tur == 2: envanter.append(KaranlikMadde(yeni_id))
                else: envanter.append(AntiMadde(yeni_id))
                
                print(f"\n[+] Yeni nesne eklendi: {yeni_id}")

            elif secim == "2":
                print("\n--- ENVANTER RAPORU ---")
                if not envanter:
                    print("Envanter boş.")
                for nesne in envanter:
                    print(nesne.durum_bilgisi())

            elif secim == "3":
                aid = input("Analiz edilecek ID: ").strip()
                # Python'da LINQ yerine list comprehension veya next() kullanılır
                nesne = next((x for x in envanter if x.id == aid), None)
                
                if nesne:
                    print("\n--- ANALİZ BAŞLIYOR ---")
                    nesne.analiz_et()
                else:
                    print("\n[!] HATA: Nesne bulunamadı.")

            elif secim == "4":
                sid = input("Soğutulacak ID: ").strip()
                nesne = next((x for x in envanter if x.id == sid), None)
                
                if nesne:
                    # Type Checking (isinstance)
                    if isinstance(nesne, IKritik):
                        print("\n--- SOĞUTMA BAŞLATILDI ---")
                        nesne.acil_durum_sogutmasi()
                    else:
                        print("\n[!] HATA: Bu nesne soğutulamaz! (IKritik değil)")
                else:
                    print("\n[!] HATA: Nesne bulunamadı.")

            elif secim == "5":
                break
                
            else:
                print("Geçersiz seçim.")

    except KuantumCokusuException as e:
        print("\n*********************************")
        print("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...")
        print(e)
        print("*********************************")

if __name__ == "__main__":
    main()