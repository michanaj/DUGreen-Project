using GraphLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntelPowerGadgetFramework;
using System.Threading;
using WattsUpFramework;
using System.Diagnostics;

namespace GreenC5Simulator
{
    public partial class SimulatorWindow : Form
    {
        ///Developed by: Junya Michanan
        ///Computer Science Department, University of Denver, Denver, Colorado USA 80210
        ///The graph library is developed by DI Zimmermann Stephan (Thank you)

        #region Variables
        private int DataSourceLenght = 3600;//36 samples or about 6 minutes of data, each is 100-millisecond apart
        private int DataDispalyLenght = 1000;//show 1000 samples on the graph dispaly area
        private PrecisionTimer.Timer intelGadgetTimer = null;
        private PrecisionTimer.Timer wattsUpTimer = null;
        //private DateTime lastTimerTick = DateTime.Now;
        private int sampleCount = 0;

        private int intelPowerGadgetRePaintTick = 100; //millisconds
        private int intelPowerGadgetDSIndex = 0; //data source index
        private int wattsupRePaintTick = 1000; //millisconds
        private int wattsupDSIndex = 1; //data source index

        private float intelWattReadingValue = 0;
        private bool isintelPowerGadgetGraphMoving = false;

        //WattsUp 
        private WattsUp _MyWattsUp = new WattsUp();
        private float wattsupReadingValue = 0;
        private bool isWattsUpGraphMoving = false;
        string err = "";

        Stopwatch stopwatch = new Stopwatch();


        bool isStartMode = true;
        bool startEnergyProfile = false;
        double programTotalWatts = 0;
        double programSampleCount = 0;
        double programAvgWatt = 0;
        double programTotalTimes = 0;
        double programTotalEnergy = 0;
        double productivity = 0;

        //GreenC5 default properties
        //default decision making criteria
        int k1 = 5; //transform if 2 predictions have been made or 20K CRUD operations have been performed
        double k2 = 60; //transform if the prediction probability is greater than 20%
                
        #endregion

        #region Constructor

        public SimulatorWindow()
        {
            InitializeComponent();
            //start the Intel Power Gadget
            IntelPowerGadget.TimeInterval = 100;//set sample rate per milliseconds
            IntelPowerGadget.PowerDataReading += IntelPowerGadget_PowerDataReading;
            StartIntelPowerGadgetAsynch();

            display.Smoothing = System.Drawing.Drawing2D.SmoothingMode.None;
            display.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_HOR;
            //display.SetPlayPanelInvisible();


            intelGadgetTimer = new PrecisionTimer.Timer();
            intelGadgetTimer.Period = intelPowerGadgetRePaintTick;
            intelGadgetTimer.Tick += new EventHandler(OnIntelGadgetPaintGraphTimerTick);
            //lastTimerTick = DateTime.Now;
            intelGadgetTimer.Start();

            wattsUpTimer = new PrecisionTimer.Timer();
            wattsUpTimer.Period = wattsupRePaintTick;
            wattsUpTimer.Tick += new EventHandler(OnWattsUpPaintGraphTimerTick);
            wattsUpTimer.Start();

            _MyWattsUp.InternalSamplingRate = 1;
            _MyWattsUp.SamplingRate = 1;
            _MyWattsUp.IsUseWattsDelta = true;
            _MyWattsUp.OnWuReading += new WuReadingEventHandler(myWattsUp_OnWuReading);
            StartWattsUpAsynch();

            CalcDataGraphs();
            display.Refresh();

            //set default value for decision making criteria
            cbK1.SelectedItem = k1.ToString();
            cbK2.SelectedItem = k2.ToString();
        }

        #endregion

