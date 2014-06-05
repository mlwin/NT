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

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    /// <summary>
    /// Enter the description of your new custom indicator here
    /// </summary>
    [Description("Signals on reversal to trend direction.")]
    public class MyReversal2Trend : Indicator
    {
        #region Variables
        // Wizard generated variables
            private int period1 = 19; // Default setting for Period1
            private int period2 = 55; // Default setting for Period2
        // User defined variables (add any user defined variables below)
        
        private bool   _bPlaySoundLong  = true;
        private bool   _bPlaySoundShort = true;
        private int    _lastBarPlayed = 0;
        private string _soundFile = @"C:\Sounds\AlertAlert.wav";
        
        private DataSeries _signal;
        
        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
            Overlay                = true;
            _signal             = new DataSeries(this);
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {    
            if(CurrentBar<period1 || CurrentBar<period2)
                return;
            
            if(FirstTickOfBar)
            {
                _signal.Set(0);
            }
            
            if(EMA(period1)[0] > EMA(period2)[0] &&
               Open[0] < Close[0] &&
               Open[1] > Close[1])
            {
                DrawArrowUp("TUp"+CurrentBar, true, 0, Low[0]-2*TickSize, Color.Cyan);
                if(_bPlaySoundLong && (CurrentBar > _lastBarPlayed+1))
                {
                    _lastBarPlayed = CurrentBar;
                    PlaySound(_soundFile);
                    _signal.Set(1);
                }
            }
            else if(EMA(period1)[0] < EMA(period2)[0] &&
                    Open[0] > Close[0] &&
                    Open[1] < Close[1]
                )
            {
                DrawArrowDown("TDown"+CurrentBar, true, 0, High[0]+2*TickSize, Color.Orange);
                
                if(_bPlaySoundShort && (CurrentBar > _lastBarPlayed+1))
                {
                    _lastBarPlayed = CurrentBar;
                    PlaySound(_soundFile);
                    _signal.Set(2);
                }
            }
            
        }

        #region Properties

        [Description("")]
        [GridCategory("Parameters")]
        public int Period1
        {
            get { return period1; }
            set { period1 = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int Period2
        {
            get { return period2; }
            set { period2 = Math.Max(1, value); }
        }
        
        [Description("")]
        [GridCategory("Parameters")]
        public bool PlaySoundLong
        {
            get { return _bPlaySoundLong; }
            set { _bPlaySoundLong = value; }
        }
        
        [Description("")]
        [GridCategory("Parameters")]
        public bool PlaySoundShort
        {
            get { return _bPlaySoundShort; }
            set { _bPlaySoundShort = value; }
        }
        
        [Description("Sound file for alert.")]
        [GridCategory("Parameters")]
        public string SoundFile
        {
            get { return _soundFile; }
            set { _soundFile = value; }
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
        private MyReversal2Trend[] cacheMyReversal2Trend = null;

        private static MyReversal2Trend checkMyReversal2Trend = new MyReversal2Trend();

        /// <summary>
        /// Signals on reversal to trend direction.
        /// </summary>
        /// <returns></returns>
        public MyReversal2Trend MyReversal2Trend(int period1, int period2, bool playSoundLong, bool playSoundShort, string soundFile)
        {
            return MyReversal2Trend(Input, period1, period2, playSoundLong, playSoundShort, soundFile);
        }

        /// <summary>
        /// Signals on reversal to trend direction.
        /// </summary>
        /// <returns></returns>
        public MyReversal2Trend MyReversal2Trend(Data.IDataSeries input, int period1, int period2, bool playSoundLong, bool playSoundShort, string soundFile)
        {
            if (cacheMyReversal2Trend != null)
                for (int idx = 0; idx < cacheMyReversal2Trend.Length; idx++)
                    if (cacheMyReversal2Trend[idx].Period1 == period1 && cacheMyReversal2Trend[idx].Period2 == period2 && cacheMyReversal2Trend[idx].PlaySoundLong == playSoundLong && cacheMyReversal2Trend[idx].PlaySoundShort == playSoundShort && cacheMyReversal2Trend[idx].SoundFile == soundFile && cacheMyReversal2Trend[idx].EqualsInput(input))
                        return cacheMyReversal2Trend[idx];

            lock (checkMyReversal2Trend)
            {
                checkMyReversal2Trend.Period1 = period1;
                period1 = checkMyReversal2Trend.Period1;
                checkMyReversal2Trend.Period2 = period2;
                period2 = checkMyReversal2Trend.Period2;
                checkMyReversal2Trend.PlaySoundLong = playSoundLong;
                playSoundLong = checkMyReversal2Trend.PlaySoundLong;
                checkMyReversal2Trend.PlaySoundShort = playSoundShort;
                playSoundShort = checkMyReversal2Trend.PlaySoundShort;
                checkMyReversal2Trend.SoundFile = soundFile;
                soundFile = checkMyReversal2Trend.SoundFile;

                if (cacheMyReversal2Trend != null)
                    for (int idx = 0; idx < cacheMyReversal2Trend.Length; idx++)
                        if (cacheMyReversal2Trend[idx].Period1 == period1 && cacheMyReversal2Trend[idx].Period2 == period2 && cacheMyReversal2Trend[idx].PlaySoundLong == playSoundLong && cacheMyReversal2Trend[idx].PlaySoundShort == playSoundShort && cacheMyReversal2Trend[idx].SoundFile == soundFile && cacheMyReversal2Trend[idx].EqualsInput(input))
                            return cacheMyReversal2Trend[idx];

                MyReversal2Trend indicator = new MyReversal2Trend();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.Period1 = period1;
                indicator.Period2 = period2;
                indicator.PlaySoundLong = playSoundLong;
                indicator.PlaySoundShort = playSoundShort;
                indicator.SoundFile = soundFile;
                Indicators.Add(indicator);
                indicator.SetUp();

                MyReversal2Trend[] tmp = new MyReversal2Trend[cacheMyReversal2Trend == null ? 1 : cacheMyReversal2Trend.Length + 1];
                if (cacheMyReversal2Trend != null)
                    cacheMyReversal2Trend.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheMyReversal2Trend = tmp;
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
        /// Signals on reversal to trend direction.
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.MyReversal2Trend MyReversal2Trend(int period1, int period2, bool playSoundLong, bool playSoundShort, string soundFile)
        {
            return _indicator.MyReversal2Trend(Input, period1, period2, playSoundLong, playSoundShort, soundFile);
        }

        /// <summary>
        /// Signals on reversal to trend direction.
        /// </summary>
        /// <returns></returns>
        public Indicator.MyReversal2Trend MyReversal2Trend(Data.IDataSeries input, int period1, int period2, bool playSoundLong, bool playSoundShort, string soundFile)
        {
            return _indicator.MyReversal2Trend(input, period1, period2, playSoundLong, playSoundShort, soundFile);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// Signals on reversal to trend direction.
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.MyReversal2Trend MyReversal2Trend(int period1, int period2, bool playSoundLong, bool playSoundShort, string soundFile)
        {
            return _indicator.MyReversal2Trend(Input, period1, period2, playSoundLong, playSoundShort, soundFile);
        }

        /// <summary>
        /// Signals on reversal to trend direction.
        /// </summary>
        /// <returns></returns>
        public Indicator.MyReversal2Trend MyReversal2Trend(Data.IDataSeries input, int period1, int period2, bool playSoundLong, bool playSoundShort, string soundFile)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.MyReversal2Trend(input, period1, period2, playSoundLong, playSoundShort, soundFile);
        }
    }
}
#endregion
