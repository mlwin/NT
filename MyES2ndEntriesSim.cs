#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Indicator;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Strategy;
#endregion

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    /// <summary>
    /// Enter on 2nd entry signal.
    /// </summary>
    [Description("Enter on 2nd entry signal.")]
    public class MyES2ndEntriesSim : Strategy
    {
        #region Variables
		private string _strName       = @"MyES2ndEntriesSim";
		
        // Wizard generated variables
        private int      = 4; // Default setting for     
        private int nSL = 8; // Default setting for NSL
		private int nFastFilter1 = 3;
		private int nSlowFilter1 = 20;
		
		double htfSMA   = 0.0;
		double htfEMA   = 0.0;
		
        // User defined variables (add any user defined variables below)
        #endregion

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
			Add(PeriodType.Minute, 5);
            SetProfitTarget("", CalculationMode.Ticks,     );
            //SetStopLoss("", CalculationMode.Ticks, NSL, false);

            CalculateOnBarClose = false;
			
			BarsRequired    = 20;
			DefaultQuantity = 1;
			EntriesPerDirection = 1;
    		EntryHandling = EntryHandling.AllEntries; 
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			#region TimeFilter
				
				// Don't trade before 9:45AM EST
				if(ToTime(Time[0]) < 94500)
					return;
				
				// Don't trade after 4PM
				if(ToTime(Time[0]) >= 160000)
					return;
				
			#endregion
				
			#region ExitBE
            if(Position.MarketPosition != MarketPosition.Flat &&
               BarsSinceEntry(0, "", 0) > 6)
            {
                SetProfitTarget("", CalculationMode.Ticks, 1);
            }
            #endregion
				
			if (BarsInProgress == 1)
			{
				htfSMA = SMA(200)[0];
				htfEMA = EMA(20)[0];
			}
			
			if(BarsInProgress           == 0 &&
			   Position.MarketPosition  == MarketPosition.Flat &&
			   ((BarsSinceExit(0, "", 0) >2)||(BarsSinceExit(0, "", 0)== -1)))
			{				
				// Condition set 1
				if (htfEMA > htfSMA   &&
					My2ndEntries(4, 10, "", 20).Signal[0] == 1 &&
					EMA(19)[0] > EMA(50)[0])
				{
					SetProfitTarget("", CalculationMode.Ticks,     );
					EnterLong(1, "");
				}
                // Condition set 2
                else if (htfEMA < htfSMA   &&
                        My2ndEntries(4, 10, "", 20).Signal[0] == 2 &&
					    EMA(19)[0] < EMA(50)[0])
                {
					SetProfitTarget("", CalculationMode.Ticks,     );
                    EnterShort(1, "");
                }
            }
        }
        
        protected override void OnExecution(IExecution execution)
        {
        }
        
        protected override void OnTermination() 
        {
            //MyFileWriter.Instance.Terminate();
        }
        
        private void MyPrint(string str)
        {
            PrintWithTimeStamp(_strName + "> " + str + "\n");
        }
        
        private double RoundPrice(double value)
        {
            return Bars.Instrument.MasterInstrument.Round2TickSize(value);
        }

        #region Properties
        [Description("")]
        [GridCategory("Parameters")]
        public int FastFilter1
        {
            get { return nFastFilter1; }
            set { nFastFilter1 = Math.Max(1, value); }
            
        }
        
        [Description("")]
        [GridCategory("Parameters")]
        public int SlowFilter1
        {
            get { return nSlowFilter1; }
            set { nSlowFilter1 = Math.Max(1, value); }
            
        }
        
        [Description("")]
        [GridCategory("Parameters")]
        public int     
        {
            get { return     ; }
            set {      = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int NSL
        {
            get { return nSL; }
            set { nSL = Math.Max(1, value); }
        }
        #endregion
    }
}
