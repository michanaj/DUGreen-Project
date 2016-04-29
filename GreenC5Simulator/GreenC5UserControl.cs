using System;
using SCG = System.Collections.Generic;
using C5;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrudBasedCollection;
using GreenCrudBasedCollection;
using System.IO;

namespace GreenC5Simulator
{
    public partial class GreenC5UserControl : UserControl, INotifyPropertyChanged
    {
        public string GreenC5Name { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public C5DataStructure CurrentC5DataStructure { get; set; }
        public GreenC5<string> InternalDataStructure { get; set; }
        public bool IsInfiniteMode { get; set; }

        //C5DataStructure currentDs = null;


        int numExecution = 0;
        public int ExecutionCount
        {
            get
            {
                return this.numExecution;
            }

            set
            {
                if (value != this.numExecution)
                {
                    this.numExecution = value;
                    OnPropertyChanged("ExecutionCount");
                }
            }
        }

        private KeyValuePair<string, string> SelectedProgram { get; set; }

        //run mode
        private bool IsDynamicMode { get; set; }
        private DataStructureMode RunMode { get; set; }
        private C5DataStructure StartDataStructure { get; set; }
        private DataStructureGroup CurrentDataStructureGroup { get; set; }
        private KeyValuePair<string, string> CurrentProgram { get; set; }

        //constant OP lenght and test string object
        private int opLength = 10000;
        private string testString = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest";
        private C5DataStructure[] c5DSs = new C5DataStructure[] {C5DataStructure.HashBag, C5DataStructure.ArrayList, C5DataStructure.HashedArrayList,
                                                            C5DataStructure.HashedLinkedList, C5DataStructure.HashSet,
                                                            C5DataStructure.LinkedList, C5DataStructure.SortedArray, C5DataStructure.TreeBag, C5DataStructure.TreeSet,
                                                            };//9 data structures

        private SCG.List<KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>>> dataStructuresByGroup = new SCG.List<KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>>>();
        SCG.List<string[]> rawCRUDArrayRows;
        
        bool startMode = true;

        //default criteria
        int K1 = 2;
        double K2 = 20;
        public GreenC5UserControl(string greenC5Name, bool infiniteExecution, int k1, double k2)
        {
            InitializeComponent();
            Status = "Idle";
            IsInfiniteMode = infiniteExecution;
            GreenC5Name = greenC5Name;
            lbName.Text = "Instance ID: " + GreenC5Name;
            InternalDataStructure = new GreenC5<string>();
            K1 = k1;
            K2 = k2;
            
           
            //set datastructure list by groups for validation
            SCG.List<C5DataStructure> iCollection = new SCG.List<C5DataStructure>();
            iCollection.Add(C5DataStructure.ArrayList);
            iCollection.Add(C5DataStructure.HashBag);
            iCollection.Add(C5DataStructure.HashedArrayList);
            iCollection.Add(C5DataStructure.HashedLinkedList);
            iCollection.Add(C5DataStructure.HashSet);
            iCollection.Add(C5DataStructure.LinkedList);
            iCollection.Add(C5DataStructure.SortedArray);
            iCollection.Add(C5DataStructure.TreeBag);
            iCollection.Add(C5DataStructure.TreeSet);
            dataStructuresByGroup.Add(new KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>>(DataStructureGroup.ICollection, iCollection));

            SCG.List<C5DataStructure> iCollectionBag = new SCG.List<C5DataStructure>();
            iCollectionBag.Add(C5DataStructure.HashBag);
            iCollectionBag.Add(C5DataStructure.TreeBag);
            iCollectionBag.Add(C5DataStructure.ArrayList);
            iCollectionBag.Add(C5DataStructure.LinkedList);
            dataStructuresByGroup.Add(new KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>>(DataStructureGroup.ICollectionBag, iCollectionBag));

            SCG.List<C5DataStructure> iCollectionSet = new SCG.List<C5DataStructure>();
            iCollectionSet.Add(C5DataStructure.HashSet);
            iCollectionSet.Add(C5DataStructure.TreeSet);
            iCollectionSet.Add(C5DataStructure.HashedArrayList);
            iCollectionSet.Add(C5DataStructure.HashedLinkedList);
            iCollectionSet.Add(C5DataStructure.SortedArray);
            dataStructuresByGroup.Add(new KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>>(DataStructureGroup.ICollectionSet, iCollectionSet));

            SCG.List<C5DataStructure> iList = new SCG.List<C5DataStructure>();
            iList.Add(C5DataStructure.ArrayList);
            iList.Add(C5DataStructure.LinkedList);
            iList.Add(C5DataStructure.HashedArrayList);
            iList.Add(C5DataStructure.HashedLinkedList);
            iList.Add(C5DataStructure.SortedArray);
            dataStructuresByGroup.Add(new KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>>(DataStructureGroup.IList, iList));

            SCG.List<C5DataStructure> iListBag = new SCG.List<C5DataStructure>();
            iListBag.Add(C5DataStructure.ArrayList);
            iListBag.Add(C5DataStructure.LinkedList);
            dataStructuresByGroup.Add(new KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>>(DataStructureGroup.IListBag, iListBag));

            SCG.List<C5DataStructure> iListSet = new SCG.List<C5DataStructure>();
            iListSet.Add(C5DataStructure.HashedArrayList);
            iListSet.Add(C5DataStructure.HashedLinkedList);
            iListSet.Add(C5DataStructure.SortedArray);
            dataStructuresByGroup.Add(new KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>>(DataStructureGroup.IListSet, iListSet));


            //set data structure groups
            SCG.List<KeyValuePair<DataStructureGroup, C5DataStructure>> dsGroupAndWorstDataStructures = new SCG.List<KeyValuePair<DataStructureGroup, C5DataStructure>>();
            dsGroupAndWorstDataStructures.Add(new KeyValuePair<DataStructureGroup, C5DataStructure>(DataStructureGroup.ICollection, C5DataStructure.ArrayList));
            dsGroupAndWorstDataStructures.Add(new KeyValuePair<DataStructureGroup, C5DataStructure>(DataStructureGroup.ICollectionBag, C5DataStructure.LinkedList));
            dsGroupAndWorstDataStructures.Add(new KeyValuePair<DataStructureGroup, C5DataStructure>(DataStructureGroup.ICollectionSet, C5DataStructure.SortedArray));
            dsGroupAndWorstDataStructures.Add(new KeyValuePair<DataStructureGroup, C5DataStructure>(DataStructureGroup.IList, C5DataStructure.LinkedList));
            dsGroupAndWorstDataStructures.Add(new KeyValuePair<DataStructureGroup, C5DataStructure>(DataStructureGroup.IListBag, C5DataStructure.LinkedList));
            dsGroupAndWorstDataStructures.Add(new KeyValuePair<DataStructureGroup, C5DataStructure>(DataStructureGroup.IListSet, C5DataStructure.SortedArray));

            //cbDataStructureGroup.DisplayMember = "Key";
            //cbDataStructureGroup.ValueMember = "Value";
            cbDataStructureGroup.DataSource = dsGroupAndWorstDataStructures;
            

            SCG.List<KeyValuePair<string, string>> crudWorkloadPrograms = new SCG.List<KeyValuePair<string, string>>();
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Small Program #1", @"CRUD Workloads\SmallRandomApplication1.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Small Program #2", @"CRUD Workloads\SmallRandomApplication2.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Small Program #3", @"CRUD Workloads\SmallRandomApplication3.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Small Program #4", @"CRUD Workloads\SmallRandomApplication4.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #1", @"CRUD Workloads\RandomApplication1.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #2", @"CRUD Workloads\RandomApplication2.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #3", @"CRUD Workloads\RandomApplication3.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #4", @"CRUD Workloads\RandomApplication4.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #5", @"CRUD Workloads\RandomApplication5.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #6", @"CRUD Workloads\RandomApplication6.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #7", @"CRUD Workloads\RandomApplication7.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #8", @"CRUD Workloads\RandomApplication8.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #9", @"CRUD Workloads\RandomApplication9.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Simulated Program #10", @"CRUD Workloads\RandomApplication10.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("A* Path Finder - Custom", @"CRUD Workloads\AStarPathFinderProgram_Custom_Formula.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("A* Path Finder - Diagnal Shortcut", @"CRUD Workloads\AStarPathFinderProgram_DiagonalShortcut_Formula.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("A* Path Finder - Euclidean", @"CRUD Workloads\AStarPathFinderProgram_Euclidean_Formula.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("A* Path Finder - Manhatan", @"CRUD Workloads\AStarPathFinderProgram_Manhatan_Formula.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("A* Path Finder - Max Dx Dy", @"CRUD Workloads\AStarPathFinderProgram_MaxDXDY_Formula.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Genetic Algorithm - Fitness Table", @"CRUD Workloads\GA-FittnessTableProgram.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Genetic Algorithm - Next Generation", @"CRUD Workloads\GA-NextGenerationProgram.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Genetic Algorithm - This Generation", @"CRUD Workloads\GA-ThisGenerationProgram.csv"));
            crudWorkloadPrograms.Add(new KeyValuePair<string, string>("Huffman Encoding", @"CRUD Workloads\HuffmanCodingProgram.csv"));

            
            cbPrograms.DataSource = new BindingSource(crudWorkloadPrograms, null);
            cbPrograms.DisplayMember = "Key";
            //cbPrograms.ValueMember = "Value";
            //set the values to the program potion dropdownlist

            //set data structure options to a Static Mode selection
            cbStartDataStructure.DataSource = c5DSs;

            //Set run mode
            cbRunMode.DataSource = Enum.GetValues(typeof(DataStructureMode));
            cbRunMode.SelectedItem = DataStructureMode.Dynamic;
            

            if(cbDataStructureGroup.SelectedItem != null)
            {
                KeyValuePair<DataStructureGroup, C5DataStructure> dsg = (KeyValuePair<DataStructureGroup, C5DataStructure>) cbDataStructureGroup.SelectedItem;
                CurrentDataStructureGroup = dsg.Key;
                StartDataStructure = dsg.Value;
                CurrentC5DataStructure = dsg.Value;
                lbCurrentDataStructure.Text = "Current Data Structure: " + CurrentC5DataStructure.ToString();
                cbStartDataStructure.SelectedItem = StartDataStructure;
            }

            if(cbPrograms.SelectedItem != null)
            {
                CurrentProgram = (KeyValuePair<string, string>)cbPrograms.SelectedItem; 
            }
            
            startMode = false;
            bool ok = LoadProgram();
            

        }

        private void InternalDataStructure_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentInternalDataStructure")
            {
                this.Invoke((MethodInvoker)(() => lbCurrentDataStructure.Text = "Current Data Structure: " + InternalDataStructure.CurrentDataStructure.ToString()));
            }

            if (e.PropertyName == "PredictionResultMessage")
            {
                this.Invoke((MethodInvoker)(() => lbPrediction.Text = "Prediction: " + InternalDataStructure.PredictionResultMessage));

            }

            if (e.PropertyName == "TransformationCount")
            {
                this.Invoke((MethodInvoker)(() => lbTransCount.Text = "Transform Count: " + InternalDataStructure.TransformationCount));

            }
        }

