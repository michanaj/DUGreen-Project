using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    /// <summary>
    /// Updated by Junya Michanan, U. of Denver, Colorado
    /// The code was originally developed by Dr. James D. McCaffrey (https://jamesmccaffrey.wordpress.com/about/)
    /// </summary>
    public class NeuralNetwork
    {
        #region Private Properties
        private static Random rnd;

        private int numInput;
        private int numHidden;
        private int numOutput;

        private double[] inputs;

        private double[][] ihWeights; // input-hidden 
        private double[] hBiases;
        private double[] hOutputs;

        private double[][] hoWeights; // hidden-output 
        private double[] oBiases;

        private double[] outputs;

        // Back-propagation specific arrays. 
        private double[] oGrads; // Output gradients. 
        private double[] hGrads;

        // Back-propagation momentum-specific arrays. 
        private double[][] ihPrevWeightsDelta;
        private double[] hPrevBiasesDelta;
        private double[][] hoPrevWeightsDelta;
        private double[] oPrevBiasesDelta;
        #endregion

        #region Constructor
        public NeuralNetwork(int numInput, int numHidden, int numOutput)
        {
            rnd = new Random(0); // For InitializeWeights() and Shuffle(). 

            this.numInput = numInput;
            this.numHidden = numHidden;
            this.numOutput = numOutput;

            this.inputs = new double[numInput];

            this.ihWeights = MakeMatrix(numInput, numHidden);
            this.hBiases = new double[numHidden];
            this.hOutputs = new double[numHidden];

            this.hoWeights = MakeMatrix(numHidden, numOutput);
            this.oBiases = new double[numOutput];

            this.outputs = new double[numOutput];

            this.InitializeWeights();

            // Back-propagation related arrays below. 
            this.hGrads = new double[numHidden];
            this.oGrads = new double[numOutput];

            this.ihPrevWeightsDelta = MakeMatrix(numInput, numHidden);
            this.hPrevBiasesDelta = new double[numHidden];
            this.hoPrevWeightsDelta = MakeMatrix(numHidden, numOutput);
            this.oPrevBiasesDelta = new double[numOutput];
        } // ctor 
        #endregion

        #region Methods/Helpers
        
        private static double[][] MakeMatrix(int rows, int cols) // Helper for ctor. 
        {
            double[][] result = new double[rows][];
            for (int r = 0; r < result.Length; ++r)
                result[r] = new double[cols];
            return result;
        }
        public void SetWeights(double[] weights)
        {
            // Copy weights and biases in weights[] array to i-h weights, i-h biases, 
            // h-o weights, h-o biases. 
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) +
            numHidden + numOutput;
            if (weights.Length != numWeights)
                throw new Exception("Bad weights array length: ");

            int k = 0; // Points into weights param. 

            for (int i = 0; i < numInput; ++i)
                for (int j = 0; j < numHidden; ++j)
                    ihWeights[i][j] = weights[k++];
            for (int i = 0; i < numHidden; ++i)
                hBiases[i] = weights[k++];
            for (int i = 0; i < numHidden; ++i)
                for (int j = 0; j < numOutput; ++j)
                    hoWeights[i][j] = weights[k++];
            for (int i = 0; i < numOutput; ++i)
                oBiases[i] = weights[k++];
        }
        private void InitializeWeights()
        {
            // Initialize weights and biases to small random values. 
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) +
            numHidden + numOutput;
            double[] initialWeights = new double[numWeights];
            double lo = -0.01;
            double hi = 0.01;
            for (int i = 0; i < initialWeights.Length; ++i)
                initialWeights[i] = (hi - lo) * rnd.NextDouble() + lo;
            this.SetWeights(initialWeights);
        }

        public double[] GetWeights()
        {
            // Returns the current set of weights, presumably after training. 
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            double[] result = new double[numWeights];
            int k = 0;
            for (int i = 0; i < ihWeights.Length; ++i)
                for (int j = 0; j < ihWeights[0].Length; ++j)
                    result[k++] = ihWeights[i][j];
            for (int i = 0; i < hBiases.Length; ++i)
                result[k++] = hBiases[i];
            for (int i = 0; i < hoWeights.Length; ++i)
                for (int j = 0; j < hoWeights[0].Length; ++j)
                    result[k++] = hoWeights[i][j];
            for (int i = 0; i < oBiases.Length; ++i)
                result[k++] = oBiases[i];
            return result;
        }

        public double[] ComputeOutputs(double[] xValues)
        {
            if (xValues.Length != numInput)
                throw new Exception("Bad xValues array length");

            double[] hSums = new double[numHidden]; // Hidden nodes sums scratch array. 
            double[] oSums = new double[numOutput]; // Output nodes sums. 

            for (int i = 0; i < xValues.Length; ++i) // Copy x-values to inputs. 
                this.inputs[i] = xValues[i];

            for (int j = 0; j < numHidden; ++j) // Compute i-h sum of weights * inputs. 
                for (int i = 0; i < numInput; ++i)
                    hSums[j] += this.inputs[i] * this.ihWeights[i][j]; // note += 

            for (int i = 0; i < numHidden; ++i) // Add biases to input-to-hidden sums. 
                hSums[i] += this.hBiases[i];

            for (int i = 0; i < numHidden; ++i) // Apply activation. 
                this.hOutputs[i] = HyperTan(hSums[i]); // Hard-coded. 

            for (int j = 0; j < numOutput; ++j) // Compute h-o sum of weights * hOutputs. 
                for (int i = 0; i < numHidden; ++i)
                    oSums[j] += hOutputs[i] * hoWeights[i][j];

            for (int i = 0; i < numOutput; ++i) // Add biases to input-to-hidden sums. 
                oSums[i] += oBiases[i];

            double[] softOut = Softmax(oSums); // All outputs at once for efficiency. 
            Array.Copy(softOut, outputs, softOut.Length);

            double[] retResult = new double[numOutput];
            Array.Copy(this.outputs, retResult, retResult.Length);
            return retResult;
        } // ComputeOutputs 

        private static double HyperTan(double x)
        {
            if (x < -20.0)
                return -1.0; // Approximation is correct to 30 decimals. 
            else if (x > 20.0)
                return 1.0;
            else return
           Math.Tanh(x);
        }

        private static double[] Softmax(double[] oSums)
        {
            // Does all output nodes at once so scale doesn't have to be re-computed each time. 
            double max = oSums[0]; // Determine max output sum. 
            for (int i = 0; i < oSums.Length; ++i)
                if (oSums[i] > max) max = oSums[i];

            // Determine scaling factor -- sum of exp(each val - max). 
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
                scale += Math.Exp(oSums[i] - max);

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
                result[i] = Math.Exp(oSums[i] - max) / scale;

            return result; // Now scaled so that xi sum to 1.0. 
        }
        private void UpdateWeights(double[] tValues, double learnRate, double momentum)
        {
            // Update the weights and biases using back-propagation. 
            // Assumes that SetWeights and ComputeOutputs have been called 
            // and matrices have values (other than 0.0). 
            if (tValues.Length != numOutput)
                throw new Exception("target values not same Length as output in UpdateWeights");

            // 1. Compute output gradients. 
            for (int i = 0; i < numOutput; ++i)
            {
                // Derivative for softmax = (1 - y) * y (same as log-sigmoid). 
                double derivative = (1 - outputs[i]) * outputs[i];
                // 'Mean squared error version' includes (1-y)(y) derivative. 
                oGrads[i] = derivative * (tValues[i] - outputs[i]);
            }

            // 2. Compute hidden gradients. 
            for (int i = 0; i < numHidden; ++i)
            {
                // Derivative of tanh = (1 - y) * (1 + y). 
                double derivative = (1 - hOutputs[i]) * (1 + hOutputs[i]);
                double sum = 0.0;
                for (int j = 0; j < numOutput; ++j) // Each hidden delta is the sum of numOutput terms. 
                {
                    double x = oGrads[j] * hoWeights[i][j];
                    sum += x;
                }
                hGrads[i] = derivative * sum;
            }

            // 3a. Update hidden weights (gradients must be computed right-to-left but weights 
            // can be updated in any order). 
            for (int i = 0; i < numInput; ++i) // 0..2 (3) 
            {

                for (int j = 0; j < numHidden; ++j) // 0..3 (4) 
                {
                    double delta = learnRate * hGrads[j] * inputs[i]; // Compute the new delta. 
                    ihWeights[i][j] += delta; // Update -- note '+' instead of '-'. 
                    // Now add momentum using previous delta. 
                    ihWeights[i][j] += momentum * ihPrevWeightsDelta[i][j];
                    ihPrevWeightsDelta[i][j] = delta; // Don't forget to save the delta for momentum . 
                }
            }

            // 3b. Update hidden biases. 
            for (int i = 0; i < numHidden; ++i)
            {
                double delta = learnRate * hGrads[i]; // 1.0 is constant input for bias. 
                hBiases[i] += delta;
                hBiases[i] += momentum * hPrevBiasesDelta[i]; // Momentum. 
                hPrevBiasesDelta[i] = delta; // Don't forget to save the delta. 
            }

            // 4. Update hidden-output weights. 
            for (int i = 0; i < numHidden; ++i)
            {
                for (int j = 0; j < numOutput; ++j)
                {
                    double delta = learnRate * oGrads[j] * hOutputs[i];
                    hoWeights[i][j] += delta;
                    hoWeights[i][j] += momentum * hoPrevWeightsDelta[i][j]; // Momentum. 
                    hoPrevWeightsDelta[i][j] = delta; // Save. 
                }
            }

            // 4b. Update output biases. 
            for (int i = 0; i < numOutput; ++i)
            {
                double delta = learnRate * oGrads[i] * 1.0;
                oBiases[i] += delta;
                oBiases[i] += momentum * oPrevBiasesDelta[i]; // Momentum. 
                oPrevBiasesDelta[i] = delta; // save 
            }
        } // UpdateWeights 

        public void Train(double[][] trainData, int maxEpochs, double learnRate, double momentum)
        {
            // Train a back-propagation style NN classifier using learning rate and momentum. 
            int epoch = 0;
            double[] xValues = new double[numInput]; // Inputs. 
            double[] tValues = new double[numOutput]; // Target values. 

            int[] sequence = new int[trainData.Length];
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;

            while (epoch < maxEpochs)
            {
                double mse = MeanSquaredError(trainData);
                if (mse < 0.040) break; // Consider passing value in as parameter. 

                Shuffle(sequence); // Visit each training data in random order.  

                for (int i = 0; i < trainData.Length; ++i)
                {
                    int idx = sequence[i];
                    Array.Copy(trainData[idx], xValues, numInput);
                    Array.Copy(trainData[idx], numInput, tValues, 0, numOutput);
                    ComputeOutputs(xValues); // Copy xValues in, compute outputs (store them internally). 
                    UpdateWeights(tValues, learnRate, momentum); // Find better weights. 
                } // Each training item. 
                ++epoch;
            }
        } // Train 

        private static void Shuffle(int[] sequence)
        {
            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }
        }

        private double MeanSquaredError(double[][] trainData) // Training stopping condition. 
        {
            // Average squared error per training item. 
            double sumSquaredError = 0.0;
            double[] xValues = new double[numInput]; // First numInput values in trainData. 
            double[] tValues = new double[numOutput]; // Last numOutput values. 

            // Walk through each training case. Looks like (6.9 3.2 5.7 2.3) (0 0 1). 
            for (int i = 0; i < trainData.Length; ++i)
            {
                Array.Copy(trainData[i], xValues, numInput);
                Array.Copy(trainData[i], numInput, tValues, 0, numOutput); // Get target values. 
                double[] yValues = this.ComputeOutputs(xValues); // Outputs using current weights. 
                for (int j = 0; j < numOutput; ++j)
                {
                    double err = tValues[j] - yValues[j];
                    sumSquaredError += err * err;
                }
            }

            return sumSquaredError / trainData.Length;
        }

        public double Accuracy(double[][] testData)
        {
            // Percentage correct using winner-takes all. 
            int numCorrect = 0;
            int numWrong = 0;
            double[] xValues = new double[numInput]; // Inputs. 
            double[] tValues = new double[numOutput]; // Targets. 
            double[] yValues; // Computed Y. 

            for (int i = 0; i < testData.Length; ++i)
            {
                Array.Copy(testData[i], xValues, numInput); // Get x-values.  


                Array.Copy(testData[i], numInput, tValues, 0, numOutput); // Get t-values. 
                yValues = this.ComputeOutputs(xValues);
                int maxIndex = MaxIndex(yValues); // Which cell in yValues has the largest value? 

                if (tValues[maxIndex] == 1.0) // ugly 
                    ++numCorrect;
                else
                    ++numWrong;
            }
            return (numCorrect * 1.0) / (numCorrect + numWrong); // No check for divide by zero. 
        }

        private static int MaxIndex(double[] vector) // Helper for Accuracy(). 
        {
            // Index of largest value. 
            int bigIndex = 0;
            double biggestVal = vector[0];
            for (int i = 0; i < vector.Length; ++i)
            {
                if (vector[i] > biggestVal)
                {
                    biggestVal = vector[i];
                    bigIndex = i;
                }
            }
            return bigIndex;
        } 
        #endregion

    }
}
