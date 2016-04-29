using CrudBasedCollection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WattsUpFramework;
using CBC = CrudBasedCollection;
using MachineLearning;

namespace CrudeBasedCollectionEnergyProfiler
{
    class Program
    {
        static double watts = 0;
        static DateTime timeWattRead = DateTime.Now;
        static string err = string.Empty;
        static WattsUp _MyWattsUp = new WattsUp();
        static int countWattUpReading = 0;
        static double totalWatts = 0.0;
        
        //base power calculation
        static int basePowerFlag = 0;
        static int basedPowerReadingCount = 0;
        static double totalBasePower = 0.0;
        //====================================

       
        //Change mode here  true = training set, validate set otherwise.
        static bool _TrainingMode = true;//false;//
        
        static void Main(string[] args)
        {
            //This is the start of all overhead and consider them as the based system
            //Start the time clock
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            ////Create a WattsUp object
            //WattsUp _MyWattsUp = new WattsUp();
            _MyWattsUp.InternalSamplingRate = 1;
            _MyWattsUp.SamplingRate = 1;
            _MyWattsUp.IsUseWattsDelta = true;
            _MyWattsUp.OnWuReading += new WuReadingEventHandler(myWattsUp_OnWuReading);
            //_MyWattsUp.Start(out err);
            StartWattsUpAsynch();

            Console.WriteLine("Sleep..10 sec.");
            System.Threading.Thread.Sleep(10000);    
            //Get based power in watts and warm up           
            basePowerFlag = 1;            
   
            double basedPower = totalBasePower / Double.Parse(basedPowerReadingCount.ToString());
            Console.WriteLine("BASED Power:" + basedPower);

            //reset the count
            countWattUpReading = 0;
            //warm up
            Console.WriteLine("Warming up....");
            CBC.CrudBasedCollection<string> ds = new CrudBasedCollection<string>();
            for(int i = 0; i< 10000; i++)
                ds.Create("Test");
            while (true)
            {
                if (countWattUpReading >= 10)
                {
                    ds.Clear();
                    break;
                }
            }

            //Create a file
            string fileName = "MyTestResult.csv";
            //Delete if exist
            if(File.Exists(fileName))
                File.Delete(fileName);
            //Create
            StreamWriter CsvfileWriter = new StreamWriter(fileName, true);

            //List of all C5 data structures
            C5DataStructure[] c5DSs = new C5DataStructure[] {C5DataStructure.ArrayList, C5DataStructure.HashBag, C5DataStructure.HashedArrayList, 
                                                            C5DataStructure.HashedLinkedList, C5DataStructure.HashSet,
                                                            C5DataStructure.LinkedList, C5DataStructure.SortedArray, C5DataStructure.TreeBag, C5DataStructure.TreeSet, 
                                                            };//9 data structures
            //start element size for training set
            int[] elmSizes = //new int[] { 0, 100, 1000, 10000, 100000}; //option 1: 5 possibilities
                             new int[] {
                                 0, 50, 100, 500, 1000, 5000, 10000, 
                                 50000 
                             }; //option 2: 8 possibilities

            int[] moreElmSize = new int[] {15000,20000,25000, 30000, 35000, 40000, 45000};

            int[] moreElmSize2 = new int[] { 20, 40, 60, 80, 150, 200, 300, 400, 600, 700, 800, 900};
                             
           
            //all possibilities of all proportions of CRUD operations
            //for actual dataset, 80% for learning and 20% for testing
            int[,] crudDatasetOps = new int[,]{//All 4 ops
                                   {70,10,10,10}, //4 
                                   {10,70,10,10},
                                   {10,10,70,10},
                                   {10,10,10,70},
                                   
                                   {60,20,10,10}, //12
                                   {60,10,20,10},  
                                   {60,10,10,20},
                                   {20,60,10,10},
                                   {20,10,60,10},
                                   {20,10,10,60},                                   
                                   {10,60,20,10},
                                   {10,60,10,20},
                                   {10,20,60,10},
                                   {10,20,10,60},
                                   {10,10,60,20},
                                   {10,10,20,60},

                                   {50,30,10,10}, //12
                                   {50,10,30,10},  
                                   {50,10,10,30},
                                   {30,50,10,10},
                                   {30,10,50,10},
                                   {30,10,10,50},                                   
                                   {10,50,30,10},
                                   {10,50,10,30},
                                   {10,30,50,10},
                                   {10,30,10,50},
                                   {10,10,50,30},
                                   {10,10,30,50},

                                   {50,20,20,10}, //12
                                   {50,20,10,20},
                                   {50,10,20,20},
                                   {20,50,20,10},
                                   {20,50,10,20},
                                   {20,20,50,10},
                                   {20,20,10,50},
                                   {20,10,50,20},
                                   {20,10,20,50},
                                   {10,50,20,20},
                                   {10,20,50,20},
                                   {10,20,20,50},

                                   {40,30,20,10},//24
                                   {40,30,10,20},
                                   {40,20,30,10},
                                   {40,20,10,30},
                                   {40,10,20,30},
                                   {40,10,30,20},
                                   {30,40,20,10},
                                   {30,40,10,20},
                                   {30,20,40,10},
                                   {30,20,10,40},
                                   {30,10,40,20},
                                   {30,10,20,40},
                                   {20,40,30,10},
                                   {20,40,10,30},
                                   {20,30,40,10},
                                   {20,30,10,40},
                                   {20,10,40,30},
                                   {20,10,30,40},
                                   {10,40,30,20},
                                   {10,40,20,30},
                                   {10,30,40,20},
                                   {10,30,20,40},
                                   {10,20,40,30},
                                   {10,20,30,40},

                                   {40,40,10,10},//6
                                   {40,10,40,10},
                                   {40,10,10,40},
                                   {10,40,40,10},
                                   {10,40,10,40},
                                   {10,10,40,40},

                                   {25,25,25,25},//1
                                   //3 ops
                                    {80,10,10,0},//12
                                    {80,10,0,10},
                                    {80,0,10,10},
                                    {10,80,10,0},
                                    {10,80,0,10},
                                    {10,10,80,0},
                                    {10,10,0,80},
                                    {10,0,80,10},
                                    {10,0,10,80},
                                    {0,80,10,10},
                                    {0,10,80,10},
                                    {0,10,10,80},

                                    {70,20,10,0},//24
                                    {70,20,0,10},
                                    {70,10,20,0},
                                    {70,10,0,20},
                                    {70,0,20,10},
                                    {70,0,10,20},
                                    {20,70,10,0},
                                    {20,70,0,10},
                                    {20,10,70,0},
                                    {20,10,0,70},
                                    {20,0,70,10},
                                    {20,0,10,70},
                                    {10,70,20,0},
                                    {10,70,0,20},
                                    {10,20,70,0},
                                    {10,20,0,70},
                                    {10,0,70,20},
                                    {10,0,20,70},
                                    {0,70,20,10},
                                    {0,70,10,20},
                                    {0,20,70,10},
                                    {0,20,10,70},
                                    {0,10,70,20},
                                    {0,10,20,70},

                                    {60,30,10,0},//24 
                                    {60,30,0,10},
                                    {60,10,30,0},
                                    {60,10,0,30},
                                    {60,0,30,10},
                                    {60,0,10,30},
                                    {30,60,10,0},
                                    {30,60,0,10},
                                    {30,10,60,0},
                                    {30,10,0,60},
                                    {30,0,60,10},
                                    {30,0,10,60},
                                    {10,60,30,0},
                                    {10,60,0,30},
                                    {10,30,60,0},
                                    {10,30,0,60},
                                    {10,0,60,30},
                                    {10,0,30,60},
                                    {0,60,30,10},
                                    {0,60,10,30},
                                    {0,30,60,10},
                                    {0,30,10,60},
                                    {0,10,60,30},
                                    {0,10,30,10},

                                    {50,30,20,0},//24
                                    {50,30,0,20},
                                    {50,20,30,0},
                                    {50,20,0,30},
                                    {50,0,30,20},
                                    {50,0,20,30},
                                    {30,50,20,0},
                                    {30,50,0,20},
                                    {30,20,50,0},
                                    {30,20,0,50},
                                    {30,0,50,20},
                                    {30,0,20,50},
                                    {20,50,30,0},
                                    {20,50,0,30},
                                    {20,30,50,0},
                                    {20,30,0,50},
                                    {20,0,50,30},
                                    {20,0,30,50},
                                    {0,50,30,20},
                                    {0,50,20,30},
                                    {0,30,50,20},
                                    {0,30,20,50},
                                    {0,20,50,30},
                                    {0,20,30,50},

                                    {40,30,30,0},//12
                                    {40,30,0,30},
                                    {40,0,30,30},
                                    {30,40,30,0},
                                    {30,40,0,30},
                                    {30,30,40,0},
                                    {30,30,0,40},
                                    {30,0,40,30},
                                    {30,0,30,40},
                                    {0,40,30,30},
                                    {0,30,40,30},
                                    {0,30,30,40},

                                    {33,33,33,0},//4
                                    {33,33,0,33},
                                    {33,0,33,33},
                                    {0,33,33,33},
                                    //2 ops
                                    {90,10,0,0}, //12
                                    {90,0,10,0},
                                    {90,0,0,10},
                                    {10,90,0,0},
                                    {10,0,90,0},
                                    {10,0,0,90},
                                    {0,90,10,0},
                                    {0,90,0,10},
                                    {0,10,90,0},
                                    {0,10,0,90},
                                    {0,0,90,10},
                                    {0,0,10,90},

                                    {80,20,0,0},//12 
                                    {80,0,20,0},
                                    {80,0,0,20},
                                    {20,80,0,0},
                                    {20,0,80,0},
                                    {20,0,0,80},
                                    {0,80,20,0},
                                    {0,80,0,20},
                                    {0,20,80,0},
                                    {0,20,0,80},
                                    {0,0,80,20},
                                    {0,0,20,80},

                                    {70,30,0,0},//12
                                    {70,0,30,0},
                                    {70,0,0,30},
                                    {30,70,0,0},
                                    {30,0,70,0},
                                    {30,0,0,70},
                                    {0,70,30,0},
                                    {0,70,0,30},
                                    {0,30,70,0},
                                    {0,30,0,70},
                                    {0,0,70,30},
                                    {0,0,30,70},
 
                                    {60,40,0,0},//12
                                    {60,0,40,0},
                                    {60,0,0,40},
                                    {40,60,0,0},
                                    {40,0,60,0},
                                    {40,0,0,60},
                                    {0,60,40,0},
                                    {0,60,0,40},
                                    {0,40,60,0},
                                    {0,40,0,60},
                                    {0,0,60,40},
                                    {0,0,40,60},
 
                                    {50,50,0,0},//6
                                    {50,0,50,0},
                                    {50,0,0,50},
                                    {0,50,50,0},
                                    {0,50,0,50},
                                    {0,0,50,50},
                                    //1 op
                                    {100,0,0,0},//4
                                    {0,100,0,0},
                                    {0,0,100,0},
                                    {0,0,0,100}
                                  }; 

            //Total Possibilities = 4 + 12 + 12 + 12 + 24 + 6 + 1 + 12 + 24 + 24 + 24 + 12 + 4 + 12 + 12 +12 + 12 + 6 + 4 = 229
            
            //Validate sets
            int[] elmSizesValidateSet = new int[] { 2, 93, 143, 421, 1634, 3521, 8374, 47538 }; //for validate set            
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
            
            //constant OP lenght and test string object
            int opLength = 10000;
            String testString = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest";
            int count = 1;

            //reset the count
            countWattUpReading = 0;
            
            //Start
            Console.WriteLine("Start...");
            if (_TrainingMode)
                //ProfileEnergyEnergyDataSet(stopwatch, ref ds, CsvfileWriter, c5DSs, elmSizes, crudDatasetOps, opLength, testString, ref count);
                ProfileEnergyEnergyDataSet(stopwatch, ref ds, CsvfileWriter, c5DSs, moreElmSize2, crudDatasetOps, opLength, testString, ref count);
            else//if create validate set
                ProfileEnergyEnergyDataSet(stopwatch, ref ds, CsvfileWriter, c5DSs, elmSizesValidateSet, crudValidateSetAllOps, opLength, testString, ref count);
            //profile random programs
            //ProfileEnergyEnergyRandomProgram(stopwatch, ref ds, c5DSs, opLength, testString, ref count);

            //profile realworld programs
            //ProfileEnergyEnergyRealWorldProgram(stopwatch, ref ds, c5DSs, opLength, testString, ref count);
            //Cleanup
            CsvfileWriter.Close();
            _MyWattsUp.Stop();
            
        }
        /// <summary>
        /// Energy Profiling
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <param name="ds"></param>
        /// <param name="CsvfileWriter"></param>
        /// <param name="c5DSs"></param>
        /// <param name="elmSizes"></param>
        /// <param name="crudDatasetOps"></param>
        /// <param name="opLength"></param>
        /// <param name="testString"></param>
        /// <param name="count"></param>
        private static void ProfileEnergyEnergyDataSet(Stopwatch stopwatch, ref CBC.CrudBasedCollection<string> ds, StreamWriter CsvfileWriter, C5DataStructure[] c5DSs, int[] elmSizes, int[,] crudDatasetOps, int opLength, String testString, ref int count)
        {
            //for each start element size
            for (int i = 0; i < elmSizes.Length; i++)
            {

                //for each C5 data structure
                for (int j = 0; j < c5DSs.Length; j++)
                {
                    Console.WriteLine("Start " + c5DSs[j].ToString() + " with start element size = " + elmSizes[i]);


                    //for each CRUD op proportion
                    for (int k = 0; k < crudDatasetOps.GetLength(0); k++)
                    {
                        ds = new CBC.CrudBasedCollection<string>(c5DSs[j]);
                        //calculate number of CRUD operations
                        int C = ((crudDatasetOps[k, 0]) * opLength) / 100;
                        int R = ((crudDatasetOps[k, 1]) * opLength) / 100;
                        int U = ((crudDatasetOps[k, 2]) * opLength) / 100;
                        int D = ((crudDatasetOps[k, 3]) * opLength) / 100;
                        int basePopulationSize = elmSizes[i];
                        double totaltime = 0.0;
                        int iterationCount = 0;
                        totalWatts = 0.0;
                        countWattUpReading = 0;
                        while (true)//keep doing until the wattsup readings is greater than some threshole
                        {

                            bool t;
                            //Generating based population
                            for (int p = 0; p < basePopulationSize; p++)
                            {
                                t = ds.Create(testString + p);
                            }


                            //start the clock
                            stopwatch.Restart();

                            //Conducting the operations
                            AllOps(ds, basePopulationSize, opLength, testString, C, R, U, D);

                            //stop the clock
                            stopwatch.Stop();

                            //time in second
                            totaltime = totaltime + (stopwatch.Elapsed.TotalSeconds);
                            iterationCount++;

                            //clear all elements
                            ds.Clear();

                            //check if its time to stop
                            if (countWattUpReading >= 5)
                            {
                                break;
                            }
                        }

                        double averageTime = totaltime / iterationCount;
                        double averageWatts = totalWatts / (countWattUpReading - 3);//the first two are ignore because of the delay

                        string result = count + "," + c5DSs[j].ToString() + "," + elmSizes[i] + "," + crudDatasetOps[k, 0].ToString() + "," + crudDatasetOps[k, 1].ToString() + "," + crudDatasetOps[k, 2].ToString() + "," + crudDatasetOps[k, 3].ToString() + " #Iteration:" + iterationCount + " Avg Time(Sec):" + averageTime.ToString() + " Avg Power(Watts): " + averageWatts.ToString() + ", Energy (J):" + (averageTime * averageWatts).ToString();
                        Console.WriteLine("Result:" + result);
                        string savedToFileResult = count + "," + c5DSs[j].ToString() + "," + elmSizes[i] + "," + crudDatasetOps[k, 0].ToString() + "," + crudDatasetOps[k, 1].ToString() + "," + crudDatasetOps[k, 2].ToString() + "," + crudDatasetOps[k, 3].ToString() + "," + averageTime.ToString() + "," + averageWatts.ToString() + "," + (averageTime * averageWatts).ToString();
                        CsvfileWriter.WriteLine(savedToFileResult);

                        //reset the count to zero
                        totalWatts = 0.0;
                        countWattUpReading = 0;
                        totaltime = 0.0;
                        count++;

                    }


                }
            }
        }

        
        private static void ProfileEnergyEnergyRealWorldProgram(Stopwatch stopwatch, ref CBC.CrudBasedCollection<string> ds, C5DataStructure[] c5DSs, int opLength, String testString, ref int count)
        {
           //List of real-world programs
            List<string> programs = new List<string>();
            programs.Add("Raw_AStarPathFinderProgram_Custom_Formula.csv");
            programs.Add("Raw_AStarPathFinderProgram_DiagonalShortcut_Formula.csv");
            programs.Add("Raw_AStarPathFinderProgram_Euclidean_Formula.csv");
            programs.Add("Raw_AStarPathFinderProgram_Manhatan_Formula.csv");
            programs.Add("Raw_AStarPathFinderProgram_MaxDXDY_Formula.csv");
            programs.Add("Raw_GA-FittnessTableProgram.csv");
            programs.Add("Raw_GA-NextGenerationProgram.csv");
            programs.Add("Raw_GA-ThisGenerationProgram.csv");
            programs.Add("Raw_HuffmanCodingProgram.csv");
           //Run three Runs
            for(int run =0; run<3;run++)
            //for each real-world programs
            foreach (string program in programs)           
            {
                string applicationRawXValuesFileName = string.Format(@"RealWorldPrograms\{0}", program);
                //read file
                //1. Read data from a Dataset
                List<string[]> rawDatasetArrays = Helpers.parseCSV(applicationRawXValuesFileName);

                //for writing result
                string writeFileName = "";
                if (run == 0)
                    writeFileName = string.Format(@"RealWorldPrograms\Run I\Result_{0}", program);
                else if (run == 1)
                    writeFileName = string.Format(@"RealWorldPrograms\Run II\Result_{0}", program);
                else if (run == 2)
                    writeFileName = string.Format(@"RealWorldPrograms\Run III\Result_{0}", program);
                else
                    break;

                //Delete if exist
                if (File.Exists(writeFileName))
                    File.Delete(writeFileName);
                //Create
                StreamWriter CsvfileWriter = new StreamWriter(writeFileName, true);
                                
                Console.WriteLine("Start profiling progrm " + applicationRawXValuesFileName +" ...");

                foreach (var d in rawDatasetArrays)
                {
                    
                    //for each C5 data structure
                    for (int j = 0; j < c5DSs.Length; j++)
                    {

                        Console.WriteLine("Start " + c5DSs[j].ToString() + " with start element size = " + d[0]);


                        //for each CRUD op proportion
                        //for (int k = 0; k < crudDatasetOps.GetLength(0); k++)
                        //{
                        ds = new CBC.CrudBasedCollection<string>(c5DSs[j]);
                        //calculate number of CRUD operations
                        int C = (int.Parse(d[1]) * opLength) / 100;
                        int R = (int.Parse(d[2]) * opLength) / 100;
                        int U = (int.Parse(d[3]) * opLength) / 100;
                        int D = (int.Parse(d[4]) * opLength) / 100;
                        int basePopulationSize = int.Parse(d[0]);
                        double totaltime = 0.0;
                        int iterationCount = 0;
                        totalWatts = 0.0;
                        countWattUpReading = 0;
                                         

                        while (true)//keep doing until the wattsup readings is greater than some threshole
                        {

                            bool t;
                            //Generating based population
                            for (int p = 0; p < basePopulationSize; p++)
                            {
                                t = ds.Create(testString + p);
                            }


                            //start the clock
                            stopwatch.Restart();

                            //Conducting the operations
                            AllOps(ds, basePopulationSize, opLength, testString, C, R, U, D);

                            //stop the clock
                            stopwatch.Stop();

                            //time in second
                            totaltime = totaltime + (stopwatch.Elapsed.TotalSeconds);
                            iterationCount++;

                            //clear all elements
                            ds.Clear();

                            //check if its time to stop
                            if (countWattUpReading >= 5)
                            {
                                break;
                            }
                        }

                        double averageTime = totaltime / iterationCount;
                        double averageWatts = totalWatts / (countWattUpReading - 3);//the first two are ignore because of the delay

                        string result = count + "," + c5DSs[j].ToString() + "," + d[0] + "," + d[1] + "," + d[2] + "," + d[3] + "," + d[4] + " #Iteration:" + iterationCount + " Avg Time(Sec):" + averageTime.ToString() + " Avg Power(Watts): " + averageWatts.ToString() + ", Energy (J):" + (averageTime * averageWatts).ToString();
                        Console.WriteLine("Result:" + result);
                        string savedToFileResult = count + "," + c5DSs[j].ToString() + "," + d[0] + "," + d[1] + "," + d[2] + "," + d[3] + "," + d[4] + "," + averageTime.ToString() + "," + averageWatts.ToString() + "," + (averageTime * averageWatts).ToString();
                        CsvfileWriter.WriteLine(savedToFileResult);
                        
                        //reset the count to zero
                        totalWatts = 0.0;
                        countWattUpReading = 0;
                        totaltime = 0.0;
                        count++;

                        //}


                    }
                }//end for each x-value set

                CsvfileWriter.Close();
            }

            
        }

