using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssi_projekt.ActivationFunction
{
    interface IActivationFunction
    {
        double Calculate(double value);
        double Derivative(double value);
    }
}
