using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

   
namespace bulanikmantik
{
    public partial class Form1 : Form
    {
        private BulanikDenetleyici denetleyici;

        public Form1()
        {
            InitializeComponent();
            denetleyici = new BulanikDenetleyici();
            InitializeCharts();
                SetupDataGridView();
        }
        private void SetupDataGridView()
        {
            // Eğer Designer'da sütunları eklemediyseniz, burada ekleyebilirsiniz
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("GirisAdi", "Giriş");
                dataGridView1.Columns.Add("UyelikAdi", "Üyelik");
                dataGridView1.Columns.Add("MamdaniDegeri", "Mamdani");
            }
        }

        private void UpdateMamdaniResults(double hass, double kir, double mik)
        {
            // Mamdani çıkarım sonuçlarını al
            List<MamdaniResult> results = denetleyici.MamdaniCikarma(hass, mik, kir);

            // DataGridView'i temizle 
            dataGridView1.Rows.Clear();

            // Sonuçları DataGridView'e ekle
            foreach (var result in results)
            {
                string girisAdi = "";
                switch (result.Giris)
                {
                    case "Hassaslik":
                        girisAdi = "Hassaslık";
                        break;
                    case "Miktar":
                        girisAdi = "Miktar";
                        break;
                    case "Kirlilik":
                        girisAdi = "Kirlilik";
                        break;
                }

                dataGridView1.Rows.Add(
                    girisAdi,
                    result.UyelikFonksiyonu,
                    result.MamdaniDegeri.ToString("F8")
                );
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox1.Text, out double hass) ||
                !double.TryParse(textBox2.Text, out double kir) ||
                !double.TryParse(textBox3.Text, out double mik))
            {
                MessageBox.Show("Lütfen 0-10 arası sayısal değer girin.");
                return;
            }

            denetleyici.Degerlendir(hass, kir, mik,
                out double det, out double su, out double hiz);

            // Sonuçları etiketlere yaz
            label8.Text = $"{det:F2} g";
            label9.Text = $"{su:F1} dk";
            label10.Text = $"{hiz:F1} rpm";

            // Grafikleri güncelle
            DrawMembershipFunctions(chart1, new BulanikDegisken("Hassaslik"), 0, 10, hass);
            DrawMembershipFunctions(chart2, new BulanikDegisken("Kirlilik"), 0, 10, kir);
            DrawMembershipFunctions(chart3, new BulanikDegisken("Miktar"), 0, 10, mik);
            DrawMembershipFunctions(chart4, new BulanikDegisken("Deterjan"), 0, 300, det);
            DrawMembershipFunctions(chart5, new BulanikDegisken("DonusHizi"), 0, 100, hiz);
            DrawMembershipFunctions(chart6, new BulanikDegisken("Sure"), 0, 100, su);

