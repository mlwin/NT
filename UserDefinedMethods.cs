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
            #region MyFileWriterClass
            public class MyFileWriter
            {
                // NOTE: Need to include -- using System.IO;
                
                private static MyFileWriter _instance;
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
                        sw = new StreamWriter(path);
                    
                    sw.WriteLine(data);
                }
            }
        #endregion
    }
}