        bool keepRunning = true;

        #region INotificationChanged Properties and Methods

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

        #region Public Methods

        public void Execute()
        {
            keepRunning = true;
            Status = "Active";
            
            this.Invoke((MethodInvoker)(() => lbStatus.Text = "Status: Running..."));
            this.Invoke((MethodInvoker)(() => this.BackColor = Color.Red));

            //set GreenC5 property
            //InternalDataStructure = new GreenC5<string>();
            InternalDataStructure.PropertyChanged += InternalDataStructure_PropertyChanged;
            InternalDataStructure.RunMode = RunMode;
            InternalDataStructure.InterfaceAndSetBagProperty = CurrentDataStructureGroup;
            InternalDataStructure.CurrentDataStructure = CurrentC5DataStructure;
            InternalDataStructure.SetDecisionMakingCriteria(K1, K2);
            InternalDataStructure.CreateNewInstanceOfInternalC5DataStructure(CurrentC5DataStructure);
            InternalDataStructure.TransformationCount = 0;
            InternalDataStructure.PredictionResultMessage = "N/a";
            
            
            this.Invoke((MethodInvoker)(() => lbCurrentDataStructure.Text = "Current Data Structure: " + InternalDataStructure.CurrentDataStructure.ToString()));
            this.Invoke((MethodInvoker)(() => lbTransCount.Text = "Transform Count: " + InternalDataStructure.TransformationCount));
            this.Invoke((MethodInvoker)(() => lbNumExecution.Text = "#Exec: 0"));

            int targetInd = 0;
            ExecutionCount = 0;
            do { 
                foreach (string[] workload in rawCRUDArrayRows)
                {
                    for (int i = 0; i < workload.Length; i++)
                    {
                        string op = workload[i].Trim();
                        //perform CRUD operations
                        if (op == "C")
                        {
                            bool t = InternalDataStructure.Create(testString + targetInd);
                            targetInd++;
                        }
                        else if (op == "R")
                        {
                            string item = testString + targetInd; //this always find last item
                            string t = InternalDataStructure.Retrieve(item);
                        }
                        else if (op == "U")
                        {
                            bool t = InternalDataStructure.Update(testString + targetInd);
                        }
                        else if (op == "D")
                        {
                            bool t = InternalDataStructure.Delete(testString + targetInd);
                            if (targetInd > 0)
                                targetInd--;
                        }

                        if (!keepRunning)
                        {
                            break;
                        }
                    }
                }
                ExecutionCount++;
                InternalDataStructure.Clear();//clear everytime after each program execution
                this.Invoke((MethodInvoker)(() => lbNumExecution.Text = "#Exec: " + ExecutionCount.ToString()));

                if (!keepRunning)
                {
                    break;
                }
            }
            while (IsInfiniteMode);

            this.Invoke((MethodInvoker)(() => this.BackColor = Color.LimeGreen));
            this.Invoke((MethodInvoker)(() => lbStatus.Text = "Status: Idle"));
            Status = "Idle";
        }