            // Mamdani çıkarım sonuçlarını al ve DataGridView'e doldur
            UpdateMamdaniResults(hass, kir, mik);
        }


        private void InitializeCharts()
        {
            // Hassaslık grafiği (chart1)
            var hassaslik = new BulanikDegisken("Hassaslik");
            hassaslik.KumeEkle("Saglam", new TrapezoidalMF(-4, -1.5, 2, 4));
            hassaslik.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            hassaslik.KumeEkle("Hassas", new TrapezoidalMF(5.5, 8, 12.5, 14));
            DrawMembershipFunctions(chart1, hassaslik, 0, 10, 0); // Giriş değeri başlangıçta 0

            // Kirlilik grafiği (chart2)
            var kirlilik = new BulanikDegisken("Kirlilik");
            kirlilik.KumeEkle("Kucuk", new TrapezoidalMF(-4.5, -2.5, 2, 4.5));
            kirlilik.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            kirlilik.KumeEkle("Buyuk", new TrapezoidalMF(5.5, 8, 12.5, 15));
            DrawMembershipFunctions(chart2, kirlilik, 0, 10, 0); // Giriş değeri başlangıçta 0

            // Miktar grafiği (chart3)
            var miktar = new BulanikDegisken("Miktar");
            miktar.KumeEkle("Kucuk", new TrapezoidalMF(-4, -1.5, 2, 4));
            miktar.KumeEkle("Orta", new TriangularMF(3, 5, 7));
            miktar.KumeEkle("Buyuk", new TrapezoidalMF(5.5, 8, 12.5, 14));
            DrawMembershipFunctions(chart3, miktar, 0, 10, 0); // Giriş değeri başlangıçta 0

            // Deterjan grafiği (chart4)
            var deterjan = new BulanikDegisken("Deterjan");
            deterjan.KumeEkle("CokAz", new TriangularMF(0, 20, 40));
            deterjan.KumeEkle("Az", new TriangularMF(30, 60, 90));
            deterjan.KumeEkle("Orta", new TriangularMF(80, 150, 220));
            deterjan.KumeEkle("Fazla", new TriangularMF(200, 250, 300));
            deterjan.KumeEkle("CokFazla", new TriangularMF(280, 300, 320));
            DrawMembershipFunctions(chart4, deterjan, 0, 300, 0);

            // Donus Hizi grafiği (chart5)
            var donusHizi = new BulanikDegisken("DonusHizi");
            donusHizi.KumeEkle("Kisa", new TriangularMF(0, 10, 20));
            donusHizi.KumeEkle("NormalKisa", new TriangularMF(15, 30, 45));
            donusHizi.KumeEkle("Orta", new TriangularMF(40, 50, 60));
            donusHizi.KumeEkle("NormalUzun", new TriangularMF(55, 70, 85));
            donusHizi.KumeEkle("Uzun", new TriangularMF(80, 90, 100));
            DrawMembershipFunctions(chart5, donusHizi, 0, 100, 0);

            // Sure grafiği (chart6)
            var sure = new BulanikDegisken("Sure");
            sure.KumeEkle("Kisa", new TriangularMF(0, 10, 20));
            sure.KumeEkle("NormalKisa", new TriangularMF(15, 30, 45));
            sure.KumeEkle("Orta", new TriangularMF(40, 50, 60));
            sure.KumeEkle("NormalUzun", new TriangularMF(55, 70, 85));
            sure.KumeEkle("Uzun", new TriangularMF(80, 90, 100));
            DrawMembershipFunctions(chart6, sure, 0, 100, 0);


        }
        public double CalculateCentroid(MembershipFunction mf, double minX, double maxX)
        {
            double numerator = 0;
            double denominator = 0;

            for (double x = minX; x <= maxX; x += (maxX - minX) / 1000.0)
            {
                double membership = mf.GetMembership(x);
                numerator += x * membership;
                denominator += membership;
            }

            return denominator == 0 ? 0 : numerator / denominator;
        }


        private void DrawMembershipFunctions(Chart chart, BulanikDegisken degisken, double minX, double maxX, double inputValue)
        {
            chart.ChartAreas.Clear();
            chart.Legends.Clear();

            // ChartArea oluştur
            ChartArea chartArea = new ChartArea();

            // X ekseni ayarları
            chartArea.AxisX.Minimum = minX;
            chartArea.AxisX.Maximum = maxX;
            chartArea.AxisX.LineWidth = 2;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chartArea.AxisX.Title = degisken.Ad;

            // Y ekseni ayarları
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = 1.1;
            chartArea.AxisY.LineWidth = 2;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.ArrowStyle = AxisArrowStyle.Triangle;

            // Sağ tarafta dikey başlık ekle
            if (degisken.Ad == "Hassaslik")
            {
                // Y ekseninin sağında dikey metin ekle
                StripLine stripLine = new StripLine();
                stripLine.Text = "HASSASLIK";
                stripLine.TextOrientation = TextOrientation.Rotated270;
                stripLine.TextAlignment = StringAlignment.Far;
                stripLine.IntervalOffset = maxX;
                stripLine.BackColor = System.Drawing.Color.Transparent;
                stripLine.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
                chartArea.AxisX.StripLines.Add(stripLine);
            }

            chart.ChartAreas.Add(chartArea);

            // Renk dizisi tanımla
            System.Drawing.Color[] colors = new System.Drawing.Color[] {
        System.Drawing.Color.Blue,
        System.Drawing.Color.Green,
        System.Drawing.Color.Purple
    };

            // Her üyelik fonksiyonu için bir seri ekle
            int colorIndex = 0;
            foreach (var kume in degisken.Kumeler)
            {
                Series series = new Series(kume.Key)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 3,
                    Color = colors[colorIndex % colors.Length]
                };

                // X ekseninde noktaları hesapla
                for (double x = minX; x <= maxX; x += (maxX - minX) / 100.0)
                {
                    double y = kume.Value.GetMembership(x);
                    series.Points.AddXY(x, y);
                }

                chart.Series.Add(series);

                // Üyelik fonksiyonu adını üstte renkli olarak göster
                double peakX = FindPeakX(kume.Value, minX, maxX);

                // Etiket ekle - üst orta noktada
                CustomLabel label = new CustomLabel();
                label.FromPosition = peakX - 0.5;
                label.ToPosition = peakX + 0.5;
                label.Text = kume.Key.ToUpper();
                label.ForeColor = colors[colorIndex % colors.Length];

                chartArea.AxisX.CustomLabels.Add(label);

                colorIndex++;
            }

            // Giriş değerini göstermek için dikey yeşil ok ekle
            Series inputSeries = chart.Series.FindByName("Giriş Değeri");
            if (inputSeries == null)
            {
                inputSeries = new Series("Giriş Değeri")
                {
                    ChartType = SeriesChartType.Line,
                    Color = System.Drawing.Color.Green,
                    BorderWidth = 2,
                    BorderDashStyle = ChartDashStyle.Solid
                };

                inputSeries.Points.AddXY(inputValue, 0);
                inputSeries.Points.AddXY(inputValue, 1);
                chart.Series.Add(inputSeries);
            }
            else
            {
                // Eğer seri zaten varsa, sadece noktaları güncelle
                inputSeries.Points.Clear();
                inputSeries.Points.AddXY(inputValue, 0);
                inputSeries.Points.AddXY(inputValue, 1);
            }

            // Yeşil ok başı ekle
            DataPoint arrowPoint = new DataPoint(inputValue, 1)
            {
                MarkerStyle = MarkerStyle.Triangle,
                MarkerSize = 15,
                MarkerColor = System.Drawing.Color.Green
            };
            inputSeries.Points.Add(arrowPoint);

            // Arka plan rengini beyaz yap
            chart.BackColor = System.Drawing.Color.White;
            chartArea.BackColor = System.Drawing.Color.White;

            // Legend'ı gizle
            chart.Legends.Clear();
        }


        // Bir üyelik fonksiyonunun tepe noktasını bulan yardımcı metod
        private double FindPeakX(MembershipFunction mf, double minX, double maxX)
        {
            double peakX = minX;
            double peakY = 0;

            for (double x = minX; x <= maxX; x += (maxX - minX) / 100.0)
            {
                double y = mf.GetMembership(x);
                if (y > peakY)
                {
                    peakY = y;
                    peakX = x;
                }
            }

            return peakX;
        }
        public class LeftLineMF : MembershipFunction
        {
            public double A, B; // A: where value = 1.0, B: where value = 0.0

            public LeftLineMF(double a, double b) { A = a; B = b; }

            public override double GetMembership(double x)
            {
                if (x <= A) return 1.0;
                if (x >= B) return 0.0;
                return 1.0 - ((x - A) / (B - A));
            }
        }

        // Right-side straight line membership function (increases from left to right)
        public class RightLineMF : MembershipFunction
        {
            public double A, B; // A: where value = 0.0, B: where value = 1.0

            public RightLineMF(double a, double b) { A = a; B = b; }

            public override double GetMembership(double x)
            {
                if (x <= A) return 0.0;
                if (x >= B) return 1.0;
                return (x - A) / (B - A);
            }
        }



    }

}