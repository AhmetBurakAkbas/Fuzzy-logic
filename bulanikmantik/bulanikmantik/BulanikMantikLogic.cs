// === BulanikMantikLogic.cs ===
using System;
using System.Collections.Generic;
using System.Linq;

namespace bulanikmantik
{
    public abstract class MembershipFunction
    {
        public abstract double GetMembership(double x);
    }

    public class TriangularMF : MembershipFunction
    {
        public double A, B, C;
        public TriangularMF(double a, double b, double c) { A = a; B = b; C = c; }
        public override double GetMembership(double x)
        {
            if (x <= A || x >= C) return 0;
            if (x == B) return 1;
            return x < B ? (x - A) / (B - A) : (C - x) / (C - B);
        }
    }

    public class TrapezoidalMF : MembershipFunction
    {
        public double A, B, C, D;
        public TrapezoidalMF(double a, double b, double c, double d) { A = a; B = b; C = c; D = d; }
        public override double GetMembership(double x)
        {
            if (x <= A || x >= D) return 0;
            if (x >= B && x <= C) return 1;
            return x < B ? (x - A) / (B - A) : (D - x) / (D - C);
        }
    }

    public class BulanikDegisken
    {
        public string Ad;
        public Dictionary<string, MembershipFunction> Kumeler = new Dictionary<string, MembershipFunction>();
        public BulanikDegisken(string ad) { Ad = ad; }
        public void KumeEkle(string etiket, MembershipFunction mf) => Kumeler[etiket] = mf;
        public double UyelikAl(string etiket, double x) => Kumeler[etiket].GetMembership(x);
    }

    public class BulanikKural
    {
        public (BulanikDegisken deg, string kume)[] Kosullar;
        public (BulanikDegisken deg, string kume)[] Sonuclar;
        public BulanikKural((BulanikDegisken, string)[] kosul, (BulanikDegisken, string)[] sonuc)
        {
            Kosullar = kosul;
            Sonuclar = sonuc;
        }
        public double Fire(double hass, double kir, double mik)
        {
            var values = Kosullar.Select(c =>
                c.deg.Ad switch
                {
                    "Hassaslik" => c.deg.UyelikAl(c.kume, hass),
                    "Kirlilik" => c.deg.UyelikAl(c.kume, kir),
                    "Miktar" => c.deg.UyelikAl(c.kume, mik),
                    _ => 0
                }).ToList();
            return values.Count > 0 ? values.Min() : 0;
        }
    }

    public class BulanikDenetleyici
    {
        private List<BulanikKural> kurallar;

