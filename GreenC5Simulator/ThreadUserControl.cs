using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenC5Simulator
{
    public partial class ThreadUserControl : UserControl, INotifyPropertyChanged
    {
        public string ThreadID { get; set; }
        
        private string status;
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                if (value != this.status)
                {
                    this.status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        int totalNumExecution = 0;
        public int TotalExecutionCount
        {
            get
            {
                return this.totalNumExecution;
            }

            set
            {
                if (value != this.totalNumExecution)
                {
                    this.totalNumExecution = value;
                    OnPropertyChanged("TotalExecutionCount");
                }
            }
        }
        public string Message { get; set; }        
        public int GreenC5Count { get; set; } 
        public int DecisionMakingCriteriaK1 { get; set; }
        public double DecisionMakingCriteriaK2 { get; set; }       
        private bool isInfiniteMode = false;
        //decision making criteria
        public ThreadUserControl(string threadID, int decisionMakingCriteriaK1, double decisionMakingCriteriaK2)
        {
            InitializeComponent();
            GreenC5Count = 0;
            DecisionMakingCriteriaK1 = decisionMakingCriteriaK1;
            DecisionMakingCriteriaK2 = decisionMakingCriteriaK2;
            ThreadID = threadID;
            Status = "Idle";           
            lbThreadName.Text = "Thread ID: "+ ThreadID;
            lbStatus.Text = "Status: " + Status;
            //Default GreenC5
            GreenC5UserControl greenC5 = new GreenC5UserControl("GreenC5 #"+(GreenC5Count+1), isInfiniteMode, DecisionMakingCriteriaK1, DecisionMakingCriteriaK2);
            greenC5.PropertyChanged += GreenC5_PropertyChanged;
            greenC5FlowPanel.Controls.Add(greenC5);
            GreenC5Count++;

            lbCount.Text = "GreenC5 Instance Count: " + GreenC5Count;            

        }

        public ThreadUserControl(string threadID, int decisionMakingCriteriaK1, double decisionMakingCriteriaK2, bool automatedMode)
        {
            InitializeComponent();
            GreenC5Count = 0;
            DecisionMakingCriteriaK1 = decisionMakingCriteriaK1;
            DecisionMakingCriteriaK2 = decisionMakingCriteriaK2;
            ThreadID = threadID;
            Status = "Idle";
            lbThreadName.Text = "Thread ID: " + ThreadID;
            lbStatus.Text = "Status: " + Status;
            
        }

        private void GreenC5_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ExecutionCount")
            {
                int count = 0;
                foreach (var gc5 in greenC5FlowPanel.Controls)
                {
                    if (gc5.GetType() == typeof(GreenC5UserControl))
                    {                        
                          count +=  (gc5 as GreenC5UserControl).ExecutionCount;                        
                    }
                }

                TotalExecutionCount = count;
                this.Invoke((MethodInvoker)(() => lbTotalExecution.Text = "Total Completed Executions: " + TotalExecutionCount));

            }
        }
        private void butAddNewGreenC5_Click(object sender, EventArgs e)
        {
            bool allowed = true;

            if (isInfiniteMode)//this is just to prevent adding more than one instance per thread
                allowed = false;

            if (allowed)
            {
                AddGreenC5Instance(1);                
            }
            else
            {
                //show error message
                MessageBox.Show("Only one instance of GreenC5 is allowed in each thread for Infinite Execution mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        public void AddGreenC5Instance(int numInstance)
        {
            for (int i = 0; i < numInstance; i++)
            {
                GreenC5UserControl greenC5 = new GreenC5UserControl("GreenC5 #" + (GreenC5Count + 1), isInfiniteMode, DecisionMakingCriteriaK1, DecisionMakingCriteriaK2);
                greenC5.PropertyChanged += GreenC5_PropertyChanged;
                //greenC5.InternalDataStructure.SetDecisionMakingCriteria(DecisionMakingCriteriaK1, DecisionMakingCriteriaK2);
                greenC5FlowPanel.Controls.Add(greenC5);
                GreenC5Count++;
                lbCount.Text = "GreenC5 Instance Count: " + GreenC5Count;
            }
        }

        bool keepRunning = true;       

        public void Start()
        {
            keepRunning = true;
            Status = "Active";
            TotalExecutionCount = 0;
            this.Invoke((MethodInvoker)(() => lbStatus.Text = "Status: Running..."));
            foreach (var gc5 in greenC5FlowPanel.Controls)
            {
                if(gc5.GetType() == typeof(GreenC5UserControl))
                {
                    if (keepRunning)
                        (gc5 as GreenC5UserControl).Execute();
                    else
                        break;
                }
            }
            Status = "Idle";
            this.Invoke((MethodInvoker)(() => lbStatus.Text = "Status: Idle"));

        }

        public void Stop()
        {
            keepRunning = false;
            foreach (var gc5 in greenC5FlowPanel.Controls)
            {
                if (gc5.GetType() == typeof(GreenC5UserControl))
                {
                    if ((gc5 as GreenC5UserControl).Status =="Active")
                        (gc5 as GreenC5UserControl).StopExecute();                    
                }
            }
            Status = "Idle";
            this.Invoke((MethodInvoker)(() => lbStatus.Text = "Status: Idle"));
        }

        public void SetInstanceInfiniteMode(bool infiniteMode)
        {
            foreach (var gc5 in greenC5FlowPanel.Controls)
            {
                if (gc5.GetType() == typeof(GreenC5UserControl))
                {
                    (gc5 as GreenC5UserControl).IsInfiniteMode = infiniteMode;                   
                }
            }
        }

        public void SetDecisionMakingCriteria(int k1, double k2)
        {
            DecisionMakingCriteriaK1 = k1;
            DecisionMakingCriteriaK2 = k2;
            foreach (var gc5 in greenC5FlowPanel.Controls)
            {
                if (gc5.GetType() == typeof(GreenC5UserControl))
                {
                    (gc5 as GreenC5UserControl).SetDecisionCriteria(DecisionMakingCriteriaK1, DecisionMakingCriteriaK2);
                }
            }
        }


        public void DisableAddNewGreenC5()
        {
            this.butAddNewGreenC5.Enabled = false;
        }

        #region INotification methods and events
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
