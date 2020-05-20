using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssi_projekt
{
    class Program
    {
        static void Main(string[] args)
        {
            Dane dane = new Dane();
            double[][] dane_mecze = dane.Pobierz("bundesliga_dane.txt");
            double[][] srednie = dane.Pobierz("bundesliga_srednie.txt");
        }
    }
}