        public BulanikDenetleyici()
        {
            // Değişken ve kümeler
            var hassas = new BulanikDegisken("Hassaslik");
            hassas.KumeEkle("Saglam", new TrapezoidalMF(-4, -1.5, 2, 4));
            hassas.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            hassas.KumeEkle("Hassas", new TrapezoidalMF(5.5, 8, 12.5, 14));

            var kirli = new BulanikDegisken("Kirlilik");
            kirli.KumeEkle("Kucuk", new TrapezoidalMF(-4.5, -2.5, 2, 4.5));
            kirli.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            kirli.KumeEkle("Buyuk", new TrapezoidalMF(5.5, 8, 12.5, 15));

            var miktar = new BulanikDegisken("Miktar");
            miktar.KumeEkle("Kucuk", new TrapezoidalMF(-4, -1.5, 2, 4));
            miktar.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            miktar.KumeEkle("Buyuk", new TrapezoidalMF(5.5, 8, 12.5, 14));

            var det = new BulanikDegisken("Deterjan");
            det.KumeEkle("CokAz", new TrapezoidalMF(0, 0, 20, 85));
            det.KumeEkle("Az", new TriangularMF(20, 85, 150));
            det.KumeEkle("Orta", new TriangularMF(85, 150, 215));
            det.KumeEkle("Fazla", new TriangularMF(150, 215, 280));
            det.KumeEkle("CokFazla", new TrapezoidalMF(215, 280, 300, 300));

            var su = new BulanikDegisken("Sure");
            su.KumeEkle("Kisa", new TrapezoidalMF(-46.5, -25.28, 22.3, 39.9));
            su.KumeEkle("NormalKisa", new TriangularMF(22.3, 39.9, 57.5));
            su.KumeEkle("Orta", new TriangularMF(39.9, 57.5, 75.1));
            su.KumeEkle("NormalUzun", new TriangularMF(57.5, 75.1, 92.7));
            su.KumeEkle("Uzun", new TrapezoidalMF(75, 92.7, 111.6, 130));

            var hiz = new BulanikDegisken("DonusHizi");
            hiz.KumeEkle("Hassas", new TrapezoidalMF(-5.8, -2.8, 0.5, 1.5));
            hiz.KumeEkle("NormalHassas", new TriangularMF(0.5, 2.75, 5));
            hiz.KumeEkle("Orta", new TriangularMF(2.75, 5, 7.25));
            hiz.KumeEkle("NormalGuclu", new TriangularMF(5, 7.25, 9.5));
            hiz.KumeEkle("Guclu", new TrapezoidalMF(8.5, 9.5, 12.8, 15.2));

            // Kural tablosunu rapora göre ekleyin (27 kural) 
            kurallar = new List<BulanikKural>
            {
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Kucuk"),(kirli,"Kucuk") }, new[]{ (hiz,"Hassas"),(su,"Kisa"),(det,"CokAz") }),
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Kucuk"),(kirli,"Orta") }, new[]{ (hiz,"NormalHassas"),(su,"Kisa"),(det,"Az") }),
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Kucuk"),(kirli,"Buyuk") }, new[]{ (hiz,"Orta"),(su,"NormalKisa"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Orta"),(kirli,"Kucuk") }, new[]{ (hiz,"Hassas"),(su,"Kisa"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Orta"),(kirli,"Orta") }, new[]{ (hiz,"NormalHassas"),(su,"NormalKisa"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Orta"),(kirli,"Buyuk") }, new[]{ (hiz,"Orta"),(su,"Orta"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Buyuk"),(kirli,"Kucuk") }, new[]{ (hiz,"NormalHassas"),(su,"NormalKisa"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Buyuk"),(kirli,"Orta") }, new[]{ (hiz,"NormalHassas"),(su,"Orta"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Hassas"),(miktar,"Buyuk"),(kirli,"Buyuk") }, new[]{ (hiz,"Orta"),(su,"NormalUzun"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Kucuk"),(kirli,"Kucuk") }, new[]{ (hiz,"NormalHassas"),(su,"NormalKisa"),(det,"Az") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Kucuk"),(kirli,"Orta") }, new[]{ (hiz,"Orta"),(su,"Kisa"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Kucuk"),(kirli,"Buyuk") }, new[]{ (hiz,"NormalGuclu"),(su,"Orta"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Orta"),(kirli,"Kucuk") }, new[]{ (hiz,"NormalHassas"),(su,"NormalKisa"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Orta"),(kirli,"Orta") }, new[]{ (hiz,"Orta"),(su,"Orta"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Orta"),(kirli,"Buyuk") }, new[]{ (hiz,"Hassas"),(su,"Uzun"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Buyuk"),(kirli,"Kucuk") }, new[]{ (hiz,"Hassas"),(su,"Orta"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Buyuk"),(kirli,"Orta") }, new[]{ (hiz,"Hassas"),(su,"NormalUzun"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Orta"),(miktar,"Buyuk"),(kirli,"Buyuk") }, new[]{ (hiz,"Hassas"),(su,"Uzun"),(det,"CokFazla") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Kucuk"),(kirli,"Kucuk") }, new[]{ (hiz,"Orta"),(su,"Orta"),(det,"Az") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Kucuk"),(kirli,"Orta") }, new[]{ (hiz,"NormalGuclu"),(su,"Orta"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Kucuk"),(kirli,"Buyuk") }, new[]{ (hiz,"Guclu"),(su,"NormalUzun"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Orta"),(kirli,"Kucuk") }, new[]{ (hiz,"Orta"),(su,"Orta"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Orta"),(kirli,"Orta") }, new[]{ (hiz,"NormalGuclu"),(su,"NormalUzun"),(det,"Orta") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Orta"),(kirli,"Buyuk") }, new[]{ (hiz,"Guclu"),(su,"Orta"),(det,"CokFazla") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Buyuk"),(kirli,"Kucuk") }, new[]{ (hiz,"NormalGuclu"),(su,"NormalUzun"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Buyuk"),(kirli,"Orta") }, new[]{ (hiz,"NormalGuclu"),(su,"Uzun"),(det,"Fazla") }),
                new BulanikKural(new[]{ (hassas,"Saglam"),(miktar,"Buyuk"),(kirli,"Buyuk") }, new[]{ (hiz,"Guclu"),(su,"Uzun"),(det,"CokFazla") }),
            };
        }

        private double Peak(MembershipFunction mf)
            => mf is TriangularMF t ? t.B : mf is TrapezoidalMF tp ? (tp.B + tp.C) / 2 : 0;

        public void Degerlendir(double hass, double kir, double mik,
            out double detSonuc, out double suSonuc, out double hizSonuc)
        {
            double sa = 0, sv = 0, ka = 0, kv = 0, ha = 0, hv = 0;
            foreach (var k in kurallar)
            {
                double alpha = k.Fire(hass, kir, mik);
                if (alpha <= 0) continue;
                foreach (var (deg, kume) in k.Sonuclar)
                {
                    double peak = Peak(deg.Kumeler[kume]);
                    switch (deg.Ad)
                    {
                        case "Deterjan": sa += alpha; sv += alpha * peak; break;
                        case "Sure": ka += alpha; kv += alpha * peak; break;
                        case "DonusHizi": ha += alpha; hv += alpha * peak; break;
                    }
                }
            }
            detSonuc = sa > 0 ? sv / sa : 0;
            suSonuc = ka > 0 ? kv / ka : 0;
            hizSonuc = ha > 0 ? hv / ha : 0;
       }

    }
}