using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO.Ports;

namespace WattsUpFramework
{
    ///Partial Developed by: Junya Michanan
    ///Computer Science Department, University of Denver, Denver, Colorado USA 80210
    ///The original code was developed by ? http://grouplab.cpsc.ucalgary.ca/cookbook/index.php/Toolkits/WattsUpComponent
    ///
    public delegate void WuReadingEventHandler(object sender, WuReadingEventArgs e);

    /// <summary>
    /// This event is raised whenever the WattsUp returns a data reading. WUData is returned as an event argument.
    /// </summary>
    public class WuReadingEventArgs : EventArgs
    {
        public readonly WUData wuData;
        public WuReadingEventArgs(WUData wudata)
        {
            this.wuData = wudata;
        }
    }

    public class WattsUp
    {
        #region Variables
        private SerialPort sp; //The serial port (actually a USB connection to the WattsUp that appears as a serial port) 
        private int BAUD = 115200; //Baud rate (bps)
        private int DATABITS = 8;  //Number of data bits
        private int READTIMEOUT = 500; //If we do a read and nothing is returned in this amount of time, then timeout.
        #endregion

        #region Properties

        private string _Port;
        public string Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        public string[] AvailablePorts
        {
            get { return SerialPort.GetPortNames(); }
        }

        private int _InternalSamplingRate = 1;
        [DefaultValue("1")]
        public int InternalSamplingRate
        {
            get {return _InternalSamplingRate;}
            set {
                if (value < 1)
                    value = 1;
                else if (value > 3600)
                    value = 3600;
                _InternalSamplingRate = value;                    
            }
        }

        private int _SamplingRate = 10;
        [DefaultValue("10")]
        public int SamplingRate
        {
            get { return _SamplingRate; }
            set
            {
                if (value < 1) value = 1;
                if (value > 3600) value = 3600;
                _SamplingRate = value;
            }
        }

        [DefaultValue("true")]
        public bool IsUseWattsDelta
        {
            get;
            set;
        }

        private int _WattsThreshold = 5;
        [DefaultValue("10")]
        public int WattsThreshold
        {
            get { return _WattsThreshold; }
            set
            {
                if (value < 0) value = 0;
                if (value > 3600) value = 3600;
                _WattsThreshold = value;
            }
        }

        //private WUData _LastReadWUData;
        //public WUData LastReadWattData
        //{
        //    get { return _LastReadWUData; }
        //    set { _LastReadWUData = value; }
        //}
        #endregion

        #region Public Methods
        /// <summary> Connect to the WattsUp and start recording. Returns true on success, else false  </summary>  
        public bool Start(out string err)
        {
            err = string.Empty;
           // LastReadWattData = new WUData();

            if (AvailablePorts.Count() > 0)
            {
                //Set the default port to the first available open port in the computer
                if(string.IsNullOrEmpty(this.Port))
                    this.Port = AvailablePorts.FirstOrDefault();
                                
                try
                {
                    //Initialize the serial port
                    this.sp = new SerialPort(this.Port, this.BAUD, System.IO.Ports.Parity.None, this.DATABITS);
                    this.sp.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceived);

                    //Open the serial port, set a read timeout on the serial port, 
                    //ans then command the WattsUp to return data for the specified internal sampling rate.

                    this.sp.Open();
                    if (this.sp.IsOpen)
                    {
                        this.sp.ReadTimeout = this.READTIMEOUT;
                        if (this.Write(WUCommands.ExternalModeStartString + this.InternalSamplingRate.ToString() + WUCommands.ExternalModeEndString) == "") return false;
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    return false;
                }

            }
            else
            {
                err = "No open serial port found!!!";
                return false;
            }
        }

        /// <summary> Stop recording of data and disconnect from the WattsUp. Return true if successful. </summary>  
        public bool Stop()
        {
            //We stop everything by closing the serial port
            this.sp.Close();
            return (!this.sp.IsOpen);
        }

        /// <summary> 
        /// Read the Watts Up. Returns "" if no reading is available, or if cannot do it. 
        /// Note that you shouldn't have to do this call as readings are raised as events. 
        /// I just include it mostly for debugging
        /// </summary>  
        public string GetReading()
        {
            return (TrimPacket(this.Read()));
        }

        /// <summary> Get the version of WattsUp device. Returns "" if we can't get it. </summary>  
        public string GetVersion()
        {
            string result = "";
            if (this.Write(WUCommands.VersionRequest) == "") return "";
            result = this.Read();
            return (result);
        }

