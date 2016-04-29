using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace IntelPowerGadgetFramework
{
    public class IntelPowerGadget
    {
        ///A .Net wrapper class for the Intel Power Gadget library and driver
        ///By Junya Michanan
        ///Date: 1/15/2016
        ///Version: 1.0
        ///Computer Science, University of Denver, Denver, Colorado
        ///Term of Use: The code is provided as is and is availbale or use freely. 
        ///We take no responsiblility for anything caused by the code.
        ///
        ///How to use:
        ///==========================================================
        ///1. Add EnergyLib32.dll or EnergyLib64.dll to the bin folder 
        ///2. Attatch IntelPowerGdgetFramwork reference
        ///3. Attach the PowerReading event to your code
        ///     IntelPowerGadget.PowerDataReading += IntelPowerGadget_PowerDataReading;
        ///2. Start and stop methods are to be called when start and stop the Intel Power Gadget in your code
        ///     IntelPowerGadget.Start();
        ///     IntelPowerGadget.Stop();
        ///3. Read the data using the attached event. The power data is passed via "IntelPowerGadget.PowerDataEventArgs" object 
        ///4. The sample rate can be set in the TimeInterval property (value is in millisecond):
        ///     IntelPowerGadget.TimeInterval = 500;// the optimal value is 100 and the default value is 1000.

        //Read more instructions and download the library:
        //https://software.intel.com/en-us/blogs/2014/01/07/using-the-intel-power-gadget-30-api-on-windows
        
        #region Intel Power Gadget APIs Access

        /// <summary>
        /// Initializes the library and connects to the driver.
        /// </summary>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll")]
        private static extern bool IntelEnergyLibInitialize();

        /// <summary>
        /// Reads sample data from the driver for all the supported MSRs.
        /// </summary>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll")]
        private static extern bool ReadSample();

        /// <summary>
        /// Returns the number of CPU packages on the system.
        /// </summary>
        /// <param name="nNodes"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetNumNodes(ref int nNodes);

        /// <summary>
        /// Returns the number of supported MSRs for bulk reading and logging.
        /// </summary>
        /// <param name="nMsrs"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetNumMsrs(ref int nMsrs);

        /// <summary>
        /// Returns in szName the name of the MSR specified by iMsr
        /// </summary>
        /// <param name="iMsr"></param>
        /// <param name="szName"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetMsrName(int iMsr, StringBuilder szName);
        private static bool GetMSRName(int iMSR, out string MSRName)
        {
            int bufferSize = 512;
            StringBuilder buffer = new StringBuilder(bufferSize);
            bool success = GetMsrName(iMSR, buffer);
            MSRName = buffer.ToString();            
            return success;
        }
        /// <summary>
        /// Returns the data collected by the most recent call to ReadSample(). 
        /// The returned data is for the data on the package specified by iNode, from the MSR specified by iMSR. 
        /// The data is returned in pResult, and the number of double results returned in pResult is returned in nResult
        /// </summary>
        /// <param name="iNode"></param>
        /// <param name="iMSR"></param>
        /// <param name="pResult"></param>
        /// <param name="nResult"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetPowerData(int iNode, int iMSR, [In, Out] double[] pResult, out int nResult);
        private static bool GetPowerData(int iNode, int iMSR, out double[] dResult, out int nResult)
        {
            bool success = false;
            double[] result = new double[4];
            success = GetPowerData(0, iMSR, result, out nResult);
            dResult = result;
            return success;
        }

        /// <summary>
        /// Returns true if Intel® graphics is available and false if Intel® graphics is unavailable (i.e if it’s not present or disabled).
        /// </summary>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool IsGTAvailable();

        /// <summary>
        /// Returns the current GT frequency in MHz. The data is returned in freq.
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetGTFrequency(ref int freq);

        
        /// <summary>
        /// Returns in pOffset the time (in seconds) that has elapsed between the two most recent calls to ReadSample().
        /// </summary>
        /// <param name="pOffset"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetTimeInterval(ref double pOffset);

        /// <summary>
        /// Reads the processor frequency MSR on the package specified by iNode, and returns the frequency (in MHz) in freqInMHz.
        /// </summary>
        /// <param name="iNode"></param>
        /// <param name="freqInMHz"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetIAFrequency(int iNode, ref int freqInMHz);

        /// <summary>
        /// Reads the package power info MSR on the package specified by iNode, and returns the TDP (in Watts) in TDP. 
        /// It is recommended that Package Power Limit is used instead of TDP whenever possible, as it is a more accurate upper bound to the package power than TDP.
        /// </summary>
        /// <param name="iNode"></param>
        /// <param name="TDP"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetTDP(int iNode, ref double TDP);

        /// <summary>
        /// Reads the temperature target MSR on the package specified by iNode, and returns the maximum temperature (in degrees Celsius) in degreeC.
        /// </summary>
        /// <param name="iNode"></param>
        /// <param name="degreeC"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetMaxTemperature(int iNode, ref int degreeC);

        /// <summary>
        /// Reads the temperature MSR on the package specified by iNode, and returns the current temperature (in degrees) Celsius in degreeC.
        /// </summary>
        /// <param name="iNode"></param>
        /// <param name="degreeC"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetTemperature(int iNode, ref int degreeC);

        /// <summary>
        /// Returns in pBaseFrequency the advertised processor frequency for the package specified by iNode.
        /// </summary>
        /// <param name="iNode"></param>
        /// <param name="pBaseFrequency"></param>
        /// <returns></returns>
        [DllImport("EnergyLib32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetBaseFrequency(int iNode, ref double pBaseFrequency);

        #endregion
               
        #region Timer Events
        private static Timer _timer;
        private static int nNodes = -1;
        private static int iMSR = 1; //MSR is 1 for power data
        private static int nResult = 0;
        private static double[] dResult = new double[4];
        private static double timeInterval = 0;

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //read power data
            bool readsample = IntelPowerGadget.ReadSample();
            bool readSuccess = IntelPowerGadget.GetPowerData(nNodes - 1, iMSR, out dResult, out nResult);
            bool iSuccess = IntelPowerGadget.GetTimeInterval(ref timeInterval);

            PowerDataReading(new PowerDataEventArgs { Power = dResult[0], TimeInterval = timeInterval });
                       
            
        }

        #endregion

        #region Public Methods, Properties and Events
        
        [System.ComponentModel.DefaultValue(1000)]
        public static int TimeInterval { get; set; }
        public static void Start()
        {
            _timer = new Timer(TimeInterval);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true; // Enable it
            bool initSuccess = IntelEnergyLibInitialize();
            if (initSuccess)
            {
                bool nNodeSuccess = IntelPowerGadget.GetNumNodes(ref nNodes);
                if (nNodes > 0)
                {
                   _timer.Start();
                }
                else { throw new System.Exception("No node found!"); }
            }
            else { throw new System.Exception("The Intel Power Gadget can not be initialized."); }
        }

        public static void Stop()
        {
            _timer.Stop();
        }

        public delegate void PowerDataReadingEventHandler(PowerDataEventArgs e);
        public static event PowerDataReadingEventHandler PowerDataReading;

        public class PowerDataEventArgs : EventArgs
        {
            public double Power { get; set; }
            public double TimeInterval { get; set; }
        }

        #endregion

        #region All Available APIs
        //To use this comment, change the APIs accessability to public
        /*
        int sampleRate = 1000; //rate in milliseconds

        int nNodes = -1;
        int nMSRs = -1;
        int maxTemp = -1;
        double timeInterval = 0;

        string nameMSR = "no name";

        bool initialize = IntelPowerGadget.IntelEnergyLibInitialize();

        bool numNodes = IntelPowerGadget.GetNumNodes(ref nNodes);            

        bool numMSRs = IntelPowerGadget.GetNumMsrs(ref nMSRs);


        for (int i = 0; i < 1000; i++)
        {
            if (nNodes > 0)
            {
                bool readsample = IntelPowerGadget.ReadSample();

                Console.WriteLine("Sample Rate: {0} milliseconds", sampleRate);
                Console.WriteLine("Num Node: {0}", nNodes);
                Console.WriteLine("Num MSR: {0}", nMSRs);

                bool iSuccess = IntelPowerGadget.GetTimeInterval(ref timeInterval);
                Console.WriteLine("Time Interval: {0} seconds", timeInterval);

                bool nameMSRs = IntelPowerGadget.GetMSRName(nNodes - 1, out nameMSR);
                Console.WriteLine("MSR Name: {0}", nameMSR);

                int nResult = 0;
                double[] dResult = new double[4];

                int iMSR = 0;
                bool pIsSuccess = IntelPowerGadget.GetPowerData(nNodes - 1, iMSR, out dResult, out nResult);//MSR 0, 1 double, Frequency
                Console.WriteLine("MSR: {0}", iMSR);
                Console.WriteLine("Number Results: {0}", nResult);
                Console.WriteLine("Fequency: {0} MHz", dResult[0]);

                iMSR = 1;
                pIsSuccess = IntelPowerGadget.GetPowerData(nNodes - 1, iMSR, out dResult, out nResult);//MSR 1, 3 doubles, Average Powers(watts), Cumerlative Energy (in Joules), Cumerlative Energy (in Milliwatts hours)
                Console.WriteLine("MSR: {0}", iMSR);
                Console.WriteLine("Number Results: {0}", nResult);
                Console.WriteLine("Avg Power: {0} watts", dResult[0]);
                Console.WriteLine("Cumulative Energy: {0} joules", dResult[1]);
                Console.WriteLine("Cumulative Energy: {0} milliwatts hours", dResult[2]);

                iMSR = 2;
                pIsSuccess = IntelPowerGadget.GetPowerData(nNodes - 1, iMSR, out dResult, out nResult);
                Console.WriteLine("MSR: {0}", iMSR);
                Console.WriteLine("Number Results: {0}", nResult);
                Console.WriteLine("Data 1: {0}", dResult[0]);
                Console.WriteLine("Data 2: {0}", dResult[1]);
                Console.WriteLine("Data 3: {0}", dResult[2]);
                Console.WriteLine("Data 4: {0}", dResult[3]);

                iMSR = 3;
                pIsSuccess = IntelPowerGadget.GetPowerData(nNodes - 1, iMSR, out dResult, out nResult);//MSR 3, 2 doubles, Temperature (in C), Proc Hot (Proc Hot (‘0’ if false and ‘1’ if true))
                Console.WriteLine("MSR: {0}", iMSR);
                Console.WriteLine("Number Results: {0}", nResult);
                Console.WriteLine("Temperature: {0} Degree C", dResult[0]);
                Console.WriteLine("PROC HOT: {0} (0 = false, 1 = true)", dResult[1]);

                iMSR = 4;
                pIsSuccess = IntelPowerGadget.GetPowerData(nNodes - 1, iMSR, out dResult, out nResult);
                Console.WriteLine("MSR: {0}", iMSR);
                Console.WriteLine("Number Results: {0}", nResult);
                Console.WriteLine("Data 1: {0}", dResult[0]);
                Console.WriteLine("Data 2: {0}", dResult[1]);
                Console.WriteLine("Data 3: {0}", dResult[2]);
                Console.WriteLine("Data 4: {0}", dResult[3]);

                iMSR = 5;
                pIsSuccess = IntelPowerGadget.GetPowerData(nNodes - 1, iMSR, out dResult, out nResult);//MSR 5, 1 double, Package Power Limit (in Watts)  
                Console.WriteLine("MSR: {0}", iMSR);
                Console.WriteLine("Number Results: {0}", nResult);
                Console.WriteLine("Package Power Limit: {0} Watts", dResult[0]);

                pIsSuccess = IntelPowerGadget.GetMaxTemperature(nNodes - 1, ref maxTemp);
                Console.WriteLine("Max Temperature: {0} Degree C", maxTemp);

            }

            Thread.Sleep(sampleRate);

            System.Console.Clear();
        }
        */
        #endregion
    }
}
