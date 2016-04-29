using System;
using System.Linq;
using CrudBasedCollection;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;

namespace GreenCrudBasedCollection
{
    ///Developed by: Junya Michanan
    ///Date: September 1, 2015
    ///Computer Science Department, University of Denver, Denver, Colorado USA 80210
    ///
    ///Name: GreenC5 (Green CRUD-Based C5 Collection)
    ///Description: GreenC5 is an easy-to-use green dynamic data structure that can be trained, learn and predict workload and 
    ///dynamically transform to different C5 data structures to fit the workload for energy efficiency
    ///
    
    #region Public Enumerations
    //Data Structure Mode is an application run mode for each instance of the data structure
    //Slient--This is the least overhead mode of the Green DS. The internal data structure is set with a C5 data structure with the Green component disabled (least overheads)
    //Static-. The internal data structure is statically set with a C5 data structure with the Green component enabled. The tracking mechanisim is working but the transformation and data structure switching never take place unless the mode is changed to Dynamic.
    //Dynamic--The internal data structure is changed dynamically to the workloads as directed by the Green component (higer overheads)
    public enum DataStructureMode { Silent, Static, Dynamic }
    //this enumeration is the data structure group choice to be specified by the user or program
    //this will controll how the Predictor and Decision maker do the data structure switching/transformation
    public enum DataStructureGroup { ICollection = 0, ICollectionBag = 1, ICollectionSet = 2, IList = 3, IListBag = 4, IListSet = 5 }
    //Transformation Mode is to control how the data structure is to be transformed when notified by the Green component
    //Note: This version only supports Immediate transformation mode
    public enum TransformationMode { Immediate, WhenIdle }
    //Active status of the underneath data structure
    //Note:This version only supports Active status
    public enum ActiveStatus { Active, Idle }
    #endregion

    public class GreenC5<T>:CrudBasedCollection<T>, INotifyPropertyChanged
    {
        #region Private properties
        //SimpleScheduler scheduler = new SimpleScheduler();
        private int elementSizeAtTimeOfOperation = 0;
        private int C = 0;
        private int R = 0;
        private int U = 0;
        private int D = 0;
        
        private CrudCounter crudCounter = null;
        private Green greenComponent = null;

        #endregion

        #region public Properties
        public int CrudLengthThreshold { get; set; }
         
        //This for handling RunMode property value changed event
        private DataStructureMode runMode;
        
        public DataStructureMode RunMode
        { 
            get
            {
                return runMode;
            }
            set { 
                runMode = value; 
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("RunMode");
                ReconfigGreenProperties(runMode);
            }
        }

        private DataStructureGroup interfaceAndSetBagProp;
        public DataStructureGroup InterfaceAndSetBagProperty {
            get { return interfaceAndSetBagProp; }
            set { 
                interfaceAndSetBagProp = value; 
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("InterfaceAndSetBagProperty");
                SetDefaultDataStructure(interfaceAndSetBagProp);
            }
        }


        public TransformationMode TransformMode { get; set; }
        public ActiveStatus Status { get; set; }

        public int NotificationCount { get; set; }

        private int transformationCount;
        public int TransformationCount
        {
            get
            {
                return this.transformationCount;
            }

            set
            {
                if (value != this.transformationCount)
                {
                    this.transformationCount = value;
                    OnPropertyChanged("TransformationCount");
                }
            }
        }
        private C5DataStructure currentInternalDataStructure;
        public C5DataStructure CurrentInternalDataStructure
        {
            get
            {
                return this.currentInternalDataStructure;
            }

            set
            {
                //if (value != this.currentInternalDataStructure)
                //{
                    this.currentInternalDataStructure = value;
                    OnPropertyChanged("CurrentInternalDataStructure");
                //}
            }
        }

        private string predictionInfo;
        public string PredictionResultMessage
        {
            get
            {
                return this.predictionInfo;
            }

            set
            {
                if (value != this.predictionInfo)
                {
                    this.predictionInfo = value;
                    OnPropertyChanged("PredictionResultMessage");
                }
            }
        }
        #endregion

        #region Property change and reconfig and default setting methods
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
          {
             if (PropertyChanged != null)
              {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
              }
          }        

