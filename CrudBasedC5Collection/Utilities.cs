using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasedCollection
{
    public class Utilities
    {
        public string[,] GenerateRandomCRUDOperationSequence(int numSequences, int lengthSequence)
        {
            string[,] sequences = new string[numSequences,lengthSequence];
            var random = new Random(80);
            var crudOps = new List<string>{
                        "C",
                        "R",
                        "U",
                        "D"};

            for (int i = 0; i < numSequences; i++)
            {
                for (int j = 0; j <lengthSequence; j++)
                {
                    int index = random.Next(crudOps.Count);
                    sequences[i,j] = crudOps[index];
                }
                               
            }

            return sequences;
        }

        public string[,] GeneratePercentRandomCRUDOperationSequence(int numSequences, int lengthSequence)
        {
            //use the same set of validate set to ensure full coverage
            int[,] crudValidateSetAllOps = new int[,]{
                                   {73,8,5,14}, //2 
                                   {8,72,12,8},                               
                                   
                                   {62,22,13,3}, //7
                                   {62,13,22,3},  
                                   {22,62,13,3},
                                   {22,3,62,13},                                   
                                   {3,22,13,62},
                                   {13,3,62,22},
                                   {3,13,22,62},

                                   {54,26,3,17}, //7
                                   {54,3,17,26},
                                   {26,17,54,3},
                                   {17,54,26,3},
                                   {3,26,17,54},
                                   {17,3,54,26},
                                   {3,17,26,54},

                                   {52,18,21,9}, //7
                                   {52,9,18,21},
                                   {21,52,18,9},
                                   {18,52,9,21},
                                   {21,18,9,52},
                                   {18,9,52,21},
                                   {9,52,21,18},                                   

                                   {43,27,21,9},//13
                                   {43,21,27,9},
                                   {43,21,9,27},
                                   {43,9,27,21},
                                   {27,43,9,21},
                                   {27,21,9,43},
                                   {27,9,21,43},
                                   {21,43,27,9},
                                   {21,27,43,9},
                                   {21,9,43,27},
                                   {9,43,27,21},
                                   {9,27,43,21},
                                   {9,21,43,27},
                                   
                                   {42,42,8,8},//6
                                   {42,8,42,8},
                                   {42,8,8,42},
                                   {8,42,42,8},
                                   {8,42,8,42},
                                   {8,8,42,42},

                                   {23,27,23,27},//4
                                   {27,23,27,23},
                                   {23,23,27,27},
                                   {27,27,23,23},
                                   //3 ops
                                    {81,9,10,0},//7
                                    {9,10,81,0},
                                    {10,9,0,81},
                                    {9,0,10,81},
                                    {0,81,9,10},
                                    {0,10,81,9},
                                    {0,9,10,81},

                                    {69,9,22,0},//12
                                    {69,0,9,22},
                                    {22,69,0,9},
                                    {22,9,69,0},
                                    {22,0,69,9},
                                    {9,69,22,0},
                                    {9,22,69,0},
                                    {9,22,0,69},
                                    {9,0,22,69},
                                    {0,69,9,22},
                                    {0,22,9,69},
                                    {0,9,22,69},

                                    {59,0,12,29},//6
                                    {29,59,12,0},
                                    {12,59,0,29},
                                    {12,0,59,29},
                                    {12,0,29,59},
                                    {0,29,12,59},
                                    
                                    {49,19,32,0},//10
                                    {49,0,19,32},
                                    {32,49,19,0},
                                    {32,49,0,19},
                                    {32,0,49,19},
                                    {19,49,32,0},
                                    {19,32,49,0},
                                    {19,0,49,32},
                                    {0,49,32,19},
                                    {0,32,49,19},
                                    
                                    {43,28,29,0},//10
                                    {43,28,0,29},
                                    {28,43,29,0},
                                    {29,43,0,28},
                                    {28,29,43,0},
                                    {29,28,0,43},
                                    {28,0,43,29},
                                    {29,0,28,43},
                                    {0,43,28,29},
                                    {0,29,43,28},                                    

                                    {32,34,31,3},//4
                                    {34,32,3,31},
                                    {32,3,31,34},
                                    {3,31,32,34},
                                    //2 ops
                                    {91,9,0,0}, //12
                                    {91,0,9,0},
                                    {91,0,0,9},
                                    {9,91,0,0},
                                    {9,0,91,0},
                                    {9,0,0,91},
                                    {0,91,9,0},
                                    {0,91,0,9},
                                    {0,9,91,0},
                                    {0,9,0,91},
                                    {0,0,91,9},
                                    {0,0,9,91},

                                    {82,18,0,0},//12 
                                    {82,0,18,0},
                                    {82,0,0,18},
                                    {18,82,0,0},
                                    {18,0,82,0},
                                    {18,0,0,82},
                                    {0,82,18,0},
                                    {0,82,0,18},
                                    {0,18,82,0},
                                    {0,18,0,82},
                                    {0,0,82,18},
                                    {0,0,18,82},

                                    {72,28,0,0},//12
                                    {72,0,28,0},
                                    {72,0,0,28},
                                    {28,72,0,0},
                                    {28,0,72,0},
                                    {28,0,0,72},
                                    {0,72,28,0},
                                    {0,72,0,28},
                                    {0,28,72,0},
                                    {0,28,0,72},
                                    {0,0,72,28},
                                    {0,0,28,72},
 
                                    {61,39,0,0},//12
                                    {61,0,39,0},
                                    {61,0,0,39},
                                    {39,61,0,0},
                                    {39,0,61,0},
                                    {39,0,0,61},
                                    {0,61,39,0},
                                    {0,61,0,39},
                                    {0,39,61,0},
                                    {0,39,0,61},
                                    {0,0,61,39},
                                    {0,0,39,61},
 
                                    {49,51,0,0},//6
                                    {51,0,49,0},
                                    {51,0,0,49},
                                    {0,49,51,0},
                                    {0,51,0,49},
                                    {0,0,49,51},
                                    //1 op
                                    {97,1,1,1},//5
                                    {1,98,1,0},
                                    {1,0,99,0},
                                    {0,1,1,98},
                                    {1,1,1,97}
                                  };
            
            int maxIndex = crudValidateSetAllOps.GetLength(0);

            Random rn = new Random();

             string[,] sequences = new string[numSequences, lengthSequence];

            for (int i = 0; i < numSequences; i++)
            {
                int opPercentIndex = rn.Next(maxIndex);

                var list = new[] {
                ProportionValue.Create((double)crudValidateSetAllOps[opPercentIndex, 0]/100.0, "C"), 
                ProportionValue.Create((double)crudValidateSetAllOps[opPercentIndex, 1]/100.0, "R"), 
                ProportionValue.Create((double)crudValidateSetAllOps[opPercentIndex, 2]/100.0, "U"), 
                ProportionValue.Create((double)crudValidateSetAllOps[opPercentIndex, 3]/100.0, "D") 
                };

                for (int j = 0; j <lengthSequence; j++)
                {
                    sequences[i,j] = list.ChooseByRandom();
                }
                               
            }
                       
            return sequences;
        }

        public void GenerateRandomCRUDOperationSequenceToCSV(int numSequences, int lengthSequence, string fileName)
        {
            //Delete if exist
            if (File.Exists(fileName))
                File.Delete(fileName);
            using(var w = new StreamWriter(fileName))
            {
                string[,] sequences = GeneratePercentRandomCRUDOperationSequence(numSequences, lengthSequence);

                for(int i=0; i<sequences.GetLength(0); i++)
                {
                    string[] seq = new string[sequences.GetLength(1)];;

                    for (int j = 0; j < sequences.GetLength(1); j++)
                    {
                        seq[j] = sequences[i, j];
                    }

                    string line = Concat(seq, 0, seq.Length);
                    
                    w.WriteLine(line);
                    w.Flush();
                }
            }
            
        }

        public string[] TranslateSequenceToXValues(string[] sequence, int startSize, string collectionInterface, out int lastSize)
        {
            int totalSize = sequence.Length;
            lastSize = startSize;

            Dictionary<string, int> crudOpCounts = new Dictionary<string, int>();
            crudOpCounts.Add("C", 0);
            crudOpCounts.Add("R", 0);
            crudOpCounts.Add("U", 0);
            crudOpCounts.Add("D", 0);

            for (int i = 0; i < totalSize; i++)
            {
                crudOpCounts[sequence[i]]++;

                //update the last size
                if (sequence[i] == "C")
                    lastSize = lastSize + 1;
                else if (sequence[i] == "D")
                lastSize = lastSize == 0? 0: lastSize - 1;//never go negative
                    
            }

            int cPercent = (int) (((double) crudOpCounts["C"] / (double) totalSize)*100.0);
            int rPercent = (int)(((double)crudOpCounts["R"] / (double)totalSize) * 100.0);
            int uPercent = (int)(((double)crudOpCounts["U"] / (double)totalSize) * 100.0); 
            int dPercent = 100 - cPercent - rPercent - uPercent; //to make sure they are sum up to a hundred
            //int dPercent = (crudOpCounts["D"] / totalSize) * 100;
            return new string[] {collectionInterface, startSize.ToString(), cPercent.ToString(), rPercent.ToString(), uPercent.ToString(), dPercent.ToString() };
        }

        public String Concat(string[] words, int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = start; i < end; i++)
                sb.Append((i > start ? "," : "") + words[i]);
            return sb.ToString();
        }

        public void SaveNeuralNetworkWeightsToCSV(string fileName, double[] weights)
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
    }

    public class ProportionValue<T>
    {
        public double Proportion { get; set; }
        public T Value { get; set; }
    }

    public static class ProportionValue
    {
        public static ProportionValue<T> Create<T>(double proportion, T value)
        {
            return new ProportionValue<T> { Proportion = proportion, Value = value };
        }

        static Random random = new Random();
        public static T ChooseByRandom<T>(
            this IEnumerable<ProportionValue<T>> collection)
        {
            var rnd = random.NextDouble();
            foreach (var item in collection)
            {
                if (rnd < item.Proportion)
                    return item.Value;
                rnd -= item.Proportion;
            }
            throw new InvalidOperationException(
                "The proportions in the collection do not add up to 1.");
        }
    }
}