        #region Intel Power Gadget
        static async void StartIntelPowerGadgetAsynch()
        {
            // This method runs asynchronously.
            await Task.Run(() => IntelPowerGadget.Start());

        }
        private void IntelPowerGadget_PowerDataReading(IntelPowerGadget.PowerDataEventArgs e)
        {
            intelWattReadingValue = (float) e.Power;            
            sampleCount++;
            //record the power data of the Intel Power gadget
            if (!isintelPowerGadgetGraphMoving)
            {
                display.DataSources[intelPowerGadgetDSIndex].Samples[sampleCount - 1].x = sampleCount - 1;
                display.DataSources[intelPowerGadgetDSIndex].Samples[sampleCount - 1].y = intelWattReadingValue;
            }
            else
            {
                //shift over the data
                cPoint[] src = display.DataSources[intelPowerGadgetDSIndex].Samples;
                for (int i = 0; i < src.Length - 1; i++)
                {
                    src[i].y = src[i + 1].y;
                }
                //set the last one                    
                src[src.Length - 1].y = intelWattReadingValue;
            }

            //record the WattsUp data to the datasource at the same rate
            if (!isintelPowerGadgetGraphMoving)
            {
                display.DataSources[wattsupDSIndex].Samples[sampleCount-1].x = sampleCount-1;
                display.DataSources[wattsupDSIndex].Samples[sampleCount-1].y = wattsupReadingValue;
            }
            else
            {
                //shift over the data
                cPoint[] src = display.DataSources[wattsupDSIndex].Samples;
                for (int i = 0; i < src.Length - 1; i++)
                {
                    src[i].y = src[i + 1].y;
                }
                //set the last one                    
                src[src.Length - 1].y = wattsupReadingValue;
            }

            if (sampleCount == DataSourceLenght)
            {
                sampleCount = 0;
                isintelPowerGadgetGraphMoving = true;                
            }
        }
        #endregion

        #region WattsUp 

        async void StartWattsUpAsynch()
        {
            // This method runs asynchronously.
            bool t = await Task.Run(() => _MyWattsUp.Start(out err));

        }
        private void myWattsUp_OnWuReading(object sender, WuReadingEventArgs e)
        {
            wattsupReadingValue = (float)e.wuData.Watts;
        }
        #endregion

        #region Graphs
        protected override void OnClosed(EventArgs e)
        {
            IntelPowerGadget.Stop();
            _MyWattsUp.Stop();
            intelGadgetTimer.Stop();
            intelGadgetTimer.Dispose();
            wattsUpTimer.Stop();
            wattsUpTimer.Dispose();
            
            base.OnClosed(e);
        }
        private void OnIntelGadgetPaintGraphTimerTick(object sender, EventArgs e)
        {            
            try
            {
                if (startEnergyProfile)
                {
                    //profile energy data
                    programSampleCount++;
                    programTotalWatts += intelWattReadingValue;
                    double avgWatts = programTotalWatts / programSampleCount;
                    double totalTime = stopwatch.Elapsed.TotalSeconds;
                    programTotalEnergy = (avgWatts * totalTime); 
                    this.Invoke((MethodInvoker)(() => lbAvgPower.Text = string.Format("Avg. Watts: {0:0.0000} Watts", avgWatts)));
                    this.Invoke((MethodInvoker)(() => lbTotalTime.Text = string.Format("Total Runtime: {0:0.0000} Seconds", totalTime)));
                    this.Invoke((MethodInvoker)(() => lbEnergy.Text = string.Format("Energy (J): {0:0.0000} Joules", programTotalEnergy)));
                    this.Invoke((MethodInvoker)(() => lbWattsHour.Text = string.Format("Energy (WH): {0:0.0000} Watt-Hour", programTotalEnergy/3600)));
                }


                //update the power reading every second
                this.Invoke((MethodInvoker)(() => lbIntelPowerReading.Text = string.Format("{0:0.00}W", intelWattReadingValue)));
                this.Invoke(new MethodInvoker(RefreshGraph));
            }
            catch (ObjectDisposedException ex)
            {
                // we get this on closing of form
            }
            catch (Exception ex)
            {
                Console.Write("exception invoking refreshgraph(): " + ex.Message);
            }

        }
        private void OnWattsUpPaintGraphTimerTick(object sender, EventArgs e)
        {
            try
            {
                //update the Normalized Correlation value every second
                this.Invoke((MethodInvoker)(() => lbNormCorrelation.Text = string.Format("{0:0.00}", CalculateNormalizedCorrelation())));

                //update the power reading every second
                this.Invoke((MethodInvoker)(()=> lbWattsUpPowerReading.Text = string.Format("{0:0.00}W", wattsupReadingValue)));
                this.Invoke(new MethodInvoker(RefreshGraph));
            }
            catch (ObjectDisposedException ex)
            {
                // we get this on closing of form
            }
            catch (Exception ex)
            {
                Console.Write("exception invoking refreshgraph(): " + ex.Message);
            }

        }
        private void RefreshGraph()
        {
            display.Refresh();
        }
        private void ApplyColorSchema()
        {            
            
            display.DataSources[intelPowerGadgetDSIndex].GraphColor = Color.DarkRed;
            display.DataSources[wattsupDSIndex].GraphColor = Color.DarkBlue;

            display.BackgroundColorTop = Color.White;
            display.BackgroundColorBot = Color.White;
            display.SolidGridColor = Color.LightGray;
            display.DashedGridColor = Color.LightGray;                    

        }
        protected void CalcDataGraphs()
        {

            this.SuspendLayout();

            display.DataSources.Clear();
            display.SetDisplayRangeX(0, DataDispalyLenght);
            //Stacked graph type
            display.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_HOR;                        
            DataSource intelGadget = new DataSource();
            intelGadget.Name = "Intel Power Gadget (CPU)";
            intelGadget.OnRenderXAxisLabel += RenderXLabel;
            intelGadget.Length = DataSourceLenght;
            intelGadget.AutoScaleY = false;
            intelGadget.AutoScaleX = false;
            intelGadget.SetDisplayRangeY(-2, 32);
            intelGadget.SetGridDistanceY(2);
            //all sample to zeroes
            display.DataSources.Add(intelGadget);

            DataSource wattsUp = new DataSource();
            wattsUp.Name = "Watts Up? Pro (System)";
            wattsUp.OnRenderXAxisLabel += RenderXLabel;
            wattsUp.Length = DataSourceLenght;
            wattsUp.AutoScaleY = false;
            wattsUp.AutoScaleX = false;
            wattsUp.SetDisplayRangeY(-2, 32);
            wattsUp.SetGridDistanceY(2);
            display.DataSources.Add(wattsUp);
            
            ApplyColorSchema();

            this.ResumeLayout();
            display.Refresh();

        }
        private String RenderXLabel(DataSource s, int idx)
        {
            if (s.AutoScaleX)
            {
                //if (idx % 2 == 0)
                {
                    int Value = (int)(s.Samples[idx].x);
                    return "";// + Value;
                }
                return "";
            }
            else
            {
                int Value = (int)(s.Samples[idx].x);
                String Label = "" + Value + "";
                return "";// Label;
            }
            //return "";
        }
        private String RenderYLabel(DataSource s, float value)
        {
            return String.Format("{0:0.0}", value);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            display.Dispose();

            base.OnClosing(e);
        }

