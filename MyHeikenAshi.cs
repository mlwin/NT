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
    /// Returns 0 - Unknown or in range
	///         1 - Uptrend
	///         2 - Downtrend
    /// </summary>
    [Description("")]
    public class MyHeikenAshi : Indicator
    {
        #region Variables
        // Wizard generated variables
            private int shadowThreshold = 1; // Default setting for ShadowThreshold
        // User defined variables (add any user defined variables below)
		
		private DataSeries _dsHAOpen;
		private DataSeries _dsHAClose;
		private DataSeries _dsSignal;
		private double     _fHAHigh;
		private double     _fHALow;
        private double     _fShadowAllowedTicks;
		#endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
            Overlay				= true;
			
			_dsHAOpen  = new DataSeries(this);
			_dsHAClose = new DataSeries(this);
			_dsSignal  = new DataSeries(this);
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			_dsSignal.Set(0);
			BackColor = Color.LightYellow;
			
			if (CurrentBar == 0)
			{
				_dsHAClose.Set(0);
				_dsHAOpen.Set(0);
				_fShadowAllowedTicks = shadowThreshold*TickSize;
            	return;
			}
			
			_dsHAClose.Set((Open[0]+High[0]+Low[0]+Close[0])*0.25);
			_dsHAOpen.Set((_dsHAOpen[1]+_dsHAClose[1])*0.5);
			
			_fHAHigh = Math.Max(High[0], _dsHAOpen[0]);
			_fHALow  = Math.Min(Low[0], _dsHAOpen[0]);
			
			if(_dsHAOpen[0] < _dsHAClose[0])
			{
				if(_dsHAOpen[0] == _fHALow ||
				   _dsHAOpen[0]-_fHALow <= _fShadowAllowedTicks)
				{
					//DrawArrowUp("Up"+CurrentBar, true, 0, Low[0]-4*TickSize, Color.Cyan);
					BackColor = Color.LightSeaGreen;
					_dsSignal.Set(1);
				}
			}
			else if(_dsHAOpen[0] > _dsHAClose[0])
			{
				if(_dsHAOpen[0] == _fHAHigh ||
				   _fHAHigh-_dsHAOpen[0] <= _fShadowAllowedTicks)
				{
					//DrawArrowDown("Down"+CurrentBar, true, 0, High[0]+4*TickSize, Color.Orange);
					BackColor = Color.LightSalmon;
					_dsSignal.Set(2);
				}
			}
			
        }

        #region Properties
		
        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries Signal
        {
            get { Update(); return _dsSignal; }
        }			
		
		
        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries Plot0
        {
            get { return Values[0]; }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int ShadowThreshold
        {
            get { return shadowThreshold; }
            set { shadowThreshold = Math.Max(1, value); }
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
        private MyHeikenAshi[] cacheMyHeikenAshi = null;

        private static MyHeikenAshi checkMyHeikenAshi = new MyHeikenAshi();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MyHeikenAshi MyHeikenAshi(int shadowThreshold)
        {
            return MyHeikenAshi(Input, shadowThreshold);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MyHeikenAshi MyHeikenAshi(Data.IDataSeries input, int shadowThreshold)
        {
            if (cacheMyHeikenAshi != null)
                for (int idx = 0; idx < cacheMyHeikenAshi.Length; idx++)
                    if (cacheMyHeikenAshi[idx].ShadowThreshold == shadowThreshold && cacheMyHeikenAshi[idx].EqualsInput(input))
                        return cacheMyHeikenAshi[idx];

            lock (checkMyHeikenAshi)
            {
                checkMyHeikenAshi.ShadowThreshold = shadowThreshold;
                shadowThreshold = checkMyHeikenAshi.ShadowThreshold;

                if (cacheMyHeikenAshi != null)
                    for (int idx = 0; idx < cacheMyHeikenAshi.Length; idx++)
                        if (cacheMyHeikenAshi[idx].ShadowThreshold == shadowThreshold && cacheMyHeikenAshi[idx].EqualsInput(input))
                            return cacheMyHeikenAshi[idx];

                MyHeikenAshi indicator = new MyHeikenAshi();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.ShadowThreshold = shadowThreshold;
                Indicators.Add(indicator);
                indicator.SetUp();

                MyHeikenAshi[] tmp = new MyHeikenAshi[cacheMyHeikenAshi == null ? 1 : cacheMyHeikenAshi.Length + 1];
                if (cacheMyHeikenAshi != null)
                    cacheMyHeikenAshi.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheMyHeikenAshi = tmp;
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
        public Indicator.MyHeikenAshi MyHeikenAshi(int shadowThreshold)
        {
            return _indicator.MyHeikenAshi(Input, shadowThreshold);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Indicator.MyHeikenAshi MyHeikenAshi(Data.IDataSeries input, int shadowThreshold)
        {
            return _indicator.MyHeikenAshi(input, shadowThreshold);
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
        public Indicator.MyHeikenAshi MyHeikenAshi(int shadowThreshold)
        {
            return _indicator.MyHeikenAshi(Input, shadowThreshold);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Indicator.MyHeikenAshi MyHeikenAshi(Data.IDataSeries input, int shadowThreshold)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.MyHeikenAshi(input, shadowThreshold);
        }
    }
}
#endregion
