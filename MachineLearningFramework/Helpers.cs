using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public static class Helpers
    {
        #region Dataset helpers
        public static void MakeTrainTest(double[][] allData, int seed, out double[][] trainData,
                                           out double[][] testData)
        {
            // Split allData into 80% trainData and 20% testData. 
            Random rnd = new Random(seed);
            int totRows = allData.Length;
            int numCols = allData[0].Length;

            int trainRows = (int)(totRows * 0.80); // Hard-coded 80-20 split. 
            int testRows = totRows - trainRows;

            trainData = new double[trainRows][];
            testData = new double[testRows][];

            double[][] copy = new double[allData.Length][]; // Make a reference copy. 
            for (int i = 0; i < copy.Length; ++i)
                copy[i] = allData[i];

            // Scramble row order of copy. 
            for (int i = 0; i < copy.Length; ++i)
            {
                int r = rnd.Next(i, copy.Length);
                double[] tmp = copy[r];
                copy[r] = copy[i];
                copy[i] = tmp;
            }

            // Copy first trainRows from copy[][] to trainData[][]. 
            for (int i = 0; i < trainRows; ++i)
            {
                trainData[i] = new double[numCols];
                for (int j = 0; j < numCols; ++j)
                {
                    trainData[i][j] = copy[i][j];
                }
            }

            // Copy testRows rows of allData[] into testData[][]. 
            for (int i = 0; i < testRows; ++i) // i points into testData[][]. 
            {
                testData[i] = new double[numCols];
                for (int j = 0; j < numCols; ++j)
                {
                    testData[i][j] = copy[i + trainRows][j];
                }
            }
        } // MakeTrainTest 
        #endregion

        #region Data Normalization Helpers
        public static void GaussNormal(double[][] data, int column)
        {
            int j = column; // Convenience. 
            double sum = 0.0;
            for (int i = 0; i < data.Length; ++i)
                sum += data[i][j];
            double mean = sum / data.Length;

            double sumSquares = 0.0;
            for (int i = 0; i < data.Length; ++i)
                sumSquares += (data[i][j] - mean) * (data[i][j] - mean);
            double stdDev = Math.Sqrt(sumSquares / data.Length); 

            //alternative
            //double stdDev = Math.Sqrt(sumSquares / (data.Length - 1));

            for (int i = 0; i < data.Length; ++i)
                data[i][j] = (data[i][j] - mean) / stdDev; 

        }

        public static void GaussNormal(double[][] data, int column, out double stdDev, out double mean)
        {
            int j = column; // Convenience. 
            double sum = 0.0;
            for (int i = 0; i < data.Length; ++i)
                sum += data[i][j];
            mean = sum / data.Length;

            double sumSquares = 0.0;
            for (int i = 0; i < data.Length; ++i)
                sumSquares += (data[i][j] - mean) * (data[i][j] - mean);
            stdDev = Math.Sqrt(sumSquares / data.Length);

            //alternative
            //double stdDev = Math.Sqrt(sumSquares / (data.Length - 1));

            for (int i = 0; i < data.Length; ++i)
                data[i][j] = (data[i][j] - mean) / stdDev;

        }

        public static void GaussNormal(double[][] data, int column, double stdDev, double mean)
        {
           //this one use the mean and std from the training set

            //alternative
            //double stdDev = Math.Sqrt(sumSquares / (data.Length - 1));
            int j = column; // Convenience. 
            for (int i = 0; i < data.Length; ++i)
                data[i][j] = (data[i][j] - mean) / stdDev;

        }

        public static void MinMaxNormal(double[][] data, int column)
        {
             int j = column; 
             double min = data[0][j]; 
             double max = data[0][j]; 
             for (int i = 0; i < data.Length; ++i) 
             { 
             if (data[i][j] < min) 
             min = data[i][j]; 
             if (data[i][j] > max) 
             max = data[i][j];
             }

             double range = max - min;
             if (range == 0.0) // ugly 
             {
                 for (int i = 0; i < data.Length; ++i)
                     data[i][j] = 0.5;
                 return;
             }

             for (int i = 0; i < data.Length; ++i)
                 data[i][j] = (data[i][j] - min) / range;
        }

        #endregion

        #region Encoding Helpers
        public static void ShowMatrix(double[][] matrix, int decimals)
        {
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                {
                    double v = Math.Abs(matrix[i][j]);
                    if (matrix[i][j] >= 0.0)
                        Console.Write(" ");
                    else
                        Console.Write("-");
                    Console.Write(v.ToString("F" + decimals).PadRight(5) + " ");
                }
                Console.WriteLine("");
            }
        }
        /// <summary>
        /// The method is to create an encoded text file.
        /// This method accepts a path to a text file (which is assumed to be comma-delimited and WITHOUT a header line),
        ///a 0-based column to encode, and a string that can have a value "effects" or "dummy".
        /// </summary>
        /// <param name="originalFile"></param>
        /// <param name="encodedFile"></param>
        /// <param name="column"></param>
        /// <param name="encodingType">effects or dummy</param>
        public static void EncodeFile(string originalFile, string encodedFile, int column, string encodingType, out Dictionary<string, int> encodedDictionary)
        {
            // encodingType: "effects" or "dummy" 
            FileStream ifs = new FileStream(originalFile, FileMode.Open);
            StreamReader sr = new StreamReader(ifs);
            string line = "";
            string[] tokens = null;
            
            //Dictionary<string, int> encodedDictionary = new Dictionary<string, int>();
            encodedDictionary = new Dictionary<string, int>();

            int itemNum = 0;
            while ((line = sr.ReadLine()) != null)
            {
                
                tokens = line.Trim().Split(','); // Assumes items are comma-delimited. 
                if (encodedDictionary.ContainsKey(tokens[column]) == false)
                    encodedDictionary.Add(tokens[column], itemNum++);
                
            }
            sr.Close();
            ifs.Close();

            int N = encodedDictionary.Count; // Number of distinct strings. 
 
            ifs = new FileStream(originalFile, FileMode.Open); 
            sr = new StreamReader(ifs);  
            FileStream ofs = new FileStream(encodedFile, FileMode.Create); 
            StreamWriter sw = new StreamWriter(ofs); 
            string s = null; // Result line. 

            while ((line = sr.ReadLine()) != null) 
            { 
                 s = ""; 
                 tokens = line.Split(','); // Break apart strings. 
                    for (int i = 0; i < tokens.Length; ++i) // Reconstruct. 
                { 
                 if (i == column) // Encode this string. 
                 { 
                 int index = encodedDictionary[tokens[i]]; // 0, 1, 2, or . . . 
                 if (encodingType == "effects") 
                    s += EffectsEncoding(index, N) + ","; 
                 else if (encodingType == "dummy") 
                    s += DummyEncoding(index, N) + ","; 
                 } 
                 else 
                 s += tokens[i].Trim() +","; 
                } 
                s = s.Remove(s.Length - 1); // Remove trailing ','. 
                sw.WriteLine(s); // Write the string to file. 
            } // while 
 
            sw.Close(); ofs.Close(); 
            sr.Close(); ifs.Close(); 
        }
        /// <summary>
        /// Another encodeFile using the existing encoded dictionary
        /// </summary>
        /// <param name="originalFile"></param>
        /// <param name="encodedFile"></param>
        /// <param name="column"></param>
        /// <param name="encodingType"></param>
        /// <param name="encodedDictionary"></param>
        public static void EncodeFile(string originalFile, string encodedFile, int column, string encodingType, Dictionary<string, int> encodedDictionary)
        {
            string line = "";
            string[] tokens = null;

            int N = encodedDictionary.Count; // Number of distinct strings. 

            FileStream ifs = new FileStream(originalFile, FileMode.Open);
            StreamReader sr = new StreamReader(ifs);
            FileStream ofs = new FileStream(encodedFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(ofs);
            string s = null; // Result line. 

            while ((line = sr.ReadLine()) != null)
            {
                s = "";
                tokens = line.Split(','); // Break apart strings. 
                for (int i = 0; i < tokens.Length; ++i) // Reconstruct. 
                {
                    if (i == column) // Encode this string. 
                    {
                        int index = encodedDictionary[tokens[i]]; // 0, 1, 2, or . . . 
                        if (encodingType == "effects")
                            s += EffectsEncoding(index, N) + ",";
                        else if (encodingType == "dummy")
                            s += DummyEncoding(index, N) + ",";
                    }
                    else
                        s += tokens[i].Trim() + ",";
                }
                s = s.Remove(s.Length - 1); // Remove trailing ','. 
                sw.WriteLine(s); // Write the string to file. 
            } // while 

            sw.Close(); ofs.Close();
            sr.Close(); ifs.Close();
        } 
        /// <summary>
        /// Encoding non-numeric x-data to numeric values. Use 1-of-(N-1) effects encoding unless the predictor feature is binary,
        /// in which case use a -1 and +1 encoding.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static string EffectsEncoding(int index, int N)
        {
            if (N == 2)//case when the predictor feature (an x-value) is binary
            {
                if (index == 0) return "-1";
                else if (index == 1) return "1";
            }

            int[] values = new int[N - 1];
            if (index == N - 1) // Last item is all -1s. 
            {
                for (int i = 0; i < values.Length; ++i)
                    values[i] = -1;
            }
            else
            {
                values[index] = 1; // 0 values are already there. 
            }

            string s = values[0].ToString();
            for (int i = 1; i < values.Length; ++i)
                s += "," + values[i];
            return s; 

        }
        /// <summary>
        /// Encoding categorical y-data using 1-of-N dummy encoding, 
        /// unless the feature to be predicted is binary, in which case you can use either regular 1-of-N dummy encoding, or use 0-1 encoding. 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static string DummyEncoding(int index, int N)
        {
            int[] values = new int[N];
            values[index] = 1;

            string s = values[0].ToString();
            for (int i = 1; i < values.Length; ++i)
                s += "," + values[i];
            return s;
        }

        public static double[][] LoadData(string dataFile, int numRows, int numCols)
        {
            double[][] result = new double[numRows][];

            FileStream ifs = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(ifs);
            string line = "";
            string[] tokens = null;
            int i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                tokens = line.Split(',');
                result[i] = new double[numCols];
                for (int j = 0; j < numCols; ++j)
                {
                    result[i][j] = double.Parse(tokens[j]);
                }
                ++i;
            }
            sr.Close();
            ifs.Close();
            return result;
        }

        public static int MaxIndex(double[] vector)
        {
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

        #region Stream Parser
        public static List<string[]> parseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);                
            }

            return parsedData;
        }

        public static double[][] parseCSVToDoubleMatrix(string path)
        {
            List<double[]> parsedData = new List<double[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    double[] rowDouble;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        rowDouble = Array.ConvertAll(line.Split(','), double.Parse);
                        parsedData.Add(rowDouble);
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }

            return parsedData.ToArray();
        }

        public static String Concat(string[] words, int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = start; i < end; i++)
                sb.Append((i > start ? "," : "") + words[i]);
            return sb.ToString();
        }

        public static void SaveNeuralNetworkWeightsToCSV(string fileName, double[] weights)
        {
            //Delete if exist
            if (File.Exists(fileName))
                File.Delete(fileName);
            using (var w = new StreamWriter(fileName))
            {
                string[] wstrings = new string[weights.Length];
                for (int i = 0; i < weights.Length; i++)
                {
                    wstrings[i] = weights[i].ToString();
                }

                string line = Concat(wstrings, 0, wstrings.Length);

                w.WriteLine(line);
                w.Flush();
            }
        }

        public static void SaveDictionaryToCSV(string fileName, Dictionary<string, int> dict)
        {
            //Delete if exist
            if (File.Exists(fileName))
                File.Delete(fileName);
            using (var w = new StreamWriter(fileName))
            {
                foreach(var d in dict)
                {
                    string line = string.Format("{0},{1}", d.Key, d.Value);

                    w.WriteLine(line);
                    w.Flush();
                }

                
            }
        }
        #endregion



    }
}