        private float CalculateNormalizedCorrelation()
        {
            float norm_corr = 0;
            float sum_xy = 0;
            float sum_x2 = 0;
            float sum_y2 = 0;

            //get points from Intel Power Gadget
            cPoint[] intelSrc = display.DataSources[intelPowerGadgetDSIndex].Samples;
            cPoint[] wattUpSrc = display.DataSources[wattsupDSIndex].Samples;
            for (int i = 0; i < intelSrc.Length - 1; i++)
            {
                sum_xy += intelSrc[i].y * wattUpSrc[i].y;
                sum_x2 += intelSrc[i].y * intelSrc[i].y;
                sum_y2 += wattUpSrc[i].y * wattUpSrc[i].y;
            }

            norm_corr = sum_xy / (float) Math.Sqrt((double)(sum_x2 * sum_y2));

            return norm_corr;
        }
        #endregion

        #region User Methods
        int threadCount = 0;
        int allExecutionCount = 0;
        Dictionary<string, bool> threadStatuses = new Dictionary<string, bool>();
        private void butAddNewThread_Click(object sender, EventArgs e)
        {
            ThreadUserControl thread = new ThreadUserControl("Thread #" + (threadCount + 1), k1, k2);
            thread.PropertyChanged += Thread_PropertyChanged;
            thread.SetInstanceInfiniteMode(cbInfiniteExecution.Checked);
            programExecutionPanel.Controls.Add(thread);
            threadStatuses.Add(thread.ThreadID, false);
            threadCount++;
            lbThreadCount.Text = string.Format("Thread Count: {0}", threadCount);
            butStart.Visible = true;
        }

