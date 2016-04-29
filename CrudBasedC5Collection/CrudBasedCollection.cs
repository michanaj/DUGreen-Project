using System;
using C5;

namespace CrudBasedCollection
{
    ///Developed by: Junya Michanan
    ///Date: February 19, 2015
    ///Computer Science Department, University of Denver, Denver, Colorado USA 80210
    
    /// <summary>
    /// All of C5 collections:
    /// ArrayList -->An array-based data structure
    /// LinkedList -->A double-linked list data structure
    /// HashBag -->A hash-based bag data structure
    /// TreeBag -->A balanced red-black tree bag data structure
    /// HashedArrayList, 
    /// HashedLinkedList, 
    /// SortedArray, 
    /// HashSet, 
    /// TreeSet
    /// NOTE: WrappedArray is excluded because it requires an array to be wrapped as an input parameter
    /// </summary>
    public enum C5DataStructure { ArrayList, LinkedList, HashBag, TreeBag, HashedArrayList, HashedLinkedList, SortedArray, HashSet, TreeSet,  Unknown } 

    /// <summary>
    /// CRUD Interface-->C = Create, R = Retrieve, U = Update, D = Delete
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICrudable<T>
    {
        bool Create(T obj);
        T Retrieve(T obj);
        bool Update(T obj);
        bool Delete(T obj);
    }


