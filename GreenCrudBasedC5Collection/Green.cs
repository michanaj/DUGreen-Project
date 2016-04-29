using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning;


namespace GreenCrudBasedCollection
{    
    
    public class Green
    {
        #region Priori model and default variables
        //Default values from training
        //Setup the Neural Network with the hand-tuned parameters and calculated weights and biases
        int numInput = 10;
        int numHidden = 15;
        int numOutput = 9;
        static string weightString = "-14.0602309443756,33.2446572878224,23.0355278316868,-1.18254221712243,-15.7887252145233,-10.1864372216573,3.18508268705397,11.9623549803723,-6.21029971928799,-6.11773912643887,-3.57849464037879,-6.2500214486148,10.9869974567117,16.0144341817272,9.81941026986283,-4.92801630385957,12.4861705233458,9.28336252310617,0.918719576029432,-2.73233619412713,16.3705866875803,5.31426698269153,10.6140093120585,0.238653176363039,4.86855432223174,-2.2452401703003,12.1636978667284,-1.77110790799312,-1.91724528262524,-12.299326527493,-6.82375744615094,-6.05402661466641,-3.20997270543982,16.6537577238922,-14.8475227147734,-2.38160582238565,2.64349473378154,10.8324396112336,8.12813416804292,10.0903279294003,0.102414142137982,-5.3270484743823,-4.80747231192481,-9.07899437434968,-2.65312179525773,-5.93907579716376,27.6648521444668,-26.4479065051513,-3.82414639086316,12.3291751945322,-3.02011816816788,-0.000928080708911109,-17.3751110861096,-0.735820798012846,-5.20162962199745,4.56083081866892,-4.26102470406841,0.443551874288339,15.0144958454732,5.09004458243201,9.92066310790084,-33.8273071605911,7.56366299590899,1.81238105614815,8.97707218699597,1.89133627060419,-17.1600521771176,-7.91464851732432,-0.160590905702451,1.46221868673953,-3.63305782837402,11.6241704876313,-5.55771591240401,8.53474639193749,-0.854796322818782,-8.07637850878646,-20.974795944777,-15.8476889145736,-18.2839661149735,-0.340028541365469,-12.3642048697006,6.60136259108565,-5.49819198100266,-10.0967429844416,1.31590368588088,0.342769268856609,0.145562276181432,3.74703236985703,8.32715265879603,10.6309369038862,-1.87506793135038,-0.54354965763416,-2.3818645550729,10.6935387679578,-4.31121332864133,2.80981432300898,-1.23690857858485,-3.27787288872805,3.47708320173695,-33.1034299832121,-5.24723276274819,0.536200779896927,-4.18560861714108,3.03433565611661,-3.58463458751954,-2.11414760926211,-2.68864842317603,-1.6272668678943,0.359141392010048,2.15093322786596,0.0823825952093663,-0.135094971089157,0.498948852133667,2.25166162012784,-0.0387395234710194,-5.53879478831391,0.861347127791351,-5.94182857406388,3.29331518351644,-1.23760457251446,-1.7098154678502,-2.63103819976989,-1.69383662912437,0.23086270001823,1.96206586984188,0.146536876124998,-0.118057091311481,0.45667578712905,2.16742338425537,0.10228947889757,-5.19776213274016,0.65471921390523,-5.48411830731455,2.34777755236341,-1.09957401717134,8.70832070355545,-3.86199297201486,-1.42472624738028,-0.186947986387224,-4.22745722834164,0.824214336107958,1.09882647046332,-2.64597333202777,-3.5637315494156,2.42011112673285,15.7060231244953,-2.20281609085935,17.1468933536695,-1.10725297044377,2.41806229504388,-4.9418130170865,21.5461583203655,9.70139294970575,-3.14901604100236,0.243474348343117,-6.51239134047784,6.2426434786044,12.9709961072544,-11.0123359791519,-23.9445438989221,15.7506678216204,-2.04846785141523,19.5907774118315,4.28924154836217,7.81183478620262,39.8215107756625,41.3198249150261,40.0215003973958,35.4465871056995,38.0830638487569,44.0313752971932,39.869731450112,41.3141643047971,37.606077777248,-41.1269099613165,-38.2578459836415,-39.6700105098279,-38.1817039237761,-37.030749903911,-36.3172554139619,-39.1417929813005,-40.2202049109001,-35.722255659605,41.2356316926843,38.6592801223103,39.6703166284562,38.5696799836918,39.0061433995873,35.8281386545016,38.38383149084,39.1793128131401,38.5221000338128,41.0098769166522,32.3935000609494,36.0737181488136,37.9510785565552,36.4195925487343,39.6256791169096,35.0972412597143,35.8248786792384,35.9089336284605,28.8391849661629,41.1514720707756,42.456971408271,39.7929698645066,38.2045114419216,38.8930491811777,43.5911221643585,44.5055530787046,39.7302114152251,41.0767882189176,47.1330208694768,43.8244973851391,44.1703426286084,48.8931028510787,49.7410197302624,45.8891927670456,47.1565591554496,46.9080879586839,-41.4525080786516,-43.5915810613149,-40.2558186770252,-41.2698075526386,-39.7857980037561,-44.6390077308139,-40.2069916310192,-41.5224802625225,-32.5996105183943,-35.8550111168744,-44.1461171491206,-41.3119658100429,-40.3860758632803,-40.0039139499877,-39.4894822164556,-43.240686727901,-43.2458804412858,-36.0808856512378,43.0315474235938,43.3103330801576,39.5900257143519,42.2667564835229,39.8680496101873,39.1289465693909,41.8459912586478,40.4753491462453,40.6442877692672,-19.9598747030482,-17.9832978887057,-18.6695292270122,-18.0585733524717,-18.2450857693888,-16.6632855125467,-18.1152427593831,-21.3345895359199,-20.4515790064249,-3.3336342902947,-4.33457249851372,-4.28158887810467,-4.9057726733289,-4.60341529597749,-7.53048522306455,-5.12424779041894,0.29159138899774,-2.47750491699706,36.5241019792871,49.3007521845217,40.8184654866261,44.0839749646616,48.7237738390329,48.3911528160329,33.1873561696823,34.4699716395724,55.8722372021802,-45.8197292973949,-44.815746556589,-40.8907296145755,-43.0748937423134,-40.2978406581506,-40.5769968189723,-45.1850105053672,-44.8093027762674,-43.2146127613019,-51.8709196774922,-46.924765015979,-51.7292229045742,-45.5966472919194,-48.3486221814874,-44.6101036189304,-52.6563363989616,-50.2673895820263,-45.1866236015542,-37.6855254927227,-37.520834438975,-39.0514043896485,-40.2220335029361,-39.6058880286832,-38.2021798739965,-39.4172940528295,-38.0575223900055,-41.0531038335614,-49.1373775165591,-27.6194924705767,-40.8540700476716,-45.0540836438313,-41.6847755490272,-39.7948207874153,-36.9383299247023,-38.5090006267295,-45.6873943884957";
        string[] wStringarrays;
        double[] weights;

