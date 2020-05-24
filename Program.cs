using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ssi_projekt.NeuralNetwork;
using ssi_projekt.ActivationFunction;

namespace ssi_projekt
{
    class Program
    {
        static void Main(string[] args)
        {
            Dane dane = new Dane();
            double[][] dane_mecze = dane.Pobierz("bundesliga_dane.txt");
            Console.WriteLine("Wczytano dane");
            double[][] srednie = dane.Pobierz2("bundesliga_srednie.txt");

            dane_mecze = dane.Normalizuj(dane_mecze, srednie);
            dane_mecze = dane.Tasowanie(dane_mecze);

            Network network = new Network(6, 14, 2, 1, new SigmoidFunction());

            //network.Train(dane_mecze, 1000);

            string decyzja = "";
            while (decyzja != "3")
            {
                decyzja = "";
                Console.WriteLine("1) Trenuj sieć");
                Console.WriteLine("2) Uzyskaj rezultat");
                Console.WriteLine("3) Koniec programu");

                Console.WriteLine("Wybierz opcje:");
                decyzja = Console.ReadLine();

                if (decyzja == "1")
                {
                    network.Train(dane_mecze, 1000);
                    Console.ReadKey();
                }
                else if (decyzja == "2")
                {
                    network.GetOutput(srednie);
                    Console.ReadKey();
                }
                    
            }

            Console.WriteLine("Koniec");
            Console.ReadKey();
        }
    }
}
