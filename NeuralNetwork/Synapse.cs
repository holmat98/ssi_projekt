using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssi_projekt.NeuralNetwork
{
    class Synapse
    {
        // Neurony między którymi połączona jest ta synapsa
        private Neuron fromNeuron;
        private Neuron toNeuron;

        public double Weight { get; set; }

        public Synapse(double weight, Neuron fromNeuron, Neuron toNeuron)
        {
            Weight = weight;
            this.fromNeuron = fromNeuron;
            this.toNeuron = toNeuron;
        }

        // aktualizowanie wagi dla danej synapsy
        public void UpdateWeight(double delta, double learningRate)
        {
            Weight += delta * learningRate;
        }
    }
}
