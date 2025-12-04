# ⚛️ Kuantum Kaos Yönetimi (Quantum Chaos Management)

Bu proje, "Omega Sektörü"ndeki Kuantum Veri Ambarı'nın yönetim simülasyonudur. Projenin temel amacı, **Nesne Yönelimli Programlama (OOP)** prensiplerini 4 farklı programlama dilinde (C#, Java, Python, JavaScript) uygulayarak göstermektir.

## 📜 Senaryo
Kuantum Veri Ambarı'nın yeni vardiya amiri olarak göreviniz:
1.  Depoya gelen kararsız maddeleri (Veri Paketi, Karanlık Madde, Anti Madde) kabul etmek.
2.  Bu maddeleri analiz etmek (Analiz işlemi stabiliteyi düşürür).
3.  Tehlikeli maddeleri patlamadan önce soğutmak.
4.  Stabilite 0'ın altına düşerse **Kuantum Çöküşü** gerçekleşir ve simülasyon biter.

## 🛠 Teknik Mimari ve OOP Prensipleri
Bu projede aşağıdaki teknik gereksinimler eksiksiz uygulanmıştır:

* **Abstract Class (Soyut Sınıf):** `KuantumNesnesi` ana sınıfı ile ortak özellikler tanımlanmıştır.
* **Encapsulation (Kapsülleme):** Stabilite (0-100) ve Tehlike Seviyesi (1-10) değerleri kontrol altına alınmıştır.
* **Inheritance (Kalıtım):** `VeriPaketi`, `KaranlikMadde` ve `AntiMadde` sınıfları ana sınıftan türetilmiştir.
* **Interface (Arayüz):** Sadece tehlikeli nesneler için `IKritik` arayüzü ve `AcilDurumSogutmasi()` metodu kullanılmıştır.
* **Polimorfizm:** Farklı türdeki nesneler tek bir liste üzerinde yönetilmiştir.
* **Custom Exception:** Oyunun sonlanması için `KuantumCokusuException` adında özel hata yönetimi yazılmıştır.

## 📂 Proje Yapısı
Kodlar 4 farklı dilde hazırlanmış ve ayrı klasörlenmiştir:

* **C#:** `.NET` ortamında çalışır.
* **Java:** JDK gerektirir.
* **Python:** `abc` modülü ile OOP yapısı kurulmuştur.
* **JavaScript:** Node.js ortamında çalışır.

## 🚀 Nasıl Çalıştırılır?

Projeyi bilgisayarınıza indirdikten sonra, ilgili dilin klasörüne gidip terminal üzerinden aşağıdaki komutları kullanabilirsiniz:

### 1. C# (CSharp Klasörü)
-dotnet run
### 2. Java (Java Klasörü)
-javac Main.java
-java Main
### 3. Python (Python Klasörü)
-python main.py
### 4. JavaScript (JavaScript Klasörü)
-node main.js
