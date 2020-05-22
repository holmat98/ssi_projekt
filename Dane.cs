using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ssi_projekt
{
    class Dane
    {
        public double[][] Pobierz(string nazwa_pliku)
        {
            string[] lines = File.ReadAllLines(nazwa_pliku); // wczytanie wszystkich danych z pliku
            double[][] data = new double[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split('\t'); // podzielenie danych w danej linii pliku
                data[i] = new double[tmp.Length];
                for (int j = 0; j < tmp.Length; j++)
                {
                    if (tmp[j] == "Augsburg")
                        data[i][j] = 0;
                    else if (tmp[j] == "Bayern Munich")
                        data[i][j] = 1;
                    else if (tmp[j] == "Dortmund")
                        data[i][j] = 2;
                    else if (tmp[j] == "Ein Frankfurt")
                        data[i][j] = 3;
                    else if (tmp[j] == "FC Koln")
                        data[i][j] = 4;
                    else if (tmp[j] == "Fortuna Dusseldorf")
                        data[i][j] = 5;
                    else if (tmp[j] == "Freiburg")
                        data[i][j] = 6;
                    else if (tmp[j] == "Hertha")
                        data[i][j] = 7;
                    else if (tmp[j] == "Hoffenheim")
                        data[i][j] = 8;
                    else if (tmp[j] == "Leverkusen")
                        data[i][j] = 9;
                    else if (tmp[j] == "Mainz")
                        data[i][j] = 10;
                    else if (tmp[j] == "Mgladbach")
                        data[i][j] = 11;
                    else if (tmp[j] == "Paderborn")
                        data[i][j] = 12;
                    else if (tmp[j] == "RB Leipzig")
                        data[i][j] = 13;
                    else if (tmp[j] == "Schalke 04")
                        data[i][j] = 14;
                    else if (tmp[j] == "Union Berlin")
                        data[i][j] = 15;
                    else if (tmp[j] == "Werder Bremen")
                        data[i][j] = 16;
                    else if (tmp[j] == "Wolfsburg")
                        data[i][j] = 17;
                    else
                        data[i][j] = Convert.ToDouble(tmp[j]); // Przekonwertowanie danych z pliku z string na double
                }
            }

            return data;
        }

        public double[][] Tasowanie(double[][] tablica)
        {
            double[][] tab2 = new double[tablica.Length][];
            for (int i = 0; i < tablica.Length; i++)
            {
                tab2[i] = new double[tablica[0].Length];
            }

            int n = tab2.Length;
            Random x = new Random();

            List<int> wylosowane = new List<int>(); // Lista z indeksami, które już zostały pozamieniane

            for (int i = 0; i < n; i++)
            {
                int wylosowana = x.Next(0, n); // Losowanie indeksu, który zostanie zamieniony
                while (wylosowane.Contains(wylosowana))
                {
                    wylosowana = x.Next(0, n);
                }

                wylosowane.Add(wylosowana);

                tab2[i] = tablica[wylosowana]; // podmienienie danych
            }

            return tab2;
        }

        public double[][] Normalizuj(double[][] tablica, params double[][] przyklad)
        {
            double nmin = 0;
            double nmax = 1;

            for (int i = 3; i < tablica[0].Length; i++)
            {
                // wartosc min i max danej kolumny
                double max = tablica[0][i];
                double min = tablica[0][i];
                for (int j = 0; j < tablica.Length; j++)
                {
                    if (max < tablica[j][i])
                        max = tablica[j][i];
                    if (min > tablica[j][i])
                        min = tablica[j][i];
                }

                // normalizowanie danych w danej kolumnie
                for (int j = 0; j < tablica.Length; j++)
                {
                    tablica[j][i] = ((tablica[j][i] - min) / (max - min)) * (nmax - nmin) + nmin;
                }

                // normalizacja dla przykladow
                for (int j = 0; j < przyklad.Length; j++)
                    przyklad[j][i] = ((przyklad[j][i] - min) / (max - min)) * (nmax - nmin) + nmin;
            }

            return tablica;
        }
    }
}