        //Gaussian Normalization values from the training results
        double gaussMeanX2 = 9950.2212950085, gaussStdvX2 = 15548.3096639413; //The Gaussian Normalization Values of X2 (Elm Size)
        double gaussMeanX3 = 23.9449853191161, gaussStdvX3 = 23.8826149010006;//The Gaussian Normalization Values of X3 (C)
        double gaussMeanX4 = 27.0638232112502, gaussStdvX4 = 25.2691970536526;//The Gaussian Normalization Values of X4 (R)
        double gaussMeanX5 = 24.9423582135682, gaussStdvX5 = 23.3831625684744;//The Gaussian Normalization Values of X5 (U) 
        double gaussMeanX6 = 23.8235203214341, gaussStdvX6 = 23.706462457807;//The Gaussian Normalization Values of X6 (D)

        Dictionary<string, int> C5Interfaces = new Dictionary<string, int>();
        Dictionary<int, string> yValues = new Dictionary<int, string>();

        //NGram
        int NValue = 2; //default as a bigram

        private Queue<string> ngram;

        //Temp variables
        int seqCount = 0;

        #endregion

        #region Private Properties
        bool isBusy = false;
        int registerCount = 0;
        CrudBasedCollection.C5DataStructure currentDataStructure;
        bool allowTransformation = false;
        #endregion

        #region Public Properties
        public NeuralNetwork Classifier { get; set; }
        public NGramPredictor Predictor { get; set; }
        public DecisionMaker DecisionMaker { get; set; }

