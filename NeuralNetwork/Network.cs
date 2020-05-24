using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using ssi_projekt.ActivationFunction;

namespace ssi_projekt.NeuralNetwork
{
    class Network
    {
        private List<Layer> layers = new List<Layer>();
        private double learningRate;

        #region Creating Neural Network
        public Network(double layersNumber, double inputNeuronsNumber, double outputNeuronsNumber, double learningRate, IActivationFunction activationFunction)
        {
            this.learningRate = learningRate;
            //tworzenie powłók oraz neuronów
            for(int i=0; i<layersNumber-1; i++) 
            {
                layers.Add(new Layer());
                for(int j=0; j<inputNeuronsNumber; j++)
                {
                    layers[i].Neurons.Add(new Neuron(activationFunction)); // stworzenie neurona z podaną funkcją aktywacji
                }
            }
            // tworzenie powłoki wyjściowej z ilością neuronów na wyjściu
            layers.Add(new Layer());
            for(int i=0; i<outputNeuronsNumber; i++)
            {
                layers.Last().Neurons.Add(new Neuron(activationFunction));
            }

            CreateSynapses(); // stworzenie połączeń między neuronami
        }

        private void CreateSynapses()
        {
            // Jeśli plik z danymi nie istnieje to wagi dla danego połączenia są losowane z przedziału [0;1]
            if(File.Exists("../../WeightsFile/weigths.txt") == false)
            {
                var rnd = new Random();
                // zaczynamy od pierwszej ukrytej powłoki
                for (int i = 1; i < layers.Count; i++)
                {
                    for (int j = 0; j < layers[i].Neurons.Count; j++) // nerony w danej powłoce
                    {
                        for (int k = 0; k < layers[i - 1].Neurons.Count; k++) // neurony w powłoce poprzedniej
                        {
                            // stworzenie połączenia między neuronami, pierwszy argument to waga, drugi neuron poprzedniej powłoki, trzeci to aktualny neuron
                            Synapse newSynapse = new Synapse(Math.Round(rnd.NextDouble(), 2), layers[i - 1].Neurons[k], layers[i].Neurons[j]);
                            // dodanie połączenia do listy z połączeniami danego neuronu
                            layers[i - 1].Neurons[k].Outputs.Add(newSynapse);
                            layers[i].Neurons[j].Inputs.Add(newSynapse);
                        }
                    }
                }
            }
            else
            {
                // wczytanie wag do tablicy z pliku
                double[] weights = readFromFile();
                // index wagi
                int weightIndex = 0;
                // połączenia są tworzone tak samo tylko, że tutaj zaczynamy od neurona 1 powłoki 1 oraz wagi są wczytwane z pliku
                for (int i = 0; i < layers.Count-1; i++)
                {
                    for (int j = 0; j < layers[i].Neurons.Count; j++)
                    {
                        for (int k = 0; k < layers[i + 1].Neurons.Count; k++)
                        {
                            Synapse newSynapse = new Synapse(weights[weightIndex], layers[i].Neurons[j], layers[i+1].Neurons[k]);
                            layers[i].Neurons[j].Outputs.Add(newSynapse);
                            layers[i+1].Neurons[k].Inputs.Add(newSynapse);
                            weightIndex++;
                        }
                    }
                }
            }
            
        }

        #endregion

