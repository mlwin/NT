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
    public class MyFibBands : Indicator
    {
        #region Variables
        // Wizard generated variables
            private int periodEMA = 20; // Default setting for PeriodEMA
            private int periodATR = 8; // Default setting for PeriodATR
        // User defined variables (add any user defined variables below)
		
		private double _dEMA;
		private double _dATR;
		
		private double  _dBand1Offset = .68;
		private double  _dBand2Offset = 1.89;
		
		private double _dBand1Range;
		private double _dBand2Range;
		
        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
            Add(new Plot(Color.FromKnownColor(KnownColor.LightSlateGray), PlotStyle.Line, "UpperBand1"));
            Add(new Plot(Color.FromKnownColor(KnownColor.LightSlateGray), PlotStyle.Line, "UpperBand2"));
            Add(new Plot(Color.FromKnownColor(KnownColor.LightSlateGray), PlotStyle.Line, "LowerBand1"));
            Add(new Plot(Color.FromKnownColor(KnownColor.LightSlateGray), PlotStyle.Line, "LowerBand2"));
			
			//Add(new Plot(Color.FromKnownColor(KnownColor.DarkSalmon), PlotStyle.Line, "UpperBand3"));
			//Add(new Plot(Color.FromKnownColor(KnownColor.DarkSalmon), PlotStyle.Line, "LowerBand3"));
			
            Overlay	= true;
			DrawOnPricePanel    = true;
			PaintPriceMarkers   = false;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
          	_dEMA = SMA(EMA(periodEMA), 3)[0];
			_dATR = SMA(ATR(periodATR), periodEMA)[0];
			
			_dBand1Range = _dBand1Offset*_dATR;
			_dBand2Range = _dBand2Offset*_dATR;
			
            UpperBand1.Set(_dEMA + _dBand1Range);
            UpperBand2.Set(_dEMA + _dBand2Range);
			//UpperBand3.Set(_dEMA + (4.23*_dATR));
			
            LowerBand1.Set(_dEMA - _dBand1Range);
            LowerBand2.Set(_dEMA - _dBand2Range);
			//LowerBand3.Set(_dEMA - (4.23*_dATR));
        }

        #region Properties
        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries UpperBand1
        {
            get { return Values[0]; }
        }

        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries UpperBand2
        {
            get { return Values[1]; }
        }

        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries LowerBand1
        {
            get { return Values[2]; }
        }

        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries LowerBand2
        {
            get { return Values[3]; }
        }
		
        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries UpperBand3
        {
            get { return Values[4]; }
        }

        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries LowerBand3
        {
            get { return Values[5]; }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int PeriodEMA
        {
            get { return periodEMA; }
            set { periodEMA = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int PeriodATR
        {
            get { return periodATR; }
            set { periodATR = Math.Max(1, value); }
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
        private MyFibBands[] cacheMyFibBands = null;

        private static MyFibBands checkMyFibBands = new MyFibBands();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MyFibBands MyFibBands(int periodATR, int periodEMA)
        {
            return MyFibBands(Input, periodATR, periodEMA);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MyFibBands MyFibBands(Data.IDataSeries input, int periodATR, int periodEMA)
        {
            if (cacheMyFibBands != null)
                for (int idx = 0; idx < cacheMyFibBands.Length; idx++)
                    if (cacheMyFibBands[idx].PeriodATR == periodATR && cacheMyFibBands[idx].PeriodEMA == periodEMA && cacheMyFibBands[idx].EqualsInput(input))
                        return cacheMyFibBands[idx];

            lock (checkMyFibBands)
            {
                checkMyFibBands.PeriodATR = periodATR;
                periodATR = checkMyFibBands.PeriodATR;
                checkMyFibBands.PeriodEMA = periodEMA;
                periodEMA = checkMyFibBands.PeriodEMA;

                if (cacheMyFibBands != null)
                    for (int idx = 0; idx < cacheMyFibBands.Length; idx++)
                        if (cacheMyFibBands[idx].PeriodATR == periodATR && cacheMyFibBands[idx].PeriodEMA == periodEMA && cacheMyFibBands[idx].EqualsInput(input))
                            return cacheMyFibBands[idx];

                MyFibBands indicator = new MyFibBands();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.PeriodATR = periodATR;
                indicator.PeriodEMA = periodEMA;
                Indicators.Add(indicator);
                indicator.SetUp();

                MyFibBands[] tmp = new MyFibBands[cacheMyFibBands == null ? 1 : cacheMyFibBands.Length + 1];
                if (cacheMyFibBands != null)
                    cacheMyFibBands.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheMyFibBands = tmp;
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
        public Indicator.MyFibBands MyFibBands(int periodATR, int periodEMA)
        {
            return _indicator.MyFibBands(Input, periodATR, periodEMA);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Indicator.MyFibBands MyFibBands(Data.IDataSeries input, int periodATR, int periodEMA)
        {
            return _indicator.MyFibBands(input, periodATR, periodEMA);
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
        public Indicator.MyFibBands MyFibBands(int periodATR, int periodEMA)
        {
            return _indicator.MyFibBands(Input, periodATR, periodEMA);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Indicator.MyFibBands MyFibBands(Data.IDataSeries input, int periodATR, int periodEMA)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.MyFibBands(input, periodATR, periodEMA);
        }
    }
}
#endregion
