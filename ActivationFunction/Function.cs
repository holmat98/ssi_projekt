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
            if (value >= 0)
                return 1;
            else
                return 0;
               
        }

        public double Derivative(double value)
        {
            return 1;
        }
    }
}