        #region Neural network training and getting output:
        public void Train(double[][] inputData, int iterations)
        {
            // iterations -> ilość powtórzeń dla wszystkich danych z inputData
            int iteration = 0;
            // tablica z błędami
            double[][] errors = new double[layers.Count][];

            for (int i = 0; i < layers.Count; i++)
            {
                errors[i] = new double[layers[i].Neurons.Count];
            }

            while (iteration < iterations)
            {
                int accuracySum = 0;
                // poruszanie po wierszach z przekazanego zestawu danych
                for(int i=0; i < inputData.Length; i++)
                {
                    // input -> dane, które będą wykorzystane do obliczenia wartości wyjściowej
                    // expected -> wartość poprawna dla danego przypadku
                    double[] input = new double[inputData[i].Length-4];
                    double[] expected = new double[2] { inputData[i][2], inputData[i][3]};
                    for (int j = 0; j < input.Length; j++)
                        input[j] = inputData[i][j + 4];
                    // wynik uzyskany dla danego przypadku
                    double[] output = Calculate(input);

                    // obliczenie błędu
                    CalculateError(output, expected, errors);
                    // zaktualizowanie wag w połączeniach między neuronami
                    UpdateWeights(errors);

                    //Console.WriteLine("------------------------------------------------------------------------------");
                    //Console.WriteLine($"Wynik: ({output[0]}||{output[1]}) Prawdziwy: ({expected[0]}||{expected[1]})");

                    int currentSum = 0;
                    for (int j = 0; j < output.Length; j++)
                    {
                        if (output[j] >= 0.55)
                            output[j] = 1;
                        else
                            output[j] = 0;

                        if (output[j] == expected[j])
                            currentSum++;
                    }
                    if (currentSum == output.Length)
                        accuracySum++;

                    //Console.WriteLine($"Wynik: ({output[0]}||{output[1]}) Prawdziwy: ({expected[0]}||{expected[1]})");
                    //Console.WriteLine("------------------------------------------------------------------------------");

                    /*if(iteration == 99)
                    {
                        Console.WriteLine("--------------------------------------------------------------------------------");
                        Console.WriteLine($"Wynik: ({output[0]}||{output[1]}) Prawdziwy: ({expected[0]}||{expected[1]})");
                    }*/



                }
                iteration++;
                
                Console.WriteLine($"{iteration}) Zgodność: {(accuracySum / (double)inputData.Length)*100}%");
                //Console.WriteLine("--------------------------------------------------------------------------------");
            }
            // Zapis uzyskanych wag do pliku
            if(File.Exists("../../WeightsFile/weigths.txt") == false)
            {
                writeToFile();
            }
            else
            {
                File.Delete("../../WeightsFile/weigths.txt");
                writeToFile();
            }
        }

        public void GetOutput(double[][] inputData)
        {
            string team1;
            Console.WriteLine("Podaj drużynę gospodarzy:");
            team1 = Console.ReadLine();
            string team2;
            Console.WriteLine("Podaj drużynę gości:");
            team2 = Console.ReadLine();

            int team1Number = TeamNumber(team1);
            int team2Number = TeamNumber(team2);

            double[] input = new double[14];

            for(int i=0; i<inputData.Length; i++)
            {
                if(inputData[i][0] == team1Number)
                {
                    for(int j=1; j<inputData[i].Length; j++)
                    {
                        input[(j - 1) * 2] = inputData[i][j];
                    }
                }

                if(inputData[i][0] == team2Number)
                {
                    for (int j = 1; j < inputData[i].Length; j++)
                    {
                        input[(j * 2) - 1] = inputData[i][j];
                    }
                }
            }

            double[] output = Calculate(input);

            for(int i=0; i<output.Length; i++)
            {
                if (output[i] >= 0.85)
                    output[i] = 1;
                else
                    output[i] = 0;
            }

            if (output[0] == 1 && output[1] == 0)
                Console.WriteLine($"Wygra drużyna {team1}");
            else if (output[0] == 0 && output[1] == 1)
                Console.WriteLine($"Wygra drużyna {team2}");
            else if (output[0] == 1 && output[1] == 1)
                Console.WriteLine($"W tym meczu będzie remis");
            else
                Console.WriteLine("Nie wiem kto wygra");
        }

        #endregion

        #region Calculating output values
        private void AddInputValues(double[] input)
        {
            // dodanie wartości wejściowych dla neuronów pierwszej powłoki
            for(int i=0; i<input.Length; i++)
            {
                layers[0].Neurons[i].InputValue = layers[0].Neurons[i].OutputValue = input[i];
            }

        }

        private double[] Calculate(double[] input)
        {
            AddInputValues(input);

            // obliczanie sum wartości wejściowej dla danego neurona. Następnie wywoływana jest metoda CalculateOutputs,
            // która oblicza wartość funkcji aktywacyjnej dla danej sumy
            for (int i = 1; i < layers.Count; i++)
            {
                for (int j = 0; j < layers[i].Neurons.Count; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < layers[i].Neurons[j].Inputs.Count; k++)
                    {
                        sum += layers[i].Neurons[j].Inputs[k].Weight * layers[i - 1].Neurons[k].OutputValue;
                    }
                    layers[i].Neurons[j].CalculateOutputs(sum);
                }
            }

            // stworzenie tablicy z obliczonym wynikiem
            double[] output = new double[layers.Last().Neurons.Count];
            for(int i=0; i<output.Length; i++)
            {
                output[i] = layers.Last().Neurons[i].OutputValue;
            }

            return output;
        }

        #endregion

