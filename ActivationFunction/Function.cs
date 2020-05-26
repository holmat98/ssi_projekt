using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssi_projekt.ActivationFunction
{
    class Function : IActivationFunction
    {
        public double Calculate(double value)
        {
            return (2 / (1 + Math.Exp(value))) - 1;
        }

        public double Derivative(double value)
        {
            return -((2 * Math.Exp(value)) / Math.Pow(Math.Exp(value) + 1, 2));
        }
    }
}
