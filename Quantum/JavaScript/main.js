////////////////////////////////////////////
//           BARIŞ TAHTALIOĞLU            //
//              TH-GROUP                  //
////////////////////////////////////////////

const readline = require('readline');

// Kullanıcıdan veri almak için arayüz
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

// Promise yapısı ile kullanıcıdan veri alma fonksiyonu (C#'taki Console.ReadLine gibi davranması için)
const sor = (soru) => {
    return new Promise((resolve) => {
        rl.question(soru, (cevap) => {
            resolve(cevap);
        });
    });
};

// 1. ÖZEL HATA YÖNETİMİ
class KuantumCokusuException extends Error {
    constructor(id) {
        super(`SİSTEM ÇÖKTÜ! Nesne ${id} kararsız hale geldi ve patladı!`);
        this.name = "KuantumCokusuException";
    }
}

// 2. SOYUT SINIF (ABSTRACT CLASS BENZERİ)
class KuantumNesnesi {
    constructor(id, tehlike) {
        this.id = id;
        this._stabilite = 100; // Başlangıç full
        this.tehlikeSeviyesi = tehlike; // Setter tetiklenir
    }

    // Stabilite Kapsülleme (Otomatik Düzeltme: 0-100 arası)
    get stabilite() { return this._stabilite; }
    set stabilite(val) {
        if (val > 100) this._stabilite = 100;
        else if (val < 0) this._stabilite = 0;
        else this._stabilite = val;
    }

    // Tehlike Seviyesi Kapsülleme (Otomatik Düzeltme: 1-10 arası)
    get tehlikeSeviyesi() { return this._tehlikeSeviyesi; }
    set tehlikeSeviyesi(val) {
        if (val < 1) this._tehlikeSeviyesi = 1;
        else if (val > 10) this._tehlikeSeviyesi = 10;
        else this._tehlikeSeviyesi = val;
    }

    // Soyut Metot (Override edilmezse hata verir)
    analizEt() {
        throw new Error("Abstract method 'analizEt' must be implemented.");
    }

    durumBilgisi() {
        return `ID: ${this.id} | Stabilite: %${this.stabilite} | Tehlike: ${this.tehlikeSeviyesi}`;
    }
}

// 3. SOMUT SINIFLAR

class VeriPaketi extends KuantumNesnesi {
    constructor(id) { super(id, 1); }

    analizEt() {
        this.stabilite -= 5;
        console.log(`>>> ${this.id} Veri içeriği okundu. (-5 Stabilite)`);
        if (this.stabilite <= 0) throw new KuantumCokusuException(this.id);
    }
}

class KaranlikMadde extends KuantumNesnesi {
    constructor(id) { super(id, 5); }

    analizEt() {
        this.stabilite -= 15;
        console.log(`>>> ${this.id} Karanlık madde analizi tamamlandı. (-15 Stabilite)`);
        if (this.stabilite <= 0) throw new KuantumCokusuException(this.id);
    }

    // IKritik metodunu implemente ediyoruz
    acilDurumSogutmasi() {
        this.stabilite += 50;
        console.log(`>>> ${this.id} soğutuldu. Yeni Stabilite: ${this.stabilite}`);
    }
}

class AntiMadde extends KuantumNesnesi {
    constructor(id) { super(id, 9); }

    analizEt() {
        this.stabilite -= 25;
        console.log(`>>> ${this.id} Evrenin dokusu titriyor... (-25 Stabilite)`);
        if (this.stabilite <= 0) throw new KuantumCokusuException(this.id);
    }

    // IKritik metodunu implemente ediyoruz
    acilDurumSogutmasi() {
        this.stabilite += 50;
        console.log(`>>> ${this.id} yoğun soğutma uygulandı. Yeni Stabilite: ${this.stabilite}`);
    }
}

// 4. MAIN LOOP (ASENKRON)
async function main() {
    const envanter = [];
    let oyunDevam = true;

    try {
        while (oyunDevam) {
            console.log("\n=====================================");
            console.log("--- KUANTUM AMBARI KONTROL PANELİ ---");
            console.log("1. Yeni Nesne Ekle");
            console.log("2. Tüm Envanteri Listele");
            console.log("3. Nesneyi Analiz Et");
            console.log("4. Acil Durum Soğutması Yap");
            console.log("5. Çıkış");
            console.log("=====================================");
            
            // Kullanıcıdan girdiyi al ve boşlukları temizle (.trim())
            let secim = await sor("Seçiminiz: ");
            secim = secim.trim();

            switch (secim) {
                case "1":
                    const rnd = Math.floor(Math.random() * 3) + 1;
                    const yeniId = "NESNE-" + (Math.floor(Math.random() * 9000) + 1000);
                    
                    if (rnd === 1) envanter.push(new VeriPaketi(yeniId));
                    else if (rnd === 2) envanter.push(new KaranlikMadde(yeniId));
                    else envanter.push(new AntiMadde(yeniId));
                    
                    console.log(`\n[+] Yeni nesne eklendi: ${yeniId}`);
                    break;

                case "2":
                    console.log("\n--- ENVANTER RAPORU ---");
                    if (envanter.length === 0) console.log("Envanter boş.");
                    envanter.forEach(nesne => {
                        console.log(nesne.durumBilgisi());
                    });
                    break;

                case "3":
                    let aId = await sor("Analiz edilecek ID: ");
                    aId = aId.trim();
                    
                    const aNesne = envanter.find(n => n.id === aId);
                    
                    if (aNesne) {
                        console.log("\n--- ANALİZ BAŞLIYOR ---");
                        aNesne.analizEt();
                    } else {
                        console.log("\n[!] HATA: Nesne bulunamadı.");
                    }
                    break;

                case "4":
                    let sId = await sor("Soğutulacak ID: ");
                    sId = sId.trim();
                    
                    const sNesne = envanter.find(n => n.id === sId);

                    if (sNesne) {
                        // JavaScript'te Interface olmadığı için metodun varlığını kontrol ediyoruz (Duck Typing)
                        if (typeof sNesne.acilDurumSogutmasi === 'function') {
                            console.log("\n--- SOĞUTMA BAŞLATILDI ---");
                            sNesne.acilDurumSogutmasi();
                        } else {
                            console.log("\n[!] HATA: Bu nesne soğutulamaz! (IKritik değil)");
                        }
                    } else {
                        console.log("\n[!] HATA: Nesne bulunamadı.");
                    }
                    break;

                case "5":
                    oyunDevam = false;
                    rl.close();
                    break;
                
                default:
                    console.log("Geçersiz seçim.");
            }
        }
    } catch (e) {
        // Hata yakalama (Custom Exception)
        if (e.name === "KuantumCokusuException") {
            console.log("\n*********************************");
            console.log("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
            console.log(e.message);
            console.log("*********************************");
        } else {
            console.log("Beklenmedik bir hata:", e);
        }
        rl.close();
    }
}

// Programı başlat

main();
