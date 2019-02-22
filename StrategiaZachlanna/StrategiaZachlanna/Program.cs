using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;

namespace StrategiaZachlanna
{
    class Program
    {
        static Random random = new Random();
        static List<double> punktX;
        static List<double> punktY;
        static int liczbaMiast;
        static int liczbaBaterii;
        static int pojemnoscBaterii = 1000;
        static bool[] miastaWymianaBaterii;

        static void Main(string[] args)
        {
            string nazwaPanstwa = args[0];
            int ileRazy = Int32.Parse(args[1]);

            //Dane
            punktX = new List<double>();
            punktY = new List<double>();

            foreach (string line in File.ReadLines(nazwaPanstwa, Encoding.UTF8))
            {
                String[] substring = line.Split(' ');
                punktX.Add(Double.Parse(substring[1]));
                punktY.Add(Double.Parse(substring[2]));
            }
            liczbaMiast = punktX.Count;

            //w ktorych mistach można ładować baterie
            miastaWymianaBaterii = new bool[liczbaMiast];
            for (int i = 0; i < miastaWymianaBaterii.Length; i++)
            {
                if (i % 5 == 0)
                    miastaWymianaBaterii[i] = true;
                else
                    continue;
            }

            for (int i = 0; i < ileRazy; i++)
            {
                string[] infoKoncowe = new string[6];
                string sciezka = "";
                Console.WriteLine("Zachlanny");
                int[] roz = Zachlanny();
                infoKoncowe[0] = "Zachlanny";
                infoKoncowe[1] = "Panstwo: " + args[0];
                infoKoncowe[2] = "Sciezka";
                for (int j = 0; j < roz.Length; j++)
                {
                    sciezka += roz[j] + " ";
                }
                infoKoncowe[3] = sciezka;
                infoKoncowe[4] = "Czas: " + WartoscfunkcjiDopasowania(roz)[0].ToString();
                infoKoncowe[5] = "Liczba baterii: " + liczbaBaterii.ToString();
                DateTime data1 = DateTime.Now;
                string nazwaPliku = data1.ToString("dd.MM.yyyy HH.mm.ss") + ".txt";

                // tworzenie pliku 
                FileStream ZapisPlik = new FileStream("C:\\Users\\" + Environment.UserName + "\\Desktop\\Zachlanny\\" + nazwaPliku, FileMode.Create, FileAccess.Write);
                StreamWriter nPlik = new StreamWriter(ZapisPlik);
                foreach (string item in infoKoncowe)
                {
                    nPlik.WriteLine(item);
                }
                nPlik.Close();
                ZapisPlik.Close();
            }
            Console.WriteLine("Koniec");

            Console.ReadKey();
        }

        static int[] Zachlanny()
        {
            double x1;
            double x2;
            double y1;
            double y2;

            double odlegloscOdMiast;
            double najktorszaOdlegloscOdMiast = 999999999;
            int nrMiasta;
            int[] sciezka = new int[liczbaMiast];
            bool[] czyMiastoJuzBylo = new bool[liczbaMiast];
            for (int i = 0; i < sciezka.Length; i++)
            {
                //los 1 miasto 
                if (i == 0)
                {
                    sciezka[i] = random.Next(0, sciezka.Length) + 1;
                    czyMiastoJuzBylo[sciezka[i] - 1] = true;
                    continue;
                }
                x1 = punktX[sciezka[i - 1] - 1];
                y1 = punktY[sciezka[i - 1] - 1];
                nrMiasta = -1;
                for (int j = 0; j < liczbaMiast; j++)
                {
                    //jezelinatrafisz na misato ktore juz bylo to pomin
                    if (czyMiastoJuzBylo[j])
                        continue;
                    else
                    {
                        x2 = punktX[j];
                        y2 = punktY[j];
                        //liczymy odleglosc od kazdego z miast i wybieramy najmniejsze 
                        odlegloscOdMiast = (double)Math.Round((Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2))), 2);
                        if (odlegloscOdMiast < najktorszaOdlegloscOdMiast)
                        {
                            najktorszaOdlegloscOdMiast = odlegloscOdMiast;
                            nrMiasta = j + 1;
                        }
                    }
                }
                sciezka[i] = nrMiasta;
                czyMiastoJuzBylo[nrMiasta - 1] = true;
                najktorszaOdlegloscOdMiast = 999999999;
            }

            return sciezka;
        }

        private static long[] WartoscfunkcjiDopasowania(int[] trasa)
        {
            long t = 0; // calkowity czas
            double v = 0.0; // clakowita predkosc 
            double s = 0.0; // calkowita droga
            double odlegloscOdMiast = 0.0;
            double najdluzszaPodTrasa = 0.0;
            double podTrasa = 0.0; //  długość trasy między ładowaniami 
            double x1;
            double x2;
            double y1;
            double y2;
            double pierwszaPodTrasa = 0.0;
            bool pierwszyPunktLaodwania = false;

            for (int i = 0; i < trasa.Length; i++)
            {
                // jezeli ostatnie msiato to zamknij cykl 
                //punktu są od 0 a tras jest od 1
                if (i == liczbaMiast - 1)
                {
                    x1 = punktX[trasa[i] - 1];
                    x2 = punktX[trasa[0] - 1];
                    y1 = punktY[trasa[i] - 1];
                    y2 = punktY[trasa[0] - 1];
                }
                else
                {
                    x1 = punktX[trasa[i] - 1];
                    x2 = punktX[trasa[i + 1] - 1];
                    y1 = punktY[trasa[i] - 1];
                    y2 = punktY[trasa[i + 1] - 1];
                }
                odlegloscOdMiast = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
                // sprawzamy czy 1 punkcie scieszki można ładować baterie jeżeli nie to sumuj 1 podtrase do 1 ładowania 
                // na koniec ta wartość będzie dodana do ostatniej podtrasy 
                if (!pierwszyPunktLaodwania && !miastaWymianaBaterii[trasa[0] - 1])
                    pierwszaPodTrasa += odlegloscOdMiast;

                s += odlegloscOdMiast;
                podTrasa += odlegloscOdMiast;

                if (i == liczbaMiast - 1)
                    podTrasa += pierwszaPodTrasa;
                if (podTrasa > najdluzszaPodTrasa)
                    najdluzszaPodTrasa = podTrasa;
                // jeżeli można ładować baterie w miescie to przestań sumować podtrasę 
                if (i != liczbaMiast - 1 && miastaWymianaBaterii[trasa[i + 1] - 1])
                {
                    podTrasa = 0.0;
                    pierwszyPunktLaodwania = true;
                }
            }
            liczbaBaterii = (int)Math.Ceiling((double)(najdluzszaPodTrasa / pojemnoscBaterii));
            if (liczbaBaterii >= 100)
                v = 0.01 / (liczbaBaterii - 95);
            else
                v = Math.Round(1 - 0.01 * liczbaBaterii, 2);
            t = (long)(s / v);
            long[] wynik = { t, liczbaBaterii };
            return wynik;
        }
    }
}