        private static void ProfileEnergyEnergyRandomProgram(Stopwatch stopwatch, ref CBC.CrudBasedCollection<string> ds, C5DataStructure[] c5DSs, int opLength, String testString, ref int count)
        {
            //Run three Runs
            for (int run = 0; run < 3; run++)
                //for each file fo 100 files
                for (int i = 0; i < 100; i++)
                {
                    string applicationRawXValuesFileName = string.Format(@"RandomPrograms\RawXValuesOfRandomApplication{0}.csv", i);
                    //read file
                    //1. Read data from a Dataset
                    List<string[]> rawDatasetArrays = Helpers.parseCSV(applicationRawXValuesFileName);

                    //for writing result
                    string writeFileName = string.Format(@"RandomPrograms\Run I\Result_RawXValuesOfRandomApplication{0}.csv", i);
                    if (run == 1)
                        writeFileName = string.Format(@"RandomPrograms\Run II\Result_RawXValuesOfRandomApplication{0}.csv", i);
                    else if (run == 2)
                        writeFileName = string.Format(@"RandomPrograms\Run III\Result_RawXValuesOfRandomApplication{0}.csv", i);
                    //Delete if exist
                    if (File.Exists(writeFileName))
                        File.Delete(writeFileName);
                    //Create
                    StreamWriter CsvfileWriter = new StreamWriter(writeFileName, true);

                    Console.WriteLine("Start profiling progrm " + applicationRawXValuesFileName + " ...");

                    foreach (var d in rawDatasetArrays)
                    {

                        //for each C5 data structure
                        for (int j = 0; j < c5DSs.Length; j++)
                        {

                            Console.WriteLine("Start " + c5DSs[j].ToString() + " with start element size = " + d[0]);


                            //for each CRUD op proportion
                            //for (int k = 0; k < crudDatasetOps.GetLength(0); k++)
                            //{
                            ds = new CBC.CrudBasedCollection<string>(c5DSs[j]);
                            //calculate number of CRUD operations
                            int C = (int.Parse(d[1]) * opLength) / 100;
                            int R = (int.Parse(d[2]) * opLength) / 100;
                            int U = (int.Parse(d[3]) * opLength) / 100;
                            int D = (int.Parse(d[4]) * opLength) / 100;
                            int basePopulationSize = int.Parse(d[0]);
                            double totaltime = 0.0;
                            int iterationCount = 0;
                            totalWatts = 0.0;
                            countWattUpReading = 0;


                            while (true)//keep doing until the wattsup readings is greater than some threshole
                            {

                                bool t;
                                //Generating based population
                                for (int p = 0; p < basePopulationSize; p++)
                                {
                                    t = ds.Create(testString + p);
                                }


                                //start the clock
                                stopwatch.Restart();

                                //Conducting the operations
                                AllOps(ds, basePopulationSize, opLength, testString, C, R, U, D);

                                //stop the clock
                                stopwatch.Stop();

                                //time in second
                                totaltime = totaltime + (stopwatch.Elapsed.TotalSeconds);
                                iterationCount++;

                                //clear all elements
                                ds.Clear();

                                //check if its time to stop
                                if (countWattUpReading >= 5)
                                {
                                    break;
                                }
                            }

                            double averageTime = totaltime / iterationCount;
                            double averageWatts = totalWatts / (countWattUpReading - 3);//the first two are ignore because of the delay

                            string result = count + "," + c5DSs[j].ToString() + "," + d[0] + "," + d[1] + "," + d[2] + "," + d[3] + "," + d[4] + " #Iteration:" + iterationCount + " Avg Time(Sec):" + averageTime.ToString() + " Avg Power(Watts): " + averageWatts.ToString() + ", Energy (J):" + (averageTime * averageWatts).ToString();
                            Console.WriteLine("Result:" + result);
                            string savedToFileResult = count + "," + c5DSs[j].ToString() + "," + d[0] + "," + d[1] + "," + d[2] + "," + d[3] + "," + d[4] + "," + averageTime.ToString() + "," + averageWatts.ToString() + "," + (averageTime * averageWatts).ToString();
                            CsvfileWriter.WriteLine(savedToFileResult);

                            //reset the count to zero
                            totalWatts = 0.0;
                            countWattUpReading = 0;
                            totaltime = 0.0;
                            count++;

                            //}


                        }
                    }//end for each x-value set

                    CsvfileWriter.Close();
                }


        }
        
