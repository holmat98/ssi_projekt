using ssi_projekt.ActivationFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssi_projekt.NeuralNetwork
{
    class Neuron
    {
        // Listy z połączeniami wchodzącymi i wychodzącymi z neurona
        public List<Synapse> Inputs { get; set; } = new List<Synapse>();
        public List<Synapse> Outputs { get; set; } = new List<Synapse>();

        // Funkcja aktywacji dla danego neurona
        public IActivationFunction ActivationFunction { get; set; }

        // Wartość wejściowa i wyjściowa danego neurona
        public double InputValue { get; set; }
        public double OutputValue { get; set; }

        public Neuron(IActivationFunction activationFunction)
        {
            ActivationFunction = activationFunction;
        }

        // Obliczenie wartości wejściowej i wyjściowej
        public void CalculateOutputs(double sum)
        {
            InputValue = sum;
            OutputValue = ActivationFunction.Calculate(InputValue);
        }
    }
}