        public void StopExecute()
        {
            keepRunning = false;
            //this.Invoke((MethodInvoker)(() => lbStatus.Text = "Status: Idle"));
            //this.Invoke((MethodInvoker)(() => this.BackColor = Color.LimeGreen));
        }

        public void SetDecisionCriteria(int k1, double k2)
        {
            K1 = k1;
            K2 = k2;
        }

        #endregion

        #region Private Methods 

        private bool LoadProgram()
        {
            bool success = false;

            try
            {
                string program = CurrentProgram.Value;               
                
                //read file
                //1. Read data from a Dataset
                rawCRUDArrayRows = parseCSV(program);
                //for each data structure
            }
            catch (Exception ex)
            {

            }

            return success;
        }

        private SCG.List<string[]> parseCSV(string path)
        {
            SCG.List<string[]> parsedData = new SCG.List<string[]>();

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
        #endregion

        #region User Selection Change Events
        private void cbDataStructureGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!startMode)
            if(cbDataStructureGroup.SelectedItem != null)
            {
                    KeyValuePair<DataStructureGroup, C5DataStructure> dsg = (KeyValuePair<DataStructureGroup, C5DataStructure>)cbDataStructureGroup.SelectedItem;
                    //automatically set the start data structure to the worst option in the selected data structure group
                    CurrentDataStructureGroup = dsg.Key;
                    StartDataStructure = dsg.Value;
                    CurrentC5DataStructure = dsg.Value;
                    lbCurrentDataStructure.Text = "Current Data Structure: " + CurrentC5DataStructure.ToString();
                    cbStartDataStructure.SelectedItem = dsg.Value;

            }
        }

        
        private void cbStartDataStructure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!startMode)
                if (cbStartDataStructure.SelectedItem != null)
                {
                   
                    //TODO? check if the selected data structure belong to the selected data structure group
                    bool ok = false;
                    foreach (KeyValuePair<DataStructureGroup, SCG.List<C5DataStructure>> group in dataStructuresByGroup)
                    {
                        if (group.Key == CurrentDataStructureGroup)
                        {
                            if (group.Value.Contains((C5DataStructure)cbStartDataStructure.SelectedItem))
                                ok = true;
                        }
                    }
                    if (ok)
                    {
                        StartDataStructure = (C5DataStructure)cbStartDataStructure.SelectedItem;
                        CurrentC5DataStructure = StartDataStructure;
                        lbCurrentDataStructure.Text = "Current Data Structure: " + CurrentC5DataStructure.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Your selected data structure is not belong to the selected group. Please select a new one.");
                        cbStartDataStructure.SelectedItem = StartDataStructure;
                    }
                }
        }
        //programs
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPrograms.SelectedItem != null)
            {
                CurrentProgram = (KeyValuePair<string, string>)cbPrograms.SelectedItem;
                bool ok = LoadProgram();
            }
        }

        //run mode
        private void cbRunMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbRunMode.SelectedItem != null)
            {
                RunMode = (DataStructureMode) cbRunMode.SelectedItem;
                InternalDataStructure.RunMode = RunMode;
            }
        }

        #endregion


    }
}