        public bool ResetMemory(out string err)
        {
            err = string.Empty;

            this.sp = new SerialPort(this.Port, this.BAUD, System.IO.Ports.Parity.None, this.DATABITS);

            try
            {
                this.sp.Open();
                if (this.sp.IsOpen)
                {
                    if (this.Write(WUCommands.ResetDataMemory) == "")
                        return false;
                    else
                    {
                        this.sp.Close();
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
        }
              
        #endregion

        #region Private Methods

        /// <summary>
        /// Get a ','-delimited argument from the string. Return "" if you cannot.
        /// Note that this isn't the most efficient way to do this if we walk the entire string
        /// using successive calls to extract all the data from a packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private string GetArg(string packet, int position)
        {
            packet = this.TrimPacket(packet);
            string[] sarray;
            sarray = packet.Split(',');
            if (position < sarray.Length)
                return sarray.GetValue(position).ToString();
            else
                return "";
        }

        /// Get the packet type, returned as a single letter
        private string GetPacketType(string packet)
        {
            return (this.GetArg(packet, 0));
        }

        //Remove extraneous packet-delimiting characters plus white space from the packet
        private string TrimPacket(string packet)
        {
            char[] chStart = { '\r', '\n', '#' };
            char[] chEnd = { ';' };
            packet = packet.TrimStart(chStart);
            packet = packet.TrimEnd(chEnd);
            return packet;
        }

        /// Read a packet from the WattsUp on the serial port. Return "" if it times out.
        /// Note that WattsUp packets should always end with a ';'
        private string Read()
        {
            char ch = ' ';
            string str = "";

            try
            {
                //All packets end with a semi-colon
                while (ch != ';')
                {
                    ch = Convert.ToChar(sp.ReadChar());
                    str += Convert.ToString(ch);
                }
                return (str);
            }
            catch
            {
                return "";
            }
        }

        /// Write the string to the serial port. 
        /// Return the string (i.e., echo it back) if successful, and "" if it can't be written
        private string Write(string str)
        {
            if (this.sp.IsOpen)
            {
                this.sp.Write(str);
                return (str);
            }
            else return "";
        }

        /// Populate the data structure by parsing the v. 

        private WUData Populate(string packet)
        {
            WUData wd = new WUData();
            wd.Record = packet;
            wd.Time = DateTime.Now;
            wd.Watts = StringToDouble(GetArg(packet, 3));
            if (wd.Watts != -1) wd.Watts = wd.Watts / 10.0;
            wd.Volts = DivideByTen(GetArg(packet, 4));
            wd.Amps = DivideByTen(GetArg(packet, 5));
            wd.WattHours = DivideByTen(GetArg(packet, 6));
            wd.Cost = DivideByTen(GetArg(packet, 7));
            wd.WattHoursPerMonth = DivideByTen(GetArg(packet, 8));
            wd.CostPerMonth = DivideByTen(GetArg(packet, 9));
            wd.WattMax = DivideByTen(GetArg(packet, 10));
            wd.WattMin = DivideByTen(GetArg(packet, 13));
            return wd;
        }


        /// Convert the string into a double, if it is a number. 
        /// Otherwise return -1     
        private double StringToDouble(string str)
        {
            try
            {
                double value = Convert.ToDouble(str);
                return (value);
            }
            catch
            {
                return (-1);
            }
        }

        private string DivideByTen(string value)
        {
            decimal d = 0;
            if (Decimal.TryParse(value, out d))
            {
                return Convert.ToString(d/ 10);
            }
            return d.ToString();
        }

        #endregion

        #region Events        

        /// <summary> Event raised whenever a new reading arrives from the WattsUp</summary>  
        [Description("Event raised whenever a new reading arrives from the WattsUp")]
        public event WuReadingEventHandler OnWuReading;


        /// <summary> Event invoked whenever a new reading arrives from the WattsUp</summary>  
        [Description("Event invoked whenever a new reading arrives from the WattsUp")]
        protected virtual void wu_OnWuReading(WUData wu)
        {
            RaiseEvent(this.OnWuReading, (object)this, new WuReadingEventArgs(wu));
        }

        /// <summary> Raise the event </summary>  
        internal static void RaiseEvent(MulticastDelegate evt, object sender, WuReadingEventArgs e)
        {
            try
            {
                if (null != evt)
                {
                    object[] args = new object[] { sender, e };
                    foreach (MulticastDelegate d in evt.GetInvocationList())
                    {
                        //if (d.Target is System.Windows.Forms.Control)
                        //{
                        //    Control c = d.Target as System.Windows.Forms.Control;
                        //    c.Invoke(d, args);
                        //}
                        //else
                        //{
                            d.DynamicInvoke(args);
                        //}
                    }
                }
            }
            catch { }
        }

        #endregion


        #region Event Handlers
        /// When data is available on the serial port, read and process the packet.
        /// Raise an event only if sufficient time has passed, or if the delta is activated
        /// and greater than the threshold
        private long last_raised_event_time = 0;
        private double last_reading = 0;
        private double last_internal_sampling_rate = -1;
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string result = "";
            result = TrimPacket(this.Read());
            if (this.GetPacketType(result) == "d")
            {
                WUData wu = new WUData();
                wu = this.Populate(result);

                //Now decide if we need to raise the event
                long current_time = DateTime.Now.Ticks / 10000000;
                bool raise = false;

                // If the time passed is greater than the sampling rate, or if
                // the difference in watts since the last recorded reading is greater than
                // the threshold, then we will want to raise an event
                long time_delta = System.Math.Abs(current_time - last_raised_event_time);
                if (time_delta >= this.SamplingRate)
                {
                    raise = true;
                }
                else
                {
                    double watt_delta = System.Math.Abs(wu.Watts - last_reading);
                    if (this.IsUseWattsDelta && watt_delta >= this.WattsThreshold)

                        raise = true;
                }
                // Raise the event, and set the time and watts reading to the current values
                if (raise)
                {
                    this.wu_OnWuReading(wu);
                    last_raised_event_time = current_time;
                    last_reading = wu.Watts;
                }

                // Now check to see if the InternalSamplingRate has changed since the last time we
                // looked. If it has, reset it.

                if (last_internal_sampling_rate == -1) last_internal_sampling_rate = this.InternalSamplingRate;
                else
                {
                    double sampling_delta = last_internal_sampling_rate - this.InternalSamplingRate;
                    if (sampling_delta != 0 && sp.IsOpen == true)
                    {
                        this.Write(WUCommands.ExternalModeStartString + this.InternalSamplingRate.ToString() + WUCommands.ExternalModeEndString);
                    }
                }
                
            }
        }


        #endregion
    }
}
