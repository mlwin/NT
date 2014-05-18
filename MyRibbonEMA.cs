#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
// Copyright (c) 2012
// mlwin1@yahoo.com
//
// ************************************************************************************

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public class MyRibbonEMA : Indicator
    {
        #region Variables
        // Wizard generated variables
            private int length1 = 20; // Default setting for Length1
            private int length2 = 30; // Default setting for Length2
            private int length3 = 40; // Default setting for Length3
            private int length4 = 50; // Default setting for Length4
        // User defined variables (add any user defined variables below)
        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
			/*
			F7D917
			EDA636
			E37354
			D94073
			CF0D91
			*/
			
            Add(new Plot(System.Drawing.ColorTranslator.FromHtml("#EDA636"), PlotStyle.Line, "MA1"));
            Add(new Plot(System.Drawing.ColorTranslator.FromHtml("#E37354"), PlotStyle.Line, "MA2"));
            Add(new Plot(System.Drawing.ColorTranslator.FromHtml("#D94073"), PlotStyle.Line, "MA3"));
            Add(new Plot(System.Drawing.ColorTranslator.FromHtml("#CF0D91"), PlotStyle.Line, "MA4"));
            Overlay				= true;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
            // Use this method for calculating your indicator values. Assign a value to each
            // plot below by replacing 'Close[0]' with your own formula.
            MA1.Set(EMA(length1)[0]);
            MA2.Set(EMA(length2)[0]);
            MA3.Set(EMA(length3)[0]);
            MA4.Set(EMA(length4)[0]);
        }

        #region Properties
        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries MA1
        {
            get { return Values[0]; }
        }

        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries MA2
        {
            get { return Values[1]; }
        }

        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries MA3
        {
            get { return Values[2]; }
        }

        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries MA4
        {
            get { return Values[3]; }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int Length1
        {
            get { return length1; }
            set { length1 = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int Length2
        {
            get { return length2; }
            set { length2 = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int Length3
        {
            get { return length3; }
            set { length3 = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int Length4
        {
            get { return length4; }
            set { length4 = Math.Max(1, value); }
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
        private MyRibbonEMA[] cacheMyRibbonEMA = null;

        private static MyRibbonEMA checkMyRibbonEMA = new MyRibbonEMA();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MyRibbonEMA MyRibbonEMA(int length1, int length2, int length3, int length4)
        {
            return MyRibbonEMA(Input, length1, length2, length3, length4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MyRibbonEMA MyRibbonEMA(Data.IDataSeries input, int length1, int length2, int length3, int length4)
        {
            if (cacheMyRibbonEMA != null)
                for (int idx = 0; idx < cacheMyRibbonEMA.Length; idx++)
                    if (cacheMyRibbonEMA[idx].Length1 == length1 && cacheMyRibbonEMA[idx].Length2 == length2 && cacheMyRibbonEMA[idx].Length3 == length3 && cacheMyRibbonEMA[idx].Length4 == length4 && cacheMyRibbonEMA[idx].EqualsInput(input))
                        return cacheMyRibbonEMA[idx];

            lock (checkMyRibbonEMA)
            {
                checkMyRibbonEMA.Length1 = length1;
                length1 = checkMyRibbonEMA.Length1;
                checkMyRibbonEMA.Length2 = length2;
                length2 = checkMyRibbonEMA.Length2;
                checkMyRibbonEMA.Length3 = length3;
                length3 = checkMyRibbonEMA.Length3;
                checkMyRibbonEMA.Length4 = length4;
                length4 = checkMyRibbonEMA.Length4;

                if (cacheMyRibbonEMA != null)
                    for (int idx = 0; idx < cacheMyRibbonEMA.Length; idx++)
                        if (cacheMyRibbonEMA[idx].Length1 == length1 && cacheMyRibbonEMA[idx].Length2 == length2 && cacheMyRibbonEMA[idx].Length3 == length3 && cacheMyRibbonEMA[idx].Length4 == length4 && cacheMyRibbonEMA[idx].EqualsInput(input))
                            return cacheMyRibbonEMA[idx];

                MyRibbonEMA indicator = new MyRibbonEMA();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.Length1 = length1;
                indicator.Length2 = length2;
                indicator.Length3 = length3;
                indicator.Length4 = length4;
                Indicators.Add(indicator);
                indicator.SetUp();

                MyRibbonEMA[] tmp = new MyRibbonEMA[cacheMyRibbonEMA == null ? 1 : cacheMyRibbonEMA.Length + 1];
                if (cacheMyRibbonEMA != null)
                    cacheMyRibbonEMA.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheMyRibbonEMA = tmp;
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.MyRibbonEMA MyRibbonEMA(int length1, int length2, int length3, int length4)
        {
            return _indicator.MyRibbonEMA(Input, length1, length2, length3, length4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Indicator.MyRibbonEMA MyRibbonEMA(Data.IDataSeries input, int length1, int length2, int length3, int length4)
        {
            return _indicator.MyRibbonEMA(input, length1, length2, length3, length4);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.MyRibbonEMA MyRibbonEMA(int length1, int length2, int length3, int length4)
        {
            return _indicator.MyRibbonEMA(Input, length1, length2, length3, length4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Indicator.MyRibbonEMA MyRibbonEMA(Data.IDataSeries input, int length1, int length2, int length3, int length4)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.MyRibbonEMA(input, length1, length2, length3, length4);
        }
    }
}
#endregion
