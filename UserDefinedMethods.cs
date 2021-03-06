#region Using declarations
using System;
using System.ComponentModel;
using System.Drawing;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Indicator;
using NinjaTrader.Strategy;
using System.IO;
#endregion

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    /// <summary>
    /// This file holds all user defined strategy methods.
    /// </summary>
    partial class Strategy
    {
		protected int lastBarInTrade = 0;
		
        #region MyFileWriterClass
        public class MyFileWriter
        {
            // NOTE: Need to include -- using System.IO;
            
            private static MyFileWriter _instance = new MyFileWriter();
            private static string path = "C:\\trades\\data.csv";
            private static System.IO.StreamWriter sw;
            
            private MyFileWriter(){}
            
            public static MyFileWriter Instance
            {
                get
                {
                    if(_instance == null)
                    {
                        _instance = new MyFileWriter();
                    }
                    return _instance;
                }
            }
            
            public void SetPath(string p)
            {
                path = p;
            }
            
            // Need to call this from OnTermination
            public void Terminate()
            {
                if(sw != null)
                {
                    sw.Dispose();
                    sw = null;
                }    
            }
                        
            public void WriteLine(String data)
            {				
                if(sw == null)
                    sw = new StreamWriter(path, true);
                
                sw.WriteLine(data);
				sw.Flush();
            }
        }
        #endregion
    
        // Example: pass in DayOfWeek.Monday
        public bool IsCurrentDay(DayOfWeek day)
        {
            DayOfWeek today = DateTime.Today.DayOfWeek;
            
            if(today == day)
                return true;
            
            return false;
        }
        
        // Example: 74500 = 7:45AM, 134500 = 1:45PM
        public bool IsDayTime(DayOfWeek day, int startTime, int endTime)
        {
            if(IsCurrentDay(day) &&
               ToTime(Time[0]) >= startTime && 
               ToTime(Time[0]) <= endTime)
                return true;
            
            return false;
        }
        
        public string CurrentBarDateTimeStr()
        {
            DateTime barDT = Time[0];
            return barDT.ToString("MM/dd/yyyy HH:mm");
        }
		
		public double RoundPrice(double value)
        {
            return Bars.Instrument.MasterInstrument.Round2TickSize(value);
        }
    }
}