    /// <summary>
    /// An Abstract Factory Class of C5 Collection
    /// </summary>
    public abstract class C5CollectionFactory<T>
    {
        public abstract void CreateNewInstanceOfInternalC5DataStructure(C5DataStructure dataStructure);
    }
    /// <summary>
    /// CRUD-Basded C5 Collection 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CrudBasedCollection<T> : C5CollectionFactory<T>, ICrudable<T>
    {
        #region Public Properties
                
        public ICollection<T> InternalC5DataStructure { get; set; }
        public C5DataStructure CurrentDataStructure { get; set; }
        public int ElementSize { get { return InternalC5DataStructure.Count; } }
        
        #endregion

        #region Constructors
        public CrudBasedCollection()
        {
            //set default internal data structure to a HashSet
            CurrentDataStructure = C5DataStructure.HashSet;
            CreateNewInstanceOfInternalC5DataStructure(CurrentDataStructure); 
            
        }
               
        public CrudBasedCollection( C5DataStructure dataStructure)
        {
            CurrentDataStructure = dataStructure;
            CreateNewInstanceOfInternalC5DataStructure(dataStructure);
            
        }
        #endregion
               
        #region C5CollectionFactory Members
        public override void CreateNewInstanceOfInternalC5DataStructure(C5DataStructure dataStructure)
        {
            switch (dataStructure)
            {
                case C5DataStructure.ArrayList:
                    InternalC5DataStructure = new ArrayList<T>();
                    break;
                case C5DataStructure.LinkedList:
                    InternalC5DataStructure = new LinkedList<T>();
                    break;
                case C5DataStructure.HashBag:
                    InternalC5DataStructure = new HashBag<T>();
                    break;
                case C5DataStructure.TreeBag:
                    InternalC5DataStructure = new TreeBag<T>();
                    break;
                case C5DataStructure.HashedArrayList:
                    InternalC5DataStructure = new HashedArrayList<T>();
                    break;  
                case C5DataStructure.HashedLinkedList:
                    InternalC5DataStructure = new HashedLinkedList<T>();
                    break;
                case C5DataStructure.SortedArray:
                    InternalC5DataStructure = new SortedArray<T>();
                    break;   
                case C5DataStructure.HashSet:
                    InternalC5DataStructure = new HashSet<T>();
                    break;
                case C5DataStructure.TreeSet:
                    InternalC5DataStructure = new TreeSet<T>();
                    break;
                default: throw new ArgumentException("Unknown C5 Collection name");
            }
            
            
        }

        
        #endregion

        #region ICrudable Interfaces
        /// <summary>
        ///Create (C) - utilize the Add method to attempt to add item obj to the collection. 
        ///Returns true if the item was added; returns false if it was not.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Create(T obj)
        {
            return InternalC5DataStructure.Add(obj);
        }

        /// <summary>
        ///Retrieve (R) - utilize the Find method and return the object if the collection contains an item equal to obj,
        ///and in that case binds one such item to the ref parameter obj; otherwise returns default object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T Retrieve(T obj)
        {
            if (InternalC5DataStructure.Find(ref obj))
                return obj;
            else
                return default(T);//this is just for testing
        }

        /// <summary>
        ///Update (U)-returns true if the collection contains an item equal to obj,
        ///in which case that item is replaced by obj; otherwise returns false without modifying
        ///the collection.
        /// </summary>
        /// <param name="obj"></param>  
        /// <returns>true or false</returns>
        public bool Update(T obj)
        {
            return InternalC5DataStructure.Update(obj);
            
        }
        /// <summary>
        ///Delete (D)-attempts to remove an item equal to obj from the collection.
        ///Returns true if the collection did contain an item equal to obj, false if it did not.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true or false</returns>
        public bool Delete(T obj)
        {
            return InternalC5DataStructure.Remove(obj);
        }

        /// <summary>
        /// This is not a part of CRUD operation. It is for clearing elements during the experiments
        /// </summary>
        public void Clear()
        {
            InternalC5DataStructure.Clear();
        }

        #endregion

        #region Public Methods
        public void TransformTo(C5DataStructure dataStructure)
        {
            if (CurrentDataStructure != dataStructure)
            {
                //Method 1: use case state ment to appropriately copy data over to appropriate data structure
                switch (dataStructure)
                {
                    case C5DataStructure.ArrayList:
                        InternalC5DataStructure = CopyToArrayList(InternalC5DataStructure);
                        break;
                    case C5DataStructure.LinkedList:
                        InternalC5DataStructure = CopyToLinkedList(InternalC5DataStructure);
                        break;
                    case C5DataStructure.HashBag:
                        InternalC5DataStructure = CopyToHashBag(InternalC5DataStructure);
                        break;
                    case C5DataStructure.TreeBag:
                        InternalC5DataStructure = CopyToTreeBag(InternalC5DataStructure);
                        break;
                    case C5DataStructure.HashedArrayList:
                        InternalC5DataStructure = CopyToHashedArrayList(InternalC5DataStructure);
                        break;
                    case C5DataStructure.HashedLinkedList:
                        InternalC5DataStructure = CopyToHashedLinkedList(InternalC5DataStructure);
                        break;
                    case C5DataStructure.SortedArray:
                        InternalC5DataStructure = CopyToSortedArray(InternalC5DataStructure);
                        break;
                    case C5DataStructure.HashSet:
                        InternalC5DataStructure = CopyToHashSet(InternalC5DataStructure);
                        break;
                    case C5DataStructure.TreeSet:
                        InternalC5DataStructure = CopyToTreeSet(InternalC5DataStructure);
                        break;
                    default: throw new ArgumentException("Unknown C5 Collection name");
                }

                CurrentDataStructure = dataStructure;
                OnTransformCompleted(EventArgs.Empty);
            }
        }

        public ArrayList<T> CopyToArrayList<T>(ICollection<T> lst)
        {
            ArrayList<T> lstCopy = new ArrayList<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);                
            }
            return lstCopy;
        }

        public LinkedList<T> CopyToLinkedList<T>(ICollection<T> lst)
        {
            LinkedList<T> lstCopy = new LinkedList<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);               
            }
            return lstCopy;
        }

        public HashBag<T> CopyToHashBag<T>(ICollection<T> lst)
        {
            HashBag<T> lstCopy = new HashBag<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);                
            }
            return lstCopy;
        }

        public TreeBag<T> CopyToTreeBag<T>(ICollection<T> lst)
        {
            TreeBag<T> lstCopy = new TreeBag<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);                
            }
            return lstCopy;
        }

        public HashedArrayList<T> CopyToHashedArrayList<T>(ICollection<T> lst)
        {
            HashedArrayList<T> lstCopy = new HashedArrayList<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);                
            }
            return lstCopy;
        }

        public HashedLinkedList<T> CopyToHashedLinkedList<T>(ICollection<T> lst)
        {
            HashedLinkedList<T> lstCopy = new HashedLinkedList<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);               
            }
            return lstCopy;
        }

        public SortedArray<T> CopyToSortedArray<T>(ICollection<T> lst)
        {
            SortedArray<T> lstCopy = new SortedArray<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);               
            }
            return lstCopy;
        }

        public HashSet<T> CopyToHashSet<T>(ICollection<T> lst)
        {
            HashSet<T> lstCopy = new HashSet<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);                
            }
            return lstCopy;
        }

        public TreeSet<T> CopyToTreeSet<T>(ICollection<T> lst)
        {
            TreeSet<T> lstCopy = new TreeSet<T>();
            foreach (var item in lst)
            {
                lstCopy.Add((T)item);                
            }
            return lstCopy;
        }

        #endregion

        #region Events
        
        protected virtual void OnTransformCompleted( EventArgs e)
        {
            EventHandler handler = TransformCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler TransformCompleted;
        #endregion


    }

    
    
    
}