        #endregion

        #region Constructor
        public Green() {

            
            wStringarrays = weightString.Split(',');
            weights = new double[wStringarrays.Length];

            //initialize the X, Y dictionary values for encoding/decoding purposes
            C5Interfaces.Add("ICollection", 0);
            C5Interfaces.Add("ICollectionBag", 1);
            C5Interfaces.Add("ICollectionSet", 2);
            C5Interfaces.Add("IList", 3);
            C5Interfaces.Add("IListBag", 4);
            C5Interfaces.Add("IListSet", 5);

            yValues.Add(0, "HashSet");
            yValues.Add(1, "ArrayList");
            yValues.Add(2, "SortedArray");
            yValues.Add(3, "TreeSet");
            yValues.Add(4, "TreeBag");
            yValues.Add(5, "LinkedList");
            yValues.Add(6, "HashedArrayList");
            yValues.Add(7, "HashedLinkedList");
            yValues.Add(8, "HashBag");

            //initilized the default weights and biases, A Priori model
            for (int i = 0; i < wStringarrays.Length; i++)
                weights[i] = double.Parse(wStringarrays[i]);

            //initialize Neural Network with a trained weights and biases
            Classifier = new NeuralNetwork(numInput, numHidden, numOutput);
            Classifier.SetWeights(weights);

            //Initialize NGram with Unknown data structures using a Queue
            ngram = new Queue<string>();
            for (int i = 0; i < NValue; i++)
                ngram.Enqueue("Unknown");

            //initialize NGram predictor
            Predictor = new NGramPredictor(NValue);

            //intialize Decision Maker
            DecisionMaker = new DecisionMaker();
            
        }
        #endregion

        #region  Methods
        /// <summary>
        /// Register feature to the CLassifier
        /// Format:G,N,%C,%R,%U,%D
        /// </summary>
        /// <param name="datastructureGroup"></param>
        /// <param name="elementSize">Number of elements in the data structure at the time of an operation, N</param>
        /// <param name="percentC">%C</param>
        /// <param name="percentR">%R</param>
        /// <param name="percentU">%U</param>
        /// <param name="percentD">%D</param>
        public void RegisterFeature(DataStructureGroup datastructureGroup, int elementSize, double percentC, double percentR, double percentU, double percentD, CrudBasedCollection.C5DataStructure currentDS)
        {
            while (!isBusy)//this is to make sure that the next feature will be registered only after the prior one has been completely registered.
            {
                isBusy = true;
                currentDataStructure = currentDS;

                
                //1. Encode and normalize the feature values
                double[] x = new double[10]; //10 input values of the ANN
                double[] encodedNormInterfaceAndSetBagProperty = EncodeNormalizeDataStructureGroup(datastructureGroup); //X1 Feature (5-length array)
                Array.Copy(encodedNormInterfaceAndSetBagProperty, 0, x, 0, 5); //copy to the first 5 arrays
                x[5] = (elementSize - gaussMeanX2) / gaussStdvX2; //X2 Feature
                x[6] = (percentC - gaussMeanX3) / gaussStdvX3; //X3 Feature
                x[7] = (percentR - gaussMeanX4) / gaussStdvX4; //X4 Feature
                x[8] = (percentU - gaussMeanX5) / gaussStdvX5; //X5 Feature
                x[9] = (percentD - gaussMeanX6) / gaussStdvX6; //X6 Feature;

                //2. Register to Classifier and compute the data structure
                double[] computed_y = Classifier.ComputeOutputs(x);//9 output values of the ANN
                //using winner-take-all method to evaluate y values
                int maxComputedYIndex = MaxIndex(computed_y);
                string computedDS = yValues[maxComputedYIndex];
                ngram.Dequeue();
                ngram.Enqueue(computedDS);
                
                //3. Register the computed DS as an N-Gram sequence to the Predictor
                Predictor.RegisterSequence(ngram.ToArray());
                string[] ngramMinus1 = new string[NValue-1];
                Array.Copy(ngram.ToArray(),1,ngramMinus1,0,NValue-1);
                CrudBasedCollection.C5DataStructure predictedDS = GetC5DataStructure(Predictor.PredictNext(ngramMinus1));                
                isBusy = false;
                registerCount++;
                //For debugging or simulation purposes only, comment this line to improve performance
                OnFeatureRegistered(new FeatureRegisteredEventArgs(datastructureGroup, elementSize, percentC, percentR, percentU, percentD, computedDS, currentDataStructure.ToString(), Predictor.PredictedProbabilty, predictedDS.ToString(), registerCount));//raise an event
                
                bool reset = false;
                
                DecisionMaker.MakeDecision(registerCount, currentDataStructure, predictedDS, Predictor.PredictedProbabilty, out reset);
                if (reset)
                   registerCount = 0;
               
                
                break;
            }
            
        }
        
        
        private int MaxIndex(double[] vector)  
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