        private void ReconfigGreenProperties(DataStructureMode runMode)
        {
            if (runMode == DataStructureMode.Static)
            {
                crudCounter = null;
                greenComponent = null;
            }
            else //if Silent and Dynamic mode, attach all 
            {
                
                crudCounter = new CrudCounter(CrudLengthThreshold);
                crudCounter.CrudLenghtThresholdReached += CrudCounter_CrudCountThresholdReached;//attached the event
                
                
                greenComponent = new Green();
                greenComponent.DecisionMaker.IsDynamicMode = false;
                greenComponent.FeatureRegistered += GreenComponent_FeatureRegistered;//OVERHEAD, comment this line to minimize overhead

                //also attach the tranform notify event only if run in Dynamic mode
                if (runMode == DataStructureMode.Dynamic)
                {
                    greenComponent.DecisionMaker.IsDynamicMode = true;
                    greenComponent.DecisionMaker.TransformationNotified += DecisionMaker_TransformNotified;
                    //this.TransformCompleted += GreenCrudBasedCollection_TransformCompleted;
                }
                
            }
        }

        
        private void SetDefaultDataStructure(DataStructureGroup interfaceAndSetBagProp)
        {
            C5DataStructure previousDS = base.CurrentDataStructure;

            //this default setting is based on our prior research experiment
            //please read our paper, "Predicting Data Structures for Energy Efficient Computing"
            if(interfaceAndSetBagProp == DataStructureGroup.ICollection)
            {
                base.CurrentDataStructure = C5DataStructure.HashSet;                
            }
            else if (interfaceAndSetBagProp == DataStructureGroup.ICollectionBag)
            {
                base.CurrentDataStructure = C5DataStructure.HashBag;
            }
            else if (interfaceAndSetBagProp == DataStructureGroup.ICollectionSet)
            {
                base.CurrentDataStructure = C5DataStructure.HashSet;
            }
            else if (interfaceAndSetBagProp == DataStructureGroup.IList)
            {
                base.CurrentDataStructure = C5DataStructure.HashedLinkedList;
            }
            else if (interfaceAndSetBagProp == DataStructureGroup.IListBag)
            {
                base.CurrentDataStructure = C5DataStructure.ArrayList;
            }
            else if (interfaceAndSetBagProp == DataStructureGroup.IListSet)
            {
                base.CurrentDataStructure = C5DataStructure.HashedLinkedList;
            }
            else
            {
                base.CurrentDataStructure = C5DataStructure.HashSet;
            }
            //This will make sure that the internal data structure is also changed with the same existing data in it
            if(previousDS != base.CurrentDataStructure)
                base.TransformTo(base.CurrentDataStructure);
        }

        #endregion

        #region Constructor
        public GreenC5()
        {
            //default setting
            CrudLengthThreshold = 10000;//***NOTE:this must always be performed before setting the RunMode property

            RunMode = DataStructureMode.Dynamic; //THIS WILL ALSO RAISE OnPropertyChanged EVENT and instantiate/enable/disable Green components           
            TransformMode = TransformationMode.Immediate;//THIS VERSION ONLY SUPPORT Immediate transformation mode
            Status = ActiveStatus.Active;                //and presume the Status always Active            
            InterfaceAndSetBagProperty = DataStructureGroup.ICollection;//THIS WILL ALSO RAISE OnPropertyChanged EVENT and set a default data structure based on the group property
            elementSizeAtTimeOfOperation = this.ElementSize;
            TransformationCount = 0;
                                    
        }
        #endregion

