using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZachlannyPrzezWymianePodtrasTurniej
{
    class Program
    {

        static Random random = new Random();
        static List<double> punktX;
        static List<double> punktY;
        static int liczbaMiast;
        static int liczbaBaterii;
        static int minuty;
        static int liczbaPupulacji;
        static int pojemnoscBaterii = 1000;
        static double wspolczynnikMutacji;
        static bool[] miastaWymianaBaterii;
        static Stopwatch watch;
        static int nr_pok;
        static int stopienMutacji;
        static DateTime data1;
        static DateTime data2;
        static long[] pierwszyWynik;
        static long[] ostatniWynik;
        static string pierwszyCzas;
        static string ostatniCzas;
        static string ostatniaNrPokolenia;

        static void Main(string[] args)
        {
            string nazwaPanstwa = args[0];
            minuty = Int32.Parse(args[1]);
            liczbaPupulacji = Int32.Parse(args[2]);
            wspolczynnikMutacji = double.Parse(args[3]);
            stopienMutacji = Int32.Parse(args[4]);
            int ileRazy = Int32.Parse(args[5]);

            pierwszyWynik = new long[2];
            ostatniWynik = new long[2];

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
                string[] infoKoncowe = new string[15];
                string sciezka = "";
                Console.WriteLine("Zachlanny + Przez wymianę podtras + turniej");
                int[] roz = Ae();
                infoKoncowe[0] = "Zachlanny + Przez wymianę podtras + turniej";
                infoKoncowe[1] = "Miasto: " + args[0];
                infoKoncowe[2] = "Czas dzialania algorytmu w minutach: " + args[1];
                infoKoncowe[3] = "Liczebnosc populacji: " + args[2];
                infoKoncowe[4] = "Wspolczynnik mutacji: " + args[3];
                infoKoncowe[5] = "Stopien mutacji: " + args[4];
                infoKoncowe[6] = "Pierwszy wynik: " + "Czas: " + pierwszyWynik[0] + " Liczba baterii: " + pierwszyWynik[1];
                infoKoncowe[7] = "Pierwszy czas: " + pierwszyCzas;
                infoKoncowe[8] = "Ostatni wynik: " + "Czas: " + ostatniWynik[0] + " Liczba baterii: " + ostatniWynik[1];
                infoKoncowe[9] = "Ostatni czas: " + ostatniCzas + " Ostatni numer pokolenia: " + ostatniaNrPokolenia;
                infoKoncowe[10] = "Sciezka";
                for (int j = 0; j < roz.Length; j++)
                {
                    sciezka += roz[j] + " ";
                }
                infoKoncowe[11] = sciezka;
                infoKoncowe[12] = "Czas: " + WartoscfunkcjiDopasowania(roz)[0].ToString();
                infoKoncowe[13] = "Liczba baterii: " + liczbaBaterii.ToString();
                infoKoncowe[14] = "Liczba pokolen: " + nr_pok.ToString();
                DateTime data1 = DateTime.Now;
                string nazwaPliku = data1.ToString("dd.MM.yyyy HH.mm.ss") + ".txt";

                // tworzenie pliku 
                FileStream ZapisPlik = new FileStream("C:\\Users\\" + Environment.UserName + "\\Desktop\\" + nazwaPliku, FileMode.Create, FileAccess.Write);
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

        static int[] Ae()
        {
            int[] niebo;
            nr_pok = 0;

            //Tworzenie populacji z wygenerowanych algorytmem zachłannym osobników
            string folderPath = "C:\\Users\\" + Environment.UserName + "\\Desktop\\Zachlanny";
            int odKtorejLinniZaczynamyCzytac = 3;

            int[,] populacja = new int[liczbaPupulacji, liczbaMiast];
            int pomDoSledzeniaOsbnikaWPopulacji = 0;
            int licznikOsbnikow = 0;

            foreach (string file in Directory.EnumerateFiles(folderPath, "*.txt"))
            {
                int pom = 0;
                if (licznikOsbnikow == liczbaPupulacji)
                    break;
                foreach (string line in File.ReadLines(file, Encoding.UTF8))
                {
                    if (licznikOsbnikow > liczbaPupulacji)
                        break;
                    if (pom >= odKtorejLinniZaczynamyCzytac)
                    {
                        String[] substring = line.Split(' ');
                        for (int i = 0; i < substring.Length - 1; i++)
                            populacja[pomDoSledzeniaOsbnikaWPopulacji, i] = Int32.Parse(substring[i]);
                        pomDoSledzeniaOsbnikaWPopulacji++;
                        break;
                    }
                    else
                        pom++;
                }
                licznikOsbnikow++;
            }

            niebo = new int[populacja.GetLength(1)];

            watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds <= minuty * 60 * 1000)
            {
                //ustawiamy niebo 
                if (nr_pok == 0)
                {
                    for (int k = 0; k < populacja.GetLength(1); k++)
                        niebo[k] = populacja[0, k];
                    pierwszyWynik = WartoscfunkcjiDopasowania(niebo);
                    data1 = DateTime.Now;
                    pierwszyCzas = data1.ToString("dd.MM.yyyy HH.mm.ss");
                    Console.WriteLine(pierwszyCzas);
                    Console.WriteLine(pierwszyWynik[1]);
                }
                //nowa populacja
                int[,] nowaPopulacja = new int[liczbaPupulacji, liczbaMiast];
                for (int i = 0; i < populacja.GetLength(0); i++)
                {
                    //Selekcja
                    int[] mama = Selekcja(populacja);
                    int[] tata = Selekcja(populacja);

                    niebo = DoNieba(niebo, mama);
                    niebo = DoNieba(niebo, tata);

                    //Rekombinacja
                    int[] dziecko = Rekombinacja(mama, tata);

                    dziecko = Mutacja(dziecko);

                    for (int j = 0; j < dziecko.Length; j++)
                    {
                        nowaPopulacja[i, j] = dziecko[j];
                    }
                }
                populacja = nowaPopulacja;
                nr_pok++;
            }
            watch.Stop();

            return niebo;
        }

        private static int[] Mutacja(int[] dziecko)
        {
            if (random.NextDouble() > wspolczynnikMutacji)
                return dziecko;
            for (int i = 0; i < stopienMutacji; i++)
            {
                int los1 = random.Next(0, dziecko.Length);
                int los2 = random.Next(0, dziecko.Length);
                int pom = 0;

                pom = dziecko[los1];
                dziecko[los1] = dziecko[los2];
                dziecko[los2] = pom;
            }

            return dziecko;
        }

        private static int[] DoNieba(int[] niebo, int[] rodzic)
        {
            long[] wartoscNiebo = WartoscfunkcjiDopasowania(niebo);
            long[] wartoscRodzic = WartoscfunkcjiDopasowania(rodzic);
            if (wartoscRodzic[0] == wartoscNiebo[0])
            {
                if (wartoscRodzic[1] < wartoscNiebo[1])
                {
                    niebo = rodzic;
                }
            }
            else if (wartoscRodzic[0] < wartoscNiebo[0])
            {
                niebo = rodzic;
                ostatniWynik = (long[])wartoscRodzic.Clone();
                data2 = DateTime.Now;
                ostatniCzas = data2.ToString("dd.MM.yyyy HH.mm.ss");
                ostatniaNrPokolenia = nr_pok.ToString();
                Console.WriteLine(ostatniCzas);
                Console.WriteLine(ostatniWynik[1]);
                Console.WriteLine(ostatniaNrPokolenia);
            }
            return niebo;
        }

        private static int[] Selekcja(int[,] populacja)
        {
            //Turnej
            int indexPierwOsobnika = random.Next(0, populacja.GetLength(0));
            int indexDrogOsobnika = random.Next(0, populacja.GetLength(0));
            int[] osobnik1 = new int[populacja.GetLength(1)];
            int[] osobnik2 = new int[populacja.GetLength(1)];

            for (int i = 0; i < populacja.GetLength(1); i++)
            {
                osobnik1[i] = populacja[indexPierwOsobnika, i];
                osobnik2[i] = populacja[indexDrogOsobnika, i];
            }

            long[] wartoscOsobnik1 = WartoscfunkcjiDopasowania(osobnik1);
            long[] wartoscOsobnik2 = WartoscfunkcjiDopasowania(osobnik2);

            if (wartoscOsobnik1[0] == wartoscOsobnik2[0])
            {
                if (wartoscOsobnik1[1] < wartoscOsobnik2[1])
                    return osobnik1;
                else
                    return osobnik2;
            }
            else if (wartoscOsobnik1[0] < wartoscOsobnik2[0])
            {
                return osobnik1;
            }
            else
                return osobnik2;
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

        private static int[] Rekombinacja(int[] mama, int[] tata)
        {
            //Przez wymiane podtras
            bool[] czyZTatyCzyZMamy = new bool[mama.Length];
            int zliczPodtrasy = 0;
            int liczbaLosowan = 1;
            int[] osobnik = new int[mama.Length];
            //zamiana reprezentacji na na postać listy sąsiedztwa (genotyp)
            mama = Genotyp(mama);
            tata = Genotyp(tata);

            while (zliczPodtrasy < mama.Length)
            {
                //losujemy ile krawedzi przepisujemy z rodzica
                int ileKrawedzi = random.Next(1, mama.Length + 1);
                for (int i = zliczPodtrasy; i < ileKrawedzi + zliczPodtrasy; i++)
                {
                    if (i == mama.Length)
                        break;
                    if (IsOdd(liczbaLosowan))
                        czyZTatyCzyZMamy[i] = true;
                    else
                        continue;
                }
                zliczPodtrasy += ileKrawedzi;
                liczbaLosowan++;
            }

            int los = 0;
            bool[] ktoreWartosciByly = new bool[mama.Length];
            int punktStartowy = 0;
            int index = 0;
            int iloscNieWyko = osobnik.Length;
            //tworzymy nowego osbnika 
            for (int i = 0; i < osobnik.Length; i++)
            {
                if (czyZTatyCzyZMamy[i])
                {
                    //z mamy
                    if (i == 0)
                    {
                        los = random.Next(0, mama.Length);
                        osobnik[los] = mama[los];
                        ktoreWartosciByly[osobnik[los] - 1] = true;
                        punktStartowy = los;
                        los = osobnik[los] - 1;
                        iloscNieWyko--;
                        continue;
                    }
                    // z mamy
                    los = ZnajdzKrawedz(iloscNieWyko, los, ref index, ref ktoreWartosciByly, punktStartowy, mama, ref osobnik);
                    iloscNieWyko--;
                }
                else
                {
                    //z taty 
                    los = ZnajdzKrawedz(iloscNieWyko, los, ref index, ref ktoreWartosciByly, punktStartowy, tata, ref osobnik);
                    iloscNieWyko--;
                }
            }

            //zamiana z genotypu na fenotyp 
            osobnik = Fenotyp(osobnik);
            return osobnik;
        }

        private static int ZnajdzKrawedz(int iloscNieWyko, int los, ref int index, ref bool[] ktoreWartosciByly, int punktStartowy, int[] rodzic, ref int[] osobnik)
        {
            index = los;
            bool czyLosowano = false;
            if (ktoreWartosciByly[rodzic[los] - 1] || ((rodzic[los] - 1) == punktStartowy))
            {
                czyLosowano = true;
                los = random.Next(1, rodzic.Length + 1);
                los -= 1;
                if (ktoreWartosciByly[los] || (los == punktStartowy))
                {
                    bool znalazlkrawedz = false;
                    int i = los;
                    while (!znalazlkrawedz)
                    {
                        // jeżeli ostatna krawedź to zamykaj cykl 
                        if (iloscNieWyko == 1)
                        {
                            los = punktStartowy;
                            znalazlkrawedz = true;
                        }
                        // jeż nie jest punktem startwoym i wartoscia ktora byla
                        if (i != punktStartowy && ktoreWartosciByly[i] == false)
                        {
                            los = i;
                            znalazlkrawedz = true;
                        }
                        else
                            i = (i + 1) % (rodzic.Length);
                    }
                }
            }
            if (czyLosowano)
            {
                los += 1;
                osobnik[index] = los;
                ktoreWartosciByly[osobnik[index] - 1] = true;
                return osobnik[index] - 1;
            }
            else
            {
                osobnik[los] = rodzic[los];
                ktoreWartosciByly[osobnik[los] - 1] = true;
                return osobnik[los] - 1;
            }
        }

        private static int[] Fenotyp(int[] osobnik)
        {
            int[] pom = new int[osobnik.Length];
            int wartosc = 0;
            for (int i = 0; i < osobnik.Length; i++)
            {
                if (i == 0)
                {
                    pom[i] = 1;
                    wartosc = osobnik[i];
                }
                else
                {
                    pom[i] = wartosc;
                    wartosc = osobnik[wartosc - 1];
                }
            }
            return pom;
        }

        private static int[] Genotyp(int[] osbnik)
        {
            int[] genotypOsbnik = new int[osbnik.Length];
            for (int i = 0; i < genotypOsbnik.Length; i++)
            {
                if (i == genotypOsbnik.Length - 1)
                {
                    genotypOsbnik[(osbnik[i]) - 1] = osbnik[0];
                    break;
                }
                genotypOsbnik[(osbnik[i]) - 1] = osbnik[i + 1];
            }
            return genotypOsbnik;
        }

        // czy nieparzysta 
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}
