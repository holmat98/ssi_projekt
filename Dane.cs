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
                data[i] = new double[tmp.Length-1];
                for (int j = 1; j < tmp.Length; j++)
                {
                    data[i][j-1] = Convert.ToDouble(tmp[j]); // Przekonwertowanie danych z pliku z string na double
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

            for (int i = 0; i < tablica[0].Length; i++)
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
