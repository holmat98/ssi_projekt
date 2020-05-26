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

            Network network = new Network(4, 14, 2, 0.5, new SigmoidFunction(), "weights.txt");
            Network network1 = new Network(4, 14, 2, 0.1, new Function(), "weights2.txt");

            //network.Train(dane_mecze, 1000);

            string decyzja = "";
            while (decyzja != "5")
            {
                decyzja = "";
                Console.WriteLine("Wsteczna propagacja błędu:");
                Console.WriteLine("1) Trenuj sieć");
                Console.WriteLine("2) Uzyskaj rezultat");
                Console.WriteLine("Reguła Heba:");
                Console.WriteLine("3) Trenuj sieć");
                Console.WriteLine("4) Uzyskaj rezultat");
                Console.WriteLine("5) Koniec programu");

                Console.WriteLine("Wybierz opcje:");
                decyzja = Console.ReadLine();

                if (decyzja == "1")
                {
                    dane_mecze = dane.Tasowanie(dane_mecze);
                    network.Train(dane_mecze, 5000);
                    Console.ReadKey();
                }
                else if (decyzja == "2")
                {
                    network.GetOutput(srednie);
                    Console.ReadKey();
                }
                else if(decyzja == "3")
                {
                    dane_mecze = dane.Tasowanie(dane_mecze);
                    network1.TrainHR(dane_mecze, 5000);
                    Console.ReadKey();
                }
                    
            }

            Console.WriteLine("Koniec");
            Console.ReadKey();
        }
    }
}
