using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Developed by Junya Michanan, University of Denver
 * This N-Gram implementation is based on the string matching psuedo code provided by Millington (see Millington 2006, 583)
 * It can handle any window size greater than 1. It uses online learning, which means that the NGram predictor leans input sequences in real-time.
 * No data is stored permanently, the NGram predictor starts every instance without any data about the current input sequence.
 * */
namespace MachineLearning
{
    public struct KeyDataRecord
    {
        public Dictionary<string, int> counts;//holds the counts for each sucessor sequence 
        public int total;//holds the total number of times the window has been seen
    }

    public class NGramPredictor
    {
        private int total = 0;

        public int NValue { get; set; }
        public double PredictedProbabilty { get; set; }

        //Holds the frequency data
        Dictionary<string, KeyDataRecord> data;

        public NGramPredictor(int nValue)
        {
            NValue = nValue;
            data = new Dictionary<string, KeyDataRecord>();

            PredictedProbabilty = 0.0;
        }
        /// <summary>
        /// Register a set of input sequnces with predictor,
        /// updating its data. We assume the sequence has exactly NValue in it
        /// </summary>
        /// <param name="sequence">a sequence of NValue item</param>
        public void RegisterSequence(string[] sequences)
        {
            if (sequences.Length != NValue)
            {
                throw new Exception("Bad set of sequences. The member count is not the same as the N value!");
            }

            total++;

            string[] previousSeq = new string[NValue - 1];
            for (int i = 0; i < NValue - 1; i++)
            {
                previousSeq[i] = sequences[i];
            }

            string currentSeq = sequences[NValue - 1];

            string key = GetStringName(previousSeq);

            if (!data.ContainsKey(key))
            {
                data[key] = new KeyDataRecord(); 
            }

            KeyDataRecord keyData = data[key];

            if (keyData.counts == null)
                keyData.counts = new Dictionary<string, int>();

            if (!keyData.counts.ContainsKey(currentSeq))
                keyData.counts[currentSeq] = 0;

            keyData.counts[currentSeq] = keyData.counts[currentSeq] + 1;
            keyData.total += 1;

            data[key] = keyData;
            
        }
        /// <summary>
        /// Predict the next item most likey from the given one.
        /// We assume arrays of sequence items has NValue -1 elements in it
        /// </summary>
        /// <param name="sequence">a sequence of NValue-1 items</param>
        /// <returns></returns>
        public string PredictNext(string[] sequence)
        {
            if (sequence.Length != NValue - 1)
            {
                throw new Exception("Bad set of sequence items. The member count is not the same as the NValue-1!");
            }

            string key = GetStringName(sequence);

            if (!data.ContainsKey(key))
            {
                PredictedProbabilty = 0;
                return "Unknown";
            }

            KeyDataRecord keyData = data[key];

            //find the highest probability
            int highestValue = 0;
            string bestItem = "Unknown";

            //Get the list of items in the store
            ICollection<string> possibleItems = keyData.counts.Keys;

            //Go through each
            foreach (string ds in possibleItems)
            {
                //Check for the highest value
                if (keyData.counts[ds] > highestValue)
                {
                    //Store the item
                    highestValue = keyData.counts[ds];
                    bestItem = ds;
                    
                }
            }

            //calculate probability
            if (total > 0)
                PredictedProbabilty = ((double)highestValue/(double)keyData.total) * 100.0;

            return bestItem;
        }

        private string GetStringName(string[] sequence)
        {
            string name = string.Empty;

            for (int i = 0; i < sequence.Length; i++)
                name += sequence[i].ToString();
            return name;
        }
               
    }
}