        private void AddNewThread(int numThread, int numInstance)
        {
            for (int i = 0; i < numThread; i++)
            {
                ThreadUserControl thread = new ThreadUserControl("Thread #" + (threadCount + 1), k1, k2, true);
                thread.PropertyChanged += Thread_PropertyChanged;
                thread.SetInstanceInfiniteMode(cbInfiniteExecution.Checked);
                programExecutionPanel.Controls.Add(thread);
                threadStatuses.Add(thread.ThreadID, false);
                threadCount++;
                lbThreadCount.Text = string.Format("Thread Count: {0}", threadCount);                
                thread.AddGreenC5Instance(numInstance);
            }
        }
        private bool allCompleted = false;
        private void Thread_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Status
            if (e.PropertyName == "Status" && (sender as ThreadUserControl).Status =="Idle")
            {
                threadStatuses[(sender as ThreadUserControl).ThreadID] = true;
                //MessageBox.Show((sender as ThreadUserControl).ThreadID + "Completed");
                //Check if every thread is completed
                allCompleted = true;
                foreach (bool status in threadStatuses.Values)
                {
                    allCompleted = allCompleted & status;
                }

                if (allCompleted)
                {
                    stopwatch.Stop();
                    //set all the statuses and buttons to the originals
                    this.Invoke((MethodInvoker)(() => butStart.Text = "Start"));
                    isStartMode = true;
                    startEnergyProfile = false;
                    //this.Invoke((MethodInvoker)(() => butAddNewThread.Enabled = true));
                    this.Invoke((MethodInvoker)(() => butClear.Enabled = true));
                    this.Invoke((MethodInvoker)(() => cbInfiniteExecution.Enabled = true));
                    
                    

                }
            }

            if (e.PropertyName == "TotalExecutionCount")
            {
                if (!isStartMode)
                {
                    allExecutionCount = 0;
                    foreach (var thread in programExecutionPanel.Controls)
                    {
                        if (thread.GetType() == typeof(ThreadUserControl))
                        {
                            allExecutionCount += (thread as ThreadUserControl).TotalExecutionCount;
                        }
                    }

                    if (programTotalEnergy > 0)
                        productivity = allExecutionCount / programTotalEnergy;

                    this.Invoke((MethodInvoker)(() => lbTotalExecution.Text = string.Format("Total Executions: {0} ", allExecutionCount)));
                    this.Invoke((MethodInvoker)(() => lbExecJoule.Text = string.Format("Productivity: {0:0.0000} Executions/Joule", productivity)));
                }
            }
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            foreach (var thread in programExecutionPanel.Controls)
            {
                if (thread.GetType() == typeof(ThreadUserControl))
                {
                    (thread as ThreadUserControl).Dispose();
                }
            }
            programExecutionPanel.Controls.Clear();
            threadStatuses.Clear();
            butAddNewThread.Enabled = true;
            threadCount = 0;
            allExecutionCount = 0;
            lbTotalExecution.Text = "Total Executions: " + allExecutionCount;
            lbThreadCount.Text = string.Format("Thread Count: {0}", threadCount);
            butStart.Visible = false;
        }
        private void butStart_Click(object sender, EventArgs e)
        {
            Start();

        }

        private void Start()
        {

            if (isStartMode)//start mode
            {
                startEnergyProfile = true;
                butStart.Text = "Stop";
                isStartMode = false;
                butAddNewThread.Enabled = false;
                butClear.Enabled = false;
                cbInfiniteExecution.Enabled = false;

                //for simplicity, disable all adding new GreenC5 instance and thread
                butAddNewThread.Enabled = false;
                foreach (var thread in programExecutionPanel.Controls)
                {
                    if (thread.GetType() == typeof(ThreadUserControl))
                    {
                        (thread as ThreadUserControl).DisableAddNewGreenC5();
                        (thread as ThreadUserControl).TotalExecutionCount = 0;
                    }
                }

                //reset everything to zeros
                programTotalWatts = 0;
                programSampleCount = 0;
                programAvgWatt = 0;
                programTotalTimes = 0;
                programTotalEnergy = 0;
                productivity = 0;

                stopwatch.Restart();
                foreach (var thread in programExecutionPanel.Controls)
                {
                    if (thread.GetType() == typeof(ThreadUserControl))
                    {
                        Task.Factory.StartNew(() => (thread as ThreadUserControl).Start());
                    }
                }

            }
            else//stop mode
            {
                foreach (var thread in programExecutionPanel.Controls)
                {
                    if (thread.GetType() == typeof(ThreadUserControl))
                    {
                        (thread as ThreadUserControl).Stop();
                    }
                }
                stopwatch.Stop();
                startEnergyProfile = false;
                butStart.Text = "Start";
                isStartMode = true;
                //butAddNewThread.Enabled = true;
                butClear.Enabled = true;
                cbInfiniteExecution.Enabled = true;

                //check the Total execution again
                allExecutionCount = 0;
                foreach (var thread in programExecutionPanel.Controls)
                {
                    if (thread.GetType() == typeof(ThreadUserControl))
                    {
                        allExecutionCount += (thread as ThreadUserControl).TotalExecutionCount;
                    }
                }

                if (programTotalEnergy > 0)
                    productivity = allExecutionCount / programTotalEnergy;

                this.Invoke((MethodInvoker)(() => lbTotalExecution.Text = string.Format("Total Executions: {0} ", allExecutionCount)));
                this.Invoke((MethodInvoker)(() => lbExecJoule.Text = string.Format("Productivity: {0:0.0000} Executions/Joules", productivity)));
            }
        }
                
