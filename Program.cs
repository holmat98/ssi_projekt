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

            dane_mecze = dane.Normalizuj(dane_mecze);
            dane_mecze = dane.Tasowanie(dane_mecze);

            Network network = new Network(10, 16, 2, 1, new SigmoidFunction());
            network.Train(dane_mecze, 1000);

            Console.WriteLine("Wczytano");
            Console.ReadKey();
        }
    }
}