        public static CrudBasedCollection.C5DataStructure GetC5DataStructure(string c5DataStructureName)
        {
            CrudBasedCollection.C5DataStructure ds;
            switch (c5DataStructureName)
            {
                case "HashSet":
                    ds = CrudBasedCollection.C5DataStructure.HashSet;
                    break;
                case "HashBag":
                    ds = CrudBasedCollection.C5DataStructure.HashBag;
                    break;
                case "TreeSet":
                    ds = CrudBasedCollection.C5DataStructure.TreeSet;
                    break;
                case "TreeBag":
                    ds = CrudBasedCollection.C5DataStructure.TreeBag;
                    break;
                case "HashedArrayList":
                    ds = CrudBasedCollection.C5DataStructure.HashedArrayList;
                    break;
                case "HashedLinkedList":
                    ds = CrudBasedCollection.C5DataStructure.HashedLinkedList;
                    break;
                case "SortedArray":
                    ds = CrudBasedCollection.C5DataStructure.SortedArray;
                    break;
                case "ArrayList":
                    ds = CrudBasedCollection.C5DataStructure.ArrayList;
                    break;
                case "LinkedList":
                    ds = CrudBasedCollection.C5DataStructure.LinkedList;
                    break;
                default: 
                    ds = CrudBasedCollection.C5DataStructure.Unknown;
                    break;
            }
            return ds;
        }
        
        private double[] EncodeNormalizeDataStructureGroup(DataStructureGroup datastructureGroup)
        {
            //encoding using 1-of-(N-1) 
            //ICollection,0
            //ICollectionBag,1
            //ICollectionSet,2
            //IList,3
            //IListBag,4
            //IListSet,5
            //THIS VALUES ARE STATIC AND SHOULD BE CHANGED ACCORDINGLY BASED ON THE THE VALUES FROM TRAINING DATASET
            double[] icollection = new double[] { 1.0, 0.0, 0.0, 0.0, 0.0 };
            double[] icollectionBag = new double[] { 0.0, 1.0, 0.0, 0.0, 0.0 };
            double[] icollectionSet = new double[] { 0.0, 0.0, 1.0, 0.0, 0.0 };
            double[] iList = new double[] { 0.0, 0.0, 0.0, 1.0, 0.0 };
            double[] iListBag = new double[] { 0.0, 0.0, 0.0, 0.0, 1.0 };
            double[] iListSet = new double[] { -1.0, -1.0, -1.0, -1.0, -1.0 };

            if (datastructureGroup == DataStructureGroup.ICollection)
                return icollection;
            else if (datastructureGroup == DataStructureGroup.ICollectionBag)
                return icollectionBag;
            else if (datastructureGroup == DataStructureGroup.ICollectionSet)
                return icollectionSet;
            else if (datastructureGroup == DataStructureGroup.IList)
                return iList;
            else if (datastructureGroup == DataStructureGroup.IListBag)
                return iListBag;
            else if (datastructureGroup == DataStructureGroup.IListSet)
                return iListSet;
            else
                return new double[] { 1.0, 1.0, 1.0, 1.0, 1.0 };//unknown
        }
        
        protected virtual void OnFeatureRegistered(FeatureRegisteredEventArgs e)
        {
            FeatureRegisteredEventHandler handler = FeatureRegistered;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event FeatureRegisteredEventHandler FeatureRegistered;
    }

    #endregion

        #region Delegates and Event methods
        // A delegate type for hooking up feature registerd notifications.
        public delegate void FeatureRegisteredEventHandler(object sender, FeatureRegisteredEventArgs e);   