        private void cbInfiniteExecution_CheckedChanged(object sender, EventArgs e)
        {
            
            bool isAllowed = true;
            if (cbInfiniteExecution.Checked == true)
            {
                //check if each thread has only one instance
                foreach (var thread in programExecutionPanel.Controls)
                {
                    if (thread.GetType() == typeof(ThreadUserControl))
                    {

                        if ((thread as ThreadUserControl).GreenC5Count > 1)
                            isAllowed = false;
                    }
                }
            }
            if (isAllowed)
            {
                if (cbInfiniteExecution.Checked)
                {
                    foreach (var thread in programExecutionPanel.Controls)
                    {
                        if (thread.GetType() == typeof(ThreadUserControl))
                        {
                            (thread as ThreadUserControl).SetInstanceInfiniteMode(true);
                        }
                    }
                }
                else
                {
                    foreach (var thread in programExecutionPanel.Controls)
                    {
                        if (thread.GetType() == typeof(ThreadUserControl))
                        {
                            (thread as ThreadUserControl).SetInstanceInfiniteMode(false);
                        }
                    }
                }
            }
            else
            {
                cbInfiniteExecution.Checked = false;
                //show error message
                MessageBox.Show("Only one instance of GreenC5 is allowed in each thread for Infinite Execution mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
                        
        private void cbK1_SelectedIndexChanged(object sender, EventArgs e)
        {
            k1 = int.Parse(cbK1.SelectedItem as string);
            foreach (var thread in programExecutionPanel.Controls)
            {
                if (thread.GetType() == typeof(ThreadUserControl))
                {
                    (thread as ThreadUserControl).SetDecisionMakingCriteria(k1, k2);
                }
            }
        }

        private void cbK2_SelectedIndexChanged(object sender, EventArgs e)
        {
            k2 = double.Parse(cbK2.SelectedItem as string);
            foreach (var thread in programExecutionPanel.Controls)
            {
                if (thread.GetType() == typeof(ThreadUserControl))
                {
                    (thread as ThreadUserControl).SetDecisionMakingCriteria(k1, k2);
                }
            }
        }

        //private void butCaseStudy3_Click(object sender, EventArgs e)
        //{
        //    isRunningCaseStudy = true;
        //    Clear();
        //    butCaseStudy3.Text = "Wait 5 sec...";
        //    Thread.Sleep(5000);
        //    butCaseStudy3.Text = "Running...";
        //    butCaseStudy3.Enabled = false;
        //    //List of simulated/real-world programs
        //    List<string> crudWorkloadPrograms = new List<string>();
        //    crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication1.csv");
        //    crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication2.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication3.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication4.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication5.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication6.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication7.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication8.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication9.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\RandomApplication10.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\AStarPathFinderProgram_Custom_Formula.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\AStarPathFinderProgram_DiagonalShortcut_Formula.csv");
        //    crudWorkloadPrograms.Add(@"CRUD Workloads\AStarPathFinderProgram_Euclidean_Formula.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\AStarPathFinderProgram_Manhatan_Formula.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\AStarPathFinderProgram_MaxDXDY_Formula.csv");
        //    crudWorkloadPrograms.Add(@"CRUD Workloads\GA-FittnessTableProgram.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\GA-NextGenerationProgram.csv");
        //    //crudWorkloadPrograms.Add(@"CRUD Workloads\GA-ThisGenerationProgram.csv");
        //    crudWorkloadPrograms.Add(@"CRUD Workloads\HuffmanCodingProgram.csv");

        //    int useCaseNumber = 1;

        //    RunSequentially(crudWorkloadPrograms, useCaseNumber);
        //    RunParallelly(crudWorkloadPrograms);
        //}

        
        //private void RunSequentially(List<string> crudWorkloadPrograms, int useCaseNumber)
        //{
        //    for(int useCase =0; useCase <useCaseNumber; useCase++)
        //    {
        //        AddNewThread(1, useCase + 1);
        //        Start();                
        //    }
            
        //}

        //private void RunParallelly(List<string> crudWorkloadPrograms)
        //{
        //    //throw new NotImplementedException();
        //}

        #endregion


    }
}
