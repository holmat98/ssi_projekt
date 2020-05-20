using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssi_projekt.NeuralNetwork
{
    class Layer
    {
        // Lista z neuronami dla danej powłoki
        public List<Neuron> Neurons { get; set; } = new List<Neuron>();
    }
}