        public class FeatureRegisteredEventArgs : EventArgs
        {
            public readonly DataStructureGroup InterfaceAndSetBagPropertyGroup;
            public readonly int ElementSizeAtTimeOfOperation;
            public readonly double PercentC;
            public readonly double PercentR;
            public readonly double PercentU;
            public readonly double PercentD;
            public readonly string ClassifiedDataStructure;
            //The followings are not features. They are just for tracking purpose only.
            public readonly string CurrentDataStructure;
            public readonly double PredictionProbabilityValue;
            public readonly string NextPredictedDataStructure;
            public readonly int RegisteredCountFromLastTransformation;

            public FeatureRegisteredEventArgs (DataStructureGroup datastructureGroup, int elementSize, double percentC, double percentR, double percentU, double percentD, 
                string classifiedDS, string currentDS, double predictedProbValue, string nextDS, int regCount)
            {
                this.InterfaceAndSetBagPropertyGroup = datastructureGroup;
                this.ElementSizeAtTimeOfOperation = elementSize;
                this.PercentC = percentC;
                this.PercentR = percentR;
                this.PercentU = percentU;
                this.PercentD = percentD;
                this.ClassifiedDataStructure = classifiedDS;
                this.CurrentDataStructure = currentDS;
                this.PredictionProbabilityValue = predictedProbValue;
                this.NextPredictedDataStructure = nextDS;
                this.RegisteredCountFromLastTransformation = regCount;
            }
        }


        // A delegate type for hooking up transform notifications.
        public delegate void TransformNotifiedEventHandler(object sender, TransformNotifiedEventArgs e);   

        public class TransformNotifiedEventArgs : EventArgs
        {
            public readonly CrudBasedCollection.C5DataStructure TransformToDataStructure;
            public TransformNotifiedEventArgs (CrudBasedCollection.C5DataStructure dataStructure)
            {
                this.TransformToDataStructure = dataStructure;
            }
        }
    #endregion

        #region Decision Maker Class
        public class DecisionMaker
            {
                #region Public Properties
                public bool IsDynamicMode { get; set; }
                public int RegisteredDatastructureCountThreshold { get; set; }
                public double PredictedPropbabilityThreshold { get; set; } 
                public CrudBasedCollection.C5DataStructure PredictedDataStructure {get; set;}
                public int YesDecisionCount { get; set; }//for tracing purpose only

                #endregion
                public DecisionMaker() 
                {
                    //TODO? Need more research on the proper threshold numbers
                    RegisteredDatastructureCountThreshold = 5;
                    PredictedPropbabilityThreshold = 60.0;

                    //default to Dynamic Mode
                    IsDynamicMode = true;

                    YesDecisionCount = 0;
                }

                public void MakeDecision(int registeredSubsequenceCount, CrudBasedCollection.C5DataStructure currentDataStructure, CrudBasedCollection.C5DataStructure predictedDataStructure, double probability, out bool resetCount) 
                {
                    resetCount = false;

                    //main logic for the dicision making           
                    if (registeredSubsequenceCount >= RegisteredDatastructureCountThreshold //   then check if data structure has been registered to the ngram for a certian threshold
                        && probability >= PredictedPropbabilityThreshold)                     //   and the predicted probability number is equal or greater than a certain threshold
                    {
                        //trace the number of Yes
                        YesDecisionCount++;

                        if (predictedDataStructure != CrudBasedCollection.C5DataStructure.Unknown && //if the predicted DS is not Unknown or not the same as the current one
                            currentDataStructure != predictedDataStructure)
                        {
                            PredictedDataStructure = predictedDataStructure;
                            resetCount = true;//the count will be reset every transformation is made
                            if (IsDynamicMode)               //Make notify only when run on Dynamic Mode  
                                OnTransformationNotified(new TransformNotifiedEventArgs(predictedDataStructure));//Raise an event to notify the Green data structure to transform to the specific data structure
                        }
                     }
            
                }        
       
       
                 #region Events and Event Handlers
                 // An event that clients can use to be notified whenever the
                  //notification is raised.
                  public event TransformNotifiedEventHandler TransformationNotified;

                  // Invoke the OnTransformed event; called whenever transformation is notified
                  protected virtual void OnTransformationNotified(TransformNotifiedEventArgs e) 
                  {
                     if (TransformationNotified != null)
                        TransformationNotified(this, e);
                  }
      
                 #endregion
            }
        #endregion

}