        static async void StartWattsUpAsynch()
        {            
            // This method runs asynchronously.
            bool t = await Task.Run(() => _MyWattsUp.Start(out err));
            
        }
        private static void myWattsUp_OnWuReading(object sender, WuReadingEventArgs e)
        {
            watts = e.wuData.Watts;
            //ignore the first two
            if (countWattUpReading > 2)
            {
                totalWatts = totalWatts + watts;                
            }
            countWattUpReading = countWattUpReading + 1;

            timeWattRead = e.wuData.Time;  
            
            Console.WriteLine(countWattUpReading +": Watts Read:" + watts.ToString("0.######") + " Time Stamp:" + timeWattRead.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

            if (basePowerFlag == 0 && watts < 50.0)//55 is a threshold
            {
                totalBasePower = totalBasePower + watts;
                basedPowerReadingCount++;
            }

        }

        /// <summary>
        /// Most of the CRUD operations here is  trying to use upperbound cost operations
        /// Create Last -- add last operation is a default operation restricted by the ICollection interface. Actually, adding/inserting at the first position is considered the highest cost among all add operations
        /// Retrieve Last -- retrieving last item is considered the highest cost among all find operations
        /// Update last -- updating the last item in a collection is considered the highest cost among all update operations
        /// Delete first -- deleting the first item in a collection is considered the highest cost among all delete operations
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="basePopulationSize"></param>
        /// <param name="opLength"></param>
        public static void AllOps(CBC.CrudBasedCollection<string> ds, int basePopulationSize, int opLength, string testString, int cSize, int rSize, int uSize, int dSize)
        {
            string s;
            bool t;
           

            //C add cSize times
            for (int i = basePopulationSize; i < basePopulationSize + cSize; i++)
            {
                t = ds.Create(testString + i);
            }

            //R retrieve rSize times
            for (int i = basePopulationSize; i > basePopulationSize - rSize; i--)
            {
                s = ds.Retrieve(testString + i);
            }

            //U update uSize times
            for (int i = basePopulationSize; i > basePopulationSize - uSize; i--)
            {
                t = ds.Update(testString + i);
            }

            //D delete dSize times           
            for (int i = basePopulationSize; i > basePopulationSize - dSize; i--)
            {
                t = ds.Delete(testString + i);
            }
        }

        /// <summary>
        /// Returns all numbers, between min and max inclusive, once in a random sequence.
        /// </summary>
        public static IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
        {
            List<int> candidates = new List<int>();
            for (int i = minInclusive; i <= maxInclusive; i++)
            {
                candidates.Add(i);
            }
            Random rnd = new Random();
            while (candidates.Count > 0)
            {
                int index = rnd.Next(candidates.Count);
                yield return candidates[index];
                candidates.RemoveAt(index);
            }
        }
                
    }
}
