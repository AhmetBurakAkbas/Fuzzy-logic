using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bulanikmantik
{
    public class MamdaniResult
    {
        public string Giris { get; set; }
        public string UyelikFonksiyonu { get; set; }
        public double MamdaniDegeri { get; set; }
    }

    public static class BulanikDenetleyiciExtensions
    {
        public static List<MamdaniResult> MamdaniCikarma(this BulanikDenetleyici denetleyici, double hassaslik, double miktar, double kirlilik)
        {
            var results = new List<MamdaniResult>();

            // Get membership values for Hassaslik
            var hassaslikDegisken = new BulanikDegisken("Hassaslik");
            hassaslikDegisken.KumeEkle("Saglam", new TrapezoidalMF(-4, -1.5, 2, 4));
            hassaslikDegisken.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            hassaslikDegisken.KumeEkle("Hassas", new TrapezoidalMF(5.5, 8, 12.5, 14));

            foreach (var kume in hassaslikDegisken.Kumeler)
            {
                results.Add(new MamdaniResult
                {
                    Giris = "Hassaslik",
                    UyelikFonksiyonu = kume.Key,
                    MamdaniDegeri = kume.Value.GetMembership(hassaslik)
                });
            }

            // Get membership values for Miktar
            var miktarDegisken = new BulanikDegisken("Miktar");
            miktarDegisken.KumeEkle("Kucuk", new TrapezoidalMF(-4, -1.5, 2, 4));
            miktarDegisken.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            miktarDegisken.KumeEkle("Buyuk", new TrapezoidalMF(5.5, 8, 12.5, 14));

            foreach (var kume in miktarDegisken.Kumeler)
            {
                results.Add(new MamdaniResult
                {
                    Giris = "Miktar",
                    UyelikFonksiyonu = kume.Key,
                    MamdaniDegeri = kume.Value.GetMembership(miktar)
                });
            }

            // Get membership values for Kirlilik
            var kirlilikDegisken = new BulanikDegisken("Kirlilik");
            kirlilikDegisken.KumeEkle("Kucuk", new TrapezoidalMF(-4.5, -2.5, 2, 4.5));
            kirlilikDegisken.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            kirlilikDegisken.KumeEkle("Buyuk", new TrapezoidalMF(5.5, 8, 12.5, 15));

            foreach (var kume in kirlilikDegisken.Kumeler)
            {
                results.Add(new MamdaniResult
                {
                    Giris = "Kirlilik",
                    UyelikFonksiyonu = kume.Key,
                    MamdaniDegeri = kume.Value.GetMembership(kirlilik)
                });
            }

            return results;
        }
    }
}