        #region Event Handlers
        //private void GreenCrudBasedCollection_TransformCompleted(object sender, EventArgs e)
        //{  
        //    Console.WriteLine("Transformation Completed => "+this.CurrentDataStructure.ToString());//OVERHEAD, comment the line to reduce the overhead
        //}
        private void GreenComponent_FeatureRegistered(object sender, FeatureRegisteredEventArgs e)
        {
            PredictionResultMessage = string.Format("{1} (SC={2}, PD={0:0.00}%)", e.PredictionProbabilityValue, e.NextPredictedDataStructure, e.RegisteredCountFromLastTransformation);
            Console.WriteLine(string.Format("{6}: Feature Registered=>G={0}, N={1}, %C={2}, %R={3}, %U={4}, %D={5}, ClassifiedDS ={11}, CDS={6}, PD={7}, NDS={8}, RegCount={9}, Yes Decision={10}", e.InterfaceAndSetBagPropertyGroup, e.ElementSizeAtTimeOfOperation, e.PercentC, e.PercentR, e.PercentU, e.PercentD, e.CurrentDataStructure, e.PredictionProbabilityValue, e.NextPredictedDataStructure, e.RegisteredCountFromLastTransformation, GetYesDecisionCount(), e.ClassifiedDataStructure));
        }                     
        private void DecisionMaker_TransformNotified(object sender, TransformNotifiedEventArgs e)
        {
            //notify the Crud-Based Collection to transform
            //Console.WriteLine("Transformatin notified and start transforming to {0}", e.TransformToDataStructure);//OVERHEAD, comment this line to reduce overhead
            base.TransformTo(e.TransformToDataStructure);
            TransformationCount++;
            CurrentInternalDataStructure = e.TransformToDataStructure;
            //Console.WriteLine("#"+TransformationCount+" Transformation Completed => " + this.CurrentDataStructure.ToString());

        }
        private void CrudCounter_CrudCountThresholdReached(object sender, EventArgs e)
        {
            
            double PC = (double) C * (double)100 / (double)CrudLengthThreshold;
            double PR = (double)R * (double)100 / (double)CrudLengthThreshold;
            double PU = (double)U * (double)100 / (double)CrudLengthThreshold;
            double PD = (double)D * (double)100 / (double)CrudLengthThreshold;
            
            //Register the features to the Green component            
            //=================================================================
            //execute the RegisterFeature method in a separate thread using Task Paralell Library (TPL)
            //Task registerFeaturedTask =  Task.Run( () => greenComponent.RegisterFeature(InterfaceAndSetBagProperty, elementSizeAtTimeOfOperation, PC , PR, PU, PD, CurrentDataStructure));
            //Task.Factory.StartNew(() => greenComponent.RegisterFeature(InterfaceAndSetBagProperty, elementSizeAtTimeOfOperation, PC, PR, PU, PD, CurrentDataStructure));
            
            //execute the RegisterFeature method in a separate thread using ThreadPool
            //ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(greenComponent.RegisterFeature), new FeatureAndStateInfo(InterfaceAndSetBagProperty, elementSizeAtTimeOfOperation, PC, PR, PU, PD, CurrentDataStructure));
            
            //OR execute the RegisterFeature method in the same thread 
            greenComponent.RegisterFeature(InterfaceAndSetBagProperty, elementSizeAtTimeOfOperation, PC, PR, PU, PD, CurrentDataStructure);           
                        
            //For testing, uncomment this section when debugging
            //Task tracingTask = Task.Run( () => Trace.WriteLine(string.Format("CURRENT: {7} COUNTER: {0} FEATURE: {1},{2},{3:F2},{4:F2},{5:F2},{6:F2}", CrudLengthThreshold + " count reached", InterfaceAndSetBagProperty, elementSizeAtTimeOfOperation, PC, PR, PU, PD, CurrentDataStructure)));
               

            elementSizeAtTimeOfOperation = this.ElementSize;

            //Reset counts to 0
            C = 0;
            R = 0;
            U = 0;
            D = 0;

            crudCounter.Reset();
            
        }
        #endregion

        #region ICrudable Interfaces
        public bool Create(T obj)
        {
            //TODO? Investigate how the try/finally impact the performance
            try
            {
                return base.Create(obj);
            }
            finally {
                C++;
                if (crudCounter != null)
                {
                    crudCounter.Add(1);
                }
            }
        }

        public T Retrieve(T obj)
        {

            try
            {
                return base.Retrieve(obj);
            }
            finally
            {                
                R++;
                if (crudCounter != null)
                {
                    crudCounter.Add(1);
                }
            }
        }

        public bool Update(T obj)
        {
            try
            {
                return base.Update(obj);
            }
            finally
            {                
                U++;
                if (crudCounter != null)
                {
                    crudCounter.Add(1);
                }
            }
            
        }
        public bool Delete(T obj)
        {

            try
            {
                return base.Delete(obj);
            }
            finally
            {               
                    D++;
                if (crudCounter != null)
                {
                    crudCounter.Add(1);
                }
            }
        }
              

        #endregion

        #region Public methods
        /// <summary>
        /// Set decsion criteria to different K1 and K2
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        public void SetDecisionMakingCriteria(int k1, double k2)
        {
            if (RunMode != DataStructureMode.Static)
            {
                greenComponent.DecisionMaker.RegisteredDatastructureCountThreshold = k1;
                greenComponent.DecisionMaker.PredictedPropbabilityThreshold = k2;
            }
        }

        /// <summary>
        /// Get number of Yes that satisfies the K1 and K2 criteria
        /// </summary>
        /// <returns></returns>
        public int GetYesDecisionCount()
        {
            return greenComponent.DecisionMaker.YesDecisionCount;
        }
        
        #endregion
    }

    #region CrudCounter Class 
    class CrudCounter
    {
        private int lenght;
        private int total = 0;

        public CrudCounter(int crudLenght)
        {
            lenght = crudLenght;
        }

        public void Reset()
        {
            total = 0;
        }

        public void Add(int x)
        {
            total += x;
            if (total >= lenght)
            {
                OnCrudLenghtThresholdReached(EventArgs.Empty);
            }
        }

        protected virtual void OnCrudLenghtThresholdReached(EventArgs e)
        {
            EventHandler handler = CrudLenghtThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler CrudLenghtThresholdReached;
    }
    #endregion
}