        #region Calculating Error and Updating Weights
        private void CalculateError(double[] output, double[] expected, double[][] errors)
        {
            // obliczanie błędu dla neuronów z ostatniej powłoki
            for (int i = 0; i < output.Length; i++)
            {
                // obliczenie wartości błędu dla otrzymanego błędu
                Neuron currentNeuron = layers.Last().Neurons[i];
                errors.Last()[i] = currentNeuron.ActivationFunction.Derivative(currentNeuron.InputValue) * (output[i] - expected[i]);
            }

            //Obliczanie błędu dla neuronów z powłok 1 do n-1
            for (int i = layers.Count - 2; i > 0; i--)
            {
                for (int j = 0; j < layers[i].Neurons.Count; j++)
                {
                    errors[i][j] = 0;
                    
                    for (int k = 0; k < layers[i + 1].Neurons.Count; k++)
                    {
                        errors[i][j] += errors[i + 1][k] * layers[i + 1].Neurons[k].Inputs[j].Weight;
                    }

                    Neuron currentNeuron = layers[i].Neurons[j];
                    errors[i][j] *= currentNeuron.ActivationFunction.Derivative(currentNeuron.InputValue);
                }
            }
        }

        private void UpdateWeights(double[][] errors)
        {
            // zaktualizowanie wag
            // nowa waga = stara waga + delta*learningRate
            for(int i=layers.Count-1; i>0; i--)
            {
                for(int j=0; j<layers[i].Neurons.Count; j++)
                {
                    for(int k=0; k<layers[i-1].Neurons.Count; k++)
                    {
                        double delta = errors[i][j] * layers[i - 1].Neurons[k].OutputValue * (-1);
                        layers[i].Neurons[j].Inputs[k].UpdateWeight(delta, learningRate);
                    }
                }
            }
        }

        #endregion

        #region Weigths writing and reading

        private void writeToFile()
        {
            // zapisanie otrzymanych wag po trenowaniu sieci neuronowej do pliku
            StreamWriter streamWriter;
            streamWriter = File.AppendText("../../WeightsFile/weigths.txt");
            for(int i=0; i<layers.Count-1; i++)
            {
                for(int j=0; j<layers[i].Neurons.Count; j++)
                {
                    for(int k=0; k<layers[i].Neurons[j].Outputs.Count; k++)
                    {
                        streamWriter.WriteLine(layers[i].Neurons[j].Outputs[k].Weight);
                    }
                }
            }
            streamWriter.Close();
        }

        private double[] readFromFile()
        {
            // wczytanie do tablicy wszystkich wag zapisanych w pliku
            string[] lines = File.ReadAllLines("../../WeightsFile/weigths.txt");
            double[] weights = new double[lines.Length];
            for(int i=0; i<lines.Length; i++)
            {
                weights[i] = Convert.ToDouble(lines[i]);
            }

            return weights;
        }

        #endregion

        private int TeamNumber(string teamName)
        {
            int number = 0;

            if (teamName == "Augsburg")
                number = 0;
            else if (teamName == "Bayern Munich")
                number = 1;
            else if (teamName == "Darmstadt")
                number = 2;
            else if (teamName == "Dortmund")
                number = 3;
            else if (teamName == "Ein Frankfurt")
                number = 4;
            else if (teamName == "FC Koln")
                number = 5;
            else if (teamName == "Fortuna Dusseldorf")
                number = 6;
            else if (teamName == "Freiburg")
                number = 7;
            else if (teamName == "Hamburg")
                number = 8;
            else if (teamName == "Hannover")
                number = 9;
            else if (teamName == "Hertha")
                number = 10;
            else if (teamName == "Hoffenheim")
                number = 11;
            else if (teamName == "Ingolstadt")
                number = 12;
            else if (teamName == "Leverkusen")
                number = 13;
            else if (teamName == "Mainz")
                number = 14;
            else if (teamName == "Mgladbach")
                number = 15;
            else if (teamName == "Nurnberg")
                number = 16;
            else if (teamName == "Paderborn")
                number = 17;
            else if (teamName == "RB Leipzig")
                number = 18;
            else if (teamName == "Schalke 04")
                number = 19;
            else if (teamName == "Stuttgart")
                number = 20;
            else if (teamName == "Union Berlin")
                number = 21;
            else if (teamName == "Werder Bremen")
                number = 22;
            else if (teamName == "Wolfsburg")
                number = 23;

            return number;
        }

    }
}
