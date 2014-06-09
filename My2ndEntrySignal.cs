#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Chart;
#endregion

// ************************************************************************************
//
// This code is provided on an "AS IS" basis, without warranty of any kind,
// including without limitation the warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//
// Copyright (c) 2014
// mlwin1@yahoo.com
//
// ************************************************************************************

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    /// <summary>
    /// Finds first and second entries 'like' signals as describe in various price action illustrations.
    /// </summary>
    [Description("Enter the description of your new custom indicator here")]
    public class My2ndEntrySignal : Indicator
    {
        public const int eStateInitial       = 0;
        public const int eStateCount0        = 1;
        public const int eStateCount0Retrace = 2;
        public const int eStateCount1        = 3;
        public const int eStateCount1Retrace = 4;
        public const int eStateCount2        = 5;

        #region Variables
        // Wizard generated variables
            private int threshold = 20; // Default setting for Threshold
        // User defined variables (add any user defined variables below)

            private int longEntryState   = eStateInitial;
            private int shortEntryState  = eStateInitial;

            private double highest = 0.0;
            private double lowest  = 0.0;

            private int _lastBarPlayed = 0;

            private int _lastUp2ndEntryBar   = 0;
            private int _lastDown2ndEntryBar = 0;
            private int _lastUp1stEntryBar   = 0;
            private int _lastDown1stEntryBar = 0;

            private DataSeries _signal;


        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
            longEntryState   = eStateInitial;
            shortEntryState  = eStateInitial;
            _lastBarPlayed   = 0;
            Overlay             = true;
            CalculateOnBarClose = false;

            _signal             = new DataSeries(this);

        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
            if(CurrentBar < threshold)
                return;

            highest = MAX(High, threshold)[1];
            lowest  = MIN(Low, threshold)[1];

            _signal.Set(0);

            if(Close[0] > highest ||
               High[0]  > highest)
            {
                longEntryState  = eStateCount0;
                shortEntryState = eStateCount0;
            }

            if(Close[0] < lowest ||
               High[0]  < lowest)
            {
                shortEntryState = eStateCount0;
                longEntryState  = eStateCount0;
            }

            switch(longEntryState)
            {
                case eStateCount0:
                    if(Close[0] < Low[1] || Low[0] < Low[1])
                    {
                        longEntryState = eStateCount0Retrace;
                    }
                    break;

                case eStateCount0Retrace:
                    if(Close[0] > High[1] || High[0] > High[1])
                    {
                        //1st entry
                        longEntryState = eStateCount1;
                        _lastUp1stEntryBar = CurrentBar;
                    }
                    break;

                case eStateCount1:    
                    if(Close[0] < Low[1] || Low[0] < Low[1])
                    {
                        longEntryState = eStateCount1Retrace;
                    }
                    break;

                case eStateCount1Retrace:
                    if(Close[0] > High[1] || High[0]  > High[1])
                    {
                        longEntryState = eStateCount2;    
                        _lastUp2ndEntryBar = CurrentBar;
                    }
                    break;

                case eStateCount2:                
                    if(Close[0] < Low[1] || Low[0] < Low[1])
                    {
                        longEntryState = eStateCount1Retrace;
                    }
                    break;
            }

            switch(shortEntryState)
            {
                case eStateCount0:
                    if(Close[0] > High[1] || High[0] > High[1])
                    {
                        shortEntryState = eStateCount0Retrace;
                    }
                    break;

                case eStateCount0Retrace:
                    if(Close[0] < Low[1] || Low[0] < Low[1])
                    {
                        //1st entry
                        shortEntryState = eStateCount1;
                        _lastDown1stEntryBar = CurrentBar;
                    }
                    break;

                case eStateCount1:    
                    if(Close[0] > High[1] || High[0] > High[1])
                    {
                        shortEntryState = eStateCount1Retrace;
                    }
                    break;

                case eStateCount1Retrace:
                    if(Close[0] < Low[1] || Low[0]  < Low[1])
                    {
                        shortEntryState = eStateCount2;    
                        _lastDown2ndEntryBar = CurrentBar;
                    }
                    break;

                case eStateCount2:                
                    if(Close[0] > High[1] || High[0] > High[1])
                    {
                        shortEntryState = eStateCount1Retrace;
                    }
                    break;

            }

            #region SetSignals
                if(_lastUp2ndEntryBar == CurrentBar)
                {
                    //DrawArrowUp("up2nd"+CurrentBar, true, 0, Low[0]-_icon1Offset*TickSize, Color.LightSeaGreen);
                    //DrawText("up2nd"+CurrentBar, "U2", 0, Low[0]-_icon1Offset*TickSize, Color.White);
                    _signal.Set(1);
                }
                else if(_lastUp1stEntryBar == CurrentBar)
                {
                    //DrawTriangleUp("up1nd"+CurrentBar, true, 0, Low[0]-_icon1Offset*TickSize, Color.LightSeaGreen);
                    //DrawText("up1nd"+CurrentBar, "U1", 0, Low[0]-_icon1Offset*TickSize, Color.White);
                }

                if(_lastDown2ndEntryBar == CurrentBar)
                {
                    //DrawArrowDown("d2nd"+CurrentBar, true, 0, High[0]+_icon1Offset*TickSize, Color.OrangeRed);
                    //DrawText("d2nd"+CurrentBar, "D2", 0, High[0]+_icon1Offset*TickSize, Color.White);

                    _signal.Set(2);
                }
                else if(_lastDown1stEntryBar == CurrentBar)
                {
                    //DrawTriangleDown("d1st"+CurrentBar, true, 0, High[0]+_icon1Offset*TickSize, Color.OrangeRed);
                    //DrawText("d1st"+CurrentBar, "D1", 0, High[0]+_icon1Offset*TickSize, Color.White);
                }
            #endregion

            /*
            DrawTextFixed("arming", " ", TextPosition.TopRight, Color.Black, new Font("Tahoma", 14), Color.Empty, Color.Empty, 10);

            if(longEntryState == eStateCount1Retrace)
            {
                DrawTextFixed("arming", "L arming", TextPosition.TopRight, Color.Black, new Font("Tahoma", 14), Color.Empty, Color.Empty, 10);
            }

            if(shortEntryState == eStateCount1Retrace)
            {
                DrawTextFixed("arming", "S arming", TextPosition.TopRight, Color.Black, new Font("Tahoma", 14), Color.Empty, Color.Empty, 10);
            }

            if(longEntryState == eStateCount1Retrace && shortEntryState == eStateCount1Retrace)
            {
                DrawTextFixed("arming", "L arming / S arming", TextPosition.TopRight, Color.Black, new Font("Tahoma", 14), Color.Empty, Color.Empty, 10);
            }
            */
        }

        #region Properties

        
        [Description("")]
        [GridCategory("Parameters")]
        public int Threshold
        {
            get { return threshold; }
            set { threshold = Math.Max(1, value); }
        }
        
        
        [Browsable(false)]    // this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]        // this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries Signal
        {
            get { Update(); return _signal; }
        }    
        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.
// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    public partial class Indicator : IndicatorBase
    {
        private My2ndEntrySignal[] cacheMy2ndEntrySignal = null;

        private static My2ndEntrySignal checkMy2ndEntrySignal = new My2ndEntrySignal();

        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        public My2ndEntrySignal My2ndEntrySignal(int threshold)
        {
            return My2ndEntrySignal(Input, threshold);
        }

        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        public My2ndEntrySignal My2ndEntrySignal(Data.IDataSeries input, int threshold)
        {
            if (cacheMy2ndEntrySignal != null)
                for (int idx = 0; idx < cacheMy2ndEntrySignal.Length; idx++)
                    if (cacheMy2ndEntrySignal[idx].Threshold == threshold && cacheMy2ndEntrySignal[idx].EqualsInput(input))
                        return cacheMy2ndEntrySignal[idx];

            lock (checkMy2ndEntrySignal)
            {
                checkMy2ndEntrySignal.Threshold = threshold;
                threshold = checkMy2ndEntrySignal.Threshold;

                if (cacheMy2ndEntrySignal != null)
                    for (int idx = 0; idx < cacheMy2ndEntrySignal.Length; idx++)
                        if (cacheMy2ndEntrySignal[idx].Threshold == threshold && cacheMy2ndEntrySignal[idx].EqualsInput(input))
                            return cacheMy2ndEntrySignal[idx];

                My2ndEntrySignal indicator = new My2ndEntrySignal();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.Threshold = threshold;
                Indicators.Add(indicator);
                indicator.SetUp();

                My2ndEntrySignal[] tmp = new My2ndEntrySignal[cacheMy2ndEntrySignal == null ? 1 : cacheMy2ndEntrySignal.Length + 1];
                if (cacheMy2ndEntrySignal != null)
                    cacheMy2ndEntrySignal.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheMy2ndEntrySignal = tmp;
                return indicator;
            }
        }
    }
}

// This namespace holds all market analyzer column definitions and is required. Do not change it.
namespace NinjaTrader.MarketAnalyzer
{
    public partial class Column : ColumnBase
    {
        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.My2ndEntrySignal My2ndEntrySignal(int threshold)
        {
            return _indicator.My2ndEntrySignal(Input, threshold);
        }

        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        public Indicator.My2ndEntrySignal My2ndEntrySignal(Data.IDataSeries input, int threshold)
        {
            return _indicator.My2ndEntrySignal(input, threshold);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.My2ndEntrySignal My2ndEntrySignal(int threshold)
        {
            return _indicator.My2ndEntrySignal(Input, threshold);
        }

        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        public Indicator.My2ndEntrySignal My2ndEntrySignal(Data.IDataSeries input, int threshold)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.My2ndEntrySignal(input, threshold);
        }
    }
}
#endregion
