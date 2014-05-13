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
//  This indicator is provided on an "AS IS" basis, without warranty of any kind,  
//  including without limitation the warranties of merchantability, fitness for a  
//  particular purpose and non-infringement.
//
//  mlwin1@yahoo.com
//
// ************************************************************************************


// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    /// <summary>
    /// Signals when price retraced to trending MA
    /// </summary>
    [Description("Signals when price retraced to trending MA.")]
    public class MyRetrace2TrendMA : Indicator
    {
        #region Variables
        // Wizard generated variables
		
			private double _open 	  = 0.0;
			private double _high 	  = 0.0;
			private double _low       = 0.0;
			private double _close     = 0.0;
		
			private double _ema5      = 0.0;
		    private double _ema5Last  = 0.0;
		
		    private double _ema13     = 0.0;
		    private double _ema13Last = 0.0;
		
		    private double _ema20     = 0.0;
		    private double _ema20Last = 0.0;
		
		    private double _srMA      = 0.0;
		    private double _srMA_Last = 0.0;
		
		    private double _ppma1     = 0.0;
		    private double _ppma1Last = 0.0;
		
		    private double _ppma3     = 0.0;
		    private double _ppma3Last = 0.0;
		
			private double _emaDiff   = 0.0;
		
			private double _ema50     = 0.0;
		
		    private bool   _bPlaySoundLong  = true;
		    private bool   _bPlaySoundShort = true;
		
		    private int    _lastBarPlayed = 0;
		    		
		    private int iTouchPeriod  = 5;
			private int iFastPeriod   = 12; // Default setting for FastPeriod
			private int iSlowPeriod   = 20; // Default setting for SlowPeriod
		    private int iSR_MA_Period = 50;
		
			private double _ema20LastAvg = 0.0;
			private double _srMA_LastAvg = 0.0;
		
			private int    _ema20_Slope = 0;
			private int    _srMA_Slope  = 0;
		
		    private string _soundFile = @"C:\Sounds\AlertAlert.wav";

			private DataSeries _signal;
		    private DataSeries _signalAux;
		
			private double _180_over_PI = 0.0;
		
			
		
			//, _signalFromDM;
		
        // User defined variables (add any user defined variables below)
        #endregion
		
        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
            Overlay				= true;
			_signal             = new DataSeries(this);
			_signalAux          = new DataSeries(this);
			CalculateOnBarClose = false;
			_lastBarPlayed = 0;
			
			_180_over_PI = 180/Math.PI;
        }
		
		#region OnStartUp
		protected override void OnStartUp() 
		{
		}
		#endregion

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
            // Use this method for calculating your indicator values. Assign a value to each
            // plot below by replacing 'Close[0]' with your own formula.
			
			if (BarsInProgress != 0)
				return;
			
			if(CurrentBar < 4)
				return;
			
			_open 	   = RoundPrice(Open[0]);
			_high 	   = RoundPrice(High[0]);
			_low 	   = RoundPrice(Low[0]);
			_close 	   = RoundPrice(Close[0]);
			
			_ema5      = EMA(iTouchPeriod)[0];
		    _ema13     = EMA(iFastPeriod)[0];
		
		    _ema20     = EMA(Typical, iSlowPeriod)[0];	
			_srMA      = SMA(iSR_MA_Period)[0];
		    _ppma1     = SMA(Typical, 1)[0];
		    _ppma3     = SMA(Typical, 3)[0];
			
			_emaDiff   = (_ema5 - _ema20)/TickSize;
			
			if(FirstTickOfBar)
			{
				_ema5Last  = EMA(Typical, iTouchPeriod)[1];
				_ema20Last = EMA(iSlowPeriod)[1];
				_srMA_Last = SMA(iSR_MA_Period)[1];
				_ema50     = EMA(50)[0];
								
				_ema20LastAvg = (_ema20Last + EMA(iSlowPeriod)[2])/2;
				_srMA_LastAvg = (_srMA_Last + SMA(iSR_MA_Period)[2])/2;
				
				//_ppma1Last = SMA(Typical, 1)[1];
				_ppma3Last = SMA(Typical, 3)[1];
				_signal.Set(0);
				_signalAux.Set(0);

			}
			
			_ema20_Slope = (int)(Math.Atan((_ema20-_ema20LastAvg)/TickSize)*_180_over_PI);
			_srMA_Slope  = (int)(Math.Atan((_srMA-_srMA_LastAvg)/TickSize)*_180_over_PI);
						
			if(_close  >   _open       &&
			   _low    <=  _ema5       &&  // low must be touching EMA5
			   _high   >   _ema20      &&  // high must be greater than EMA20
			   _ema5   >   _ema5Last   &&
			   _ema13  >   _ema20      &&
			   _ema20  >   _ema20Last  &&
				_ema5  >   _ema20        // EMA5 crosses EMA20
			   //_ema20  <=  _ema13     &&  // EMA13 crosses EMA20
			   //_ema13  <=  _ema5       &&
			   )
			{
				DrawArrowUp("TUp"+CurrentBar, true, 0, Low[0]-2*TickSize, Color.Cyan);
				_signal.Set(1);
				
				if(_close     >  _srMA      &&
				   _ppma3     <= _ppma1     &&
				   _ppma3Last <  _ppma3     &&
				   RSI(20, 3)[0] <  70      &&
				   ADX(5)[0] > 10    &&
				   ADX(5)[0] < 60    &&
				   _emaDiff > .25 && _emaDiff < 8.0 && 
				   _srMA_Slope  > 2)
				{
					DrawDiamond("DUp"+CurrentBar, true, 0, Low[0]-4*TickSize, Color.Cyan);
					_signalAux.Set(3);
				}
				
				if(_bPlaySoundLong && (CurrentBar > _lastBarPlayed+1))
				{
					_lastBarPlayed = CurrentBar;
					PlaySound(_soundFile);
				}
				
				if(_ema5 > _ema13 && _ema13 > _ema20 && _ema20 > _ema50)
				{
					BarColorSeries[0] = Color.Cyan;
				}
			}
			else if(_close  <   _open       && 
				    _high   >=  _ema5       &&  // high must be touching EMA5
				    _low    <   _ema20      &&  // low must be less than EMA20 
			        _ema5   <   _ema5Last   &&
			        _ema13  <   _ema20      &&
			        _ema20  <   _ema20Last  &&
				    _ema5  <  _ema20      // EMA5 crosses EMA20
			        //_ppma1  <   _ppma1Last  &&
			        //_ema20 >=  _ema13     && // EMA13 crosses EMA20
			        //_ema13  >=  _ema5       &&
			             )
			{
				DrawArrowDown("TDown"+CurrentBar, true, 0, High[0]+2*TickSize, Color.Orange);
				_signal.Set(2);
				
				if(_close    <  _srMA      &&
				   _ppma3    >= _ppma1     &&
				   _ppma3Last > _ppma3     &&
				   RSI(20, 3)[0] >  30     &&
				   ADX(5)[0] > 10          &&
				   ADX(5)[0] < 60          &&
				   _emaDiff < -.25 && _emaDiff > -7 &&
				   //_ema20_Slope < -10 && _ema20_Slope > -50 &&
				   _srMA_Slope  < -4)
				{
					DrawDiamond("DDown"+CurrentBar, true, 0, High[0]+4*TickSize, Color.Orange);
					_signalAux.Set(4);
				}				
				
				if(_bPlaySoundShort && (CurrentBar > _lastBarPlayed+1))
				{
					_lastBarPlayed = CurrentBar;
					PlaySound(_soundFile);
				}
				
				if(_ema5 < _ema13 && _ema13 < _ema20 && _ema20 < _ema50)
				{
					BarColorSeries[0] = Color.Fuchsia;
				}
			}
        }
	
		private double RoundPrice(double value)
        {
            return Bars.Instrument.MasterInstrument.Round2TickSize(value);
        }
        #region Properties
        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries Plot0
        {
            get { return Values[0]; }
        }

        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries Signal
        {
            get { Update(); return _signal; }
        }		
		
		
		[Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries SignalAux
        {
            get { Update(); return _signalAux; }
        }
		
		[Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public int SlopeSlowEMA
        {
            get { return _ema20_Slope; }
        }
		
		[Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public int SlopeSR_EMA
        {
            get { return _srMA_Slope; }
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
		
		
		[Description("Touch EMA")]
		[GridCategory("EMA")]
		public int TouchPeriod
		{
			get { return iTouchPeriod; }
			set { iTouchPeriod = value; }
		}
		
		[Description("Fast EMA")]
		[GridCategory("EMA")]
		public int FastPeriod
		{
			get { return iFastPeriod; }
			set { iFastPeriod = value; }
		}
		
		[Description("Slow EMA")]
		[GridCategory("EMA")]
		public int SlowPeriod
		{
			get { return iSlowPeriod; }
			set { iSlowPeriod = value; }
		}
		
		[Description("SR EMA")]
		[GridCategory("EMA")]
		public int SRPeriod
		{
			get { return iSR_MA_Period; }
			set { iSR_MA_Period = value; }
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
        private MyRetrace2TrendMA[] cacheMyRetrace2TrendMA = null;

        private static MyRetrace2TrendMA checkMyRetrace2TrendMA = new MyRetrace2TrendMA();

        /// <summary>
        /// Signals when price retraced to trending MA.
        /// </summary>
        /// <returns></returns>
        public MyRetrace2TrendMA MyRetrace2TrendMA(int fastPeriod, bool playSoundLong, bool playSoundShort, int slowPeriod, string soundFile, int sRPeriod, int touchPeriod)
        {
            return MyRetrace2TrendMA(Input, fastPeriod, playSoundLong, playSoundShort, slowPeriod, soundFile, sRPeriod, touchPeriod);
        }

        /// <summary>
        /// Signals when price retraced to trending MA.
        /// </summary>
        /// <returns></returns>
        public MyRetrace2TrendMA MyRetrace2TrendMA(Data.IDataSeries input, int fastPeriod, bool playSoundLong, bool playSoundShort, int slowPeriod, string soundFile, int sRPeriod, int touchPeriod)
        {
            if (cacheMyRetrace2TrendMA != null)
                for (int idx = 0; idx < cacheMyRetrace2TrendMA.Length; idx++)
                    if (cacheMyRetrace2TrendMA[idx].FastPeriod == fastPeriod && cacheMyRetrace2TrendMA[idx].PlaySoundLong == playSoundLong && cacheMyRetrace2TrendMA[idx].PlaySoundShort == playSoundShort && cacheMyRetrace2TrendMA[idx].SlowPeriod == slowPeriod && cacheMyRetrace2TrendMA[idx].SoundFile == soundFile && cacheMyRetrace2TrendMA[idx].SRPeriod == sRPeriod && cacheMyRetrace2TrendMA[idx].TouchPeriod == touchPeriod && cacheMyRetrace2TrendMA[idx].EqualsInput(input))
                        return cacheMyRetrace2TrendMA[idx];

            lock (checkMyRetrace2TrendMA)
            {
                checkMyRetrace2TrendMA.FastPeriod = fastPeriod;
                fastPeriod = checkMyRetrace2TrendMA.FastPeriod;
                checkMyRetrace2TrendMA.PlaySoundLong = playSoundLong;
                playSoundLong = checkMyRetrace2TrendMA.PlaySoundLong;
                checkMyRetrace2TrendMA.PlaySoundShort = playSoundShort;
                playSoundShort = checkMyRetrace2TrendMA.PlaySoundShort;
                checkMyRetrace2TrendMA.SlowPeriod = slowPeriod;
                slowPeriod = checkMyRetrace2TrendMA.SlowPeriod;
                checkMyRetrace2TrendMA.SoundFile = soundFile;
                soundFile = checkMyRetrace2TrendMA.SoundFile;
                checkMyRetrace2TrendMA.SRPeriod = sRPeriod;
                sRPeriod = checkMyRetrace2TrendMA.SRPeriod;
                checkMyRetrace2TrendMA.TouchPeriod = touchPeriod;
                touchPeriod = checkMyRetrace2TrendMA.TouchPeriod;

                if (cacheMyRetrace2TrendMA != null)
                    for (int idx = 0; idx < cacheMyRetrace2TrendMA.Length; idx++)
                        if (cacheMyRetrace2TrendMA[idx].FastPeriod == fastPeriod && cacheMyRetrace2TrendMA[idx].PlaySoundLong == playSoundLong && cacheMyRetrace2TrendMA[idx].PlaySoundShort == playSoundShort && cacheMyRetrace2TrendMA[idx].SlowPeriod == slowPeriod && cacheMyRetrace2TrendMA[idx].SoundFile == soundFile && cacheMyRetrace2TrendMA[idx].SRPeriod == sRPeriod && cacheMyRetrace2TrendMA[idx].TouchPeriod == touchPeriod && cacheMyRetrace2TrendMA[idx].EqualsInput(input))
                            return cacheMyRetrace2TrendMA[idx];

                MyRetrace2TrendMA indicator = new MyRetrace2TrendMA();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.FastPeriod = fastPeriod;
                indicator.PlaySoundLong = playSoundLong;
                indicator.PlaySoundShort = playSoundShort;
                indicator.SlowPeriod = slowPeriod;
                indicator.SoundFile = soundFile;
                indicator.SRPeriod = sRPeriod;
                indicator.TouchPeriod = touchPeriod;
                Indicators.Add(indicator);
                indicator.SetUp();

                MyRetrace2TrendMA[] tmp = new MyRetrace2TrendMA[cacheMyRetrace2TrendMA == null ? 1 : cacheMyRetrace2TrendMA.Length + 1];
                if (cacheMyRetrace2TrendMA != null)
                    cacheMyRetrace2TrendMA.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheMyRetrace2TrendMA = tmp;
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
        /// Signals when price retraced to trending MA.
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.MyRetrace2TrendMA MyRetrace2TrendMA(int fastPeriod, bool playSoundLong, bool playSoundShort, int slowPeriod, string soundFile, int sRPeriod, int touchPeriod)
        {
            return _indicator.MyRetrace2TrendMA(Input, fastPeriod, playSoundLong, playSoundShort, slowPeriod, soundFile, sRPeriod, touchPeriod);
        }

        /// <summary>
        /// Signals when price retraced to trending MA.
        /// </summary>
        /// <returns></returns>
        public Indicator.MyRetrace2TrendMA MyRetrace2TrendMA(Data.IDataSeries input, int fastPeriod, bool playSoundLong, bool playSoundShort, int slowPeriod, string soundFile, int sRPeriod, int touchPeriod)
        {
            return _indicator.MyRetrace2TrendMA(input, fastPeriod, playSoundLong, playSoundShort, slowPeriod, soundFile, sRPeriod, touchPeriod);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// Signals when price retraced to trending MA.
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.MyRetrace2TrendMA MyRetrace2TrendMA(int fastPeriod, bool playSoundLong, bool playSoundShort, int slowPeriod, string soundFile, int sRPeriod, int touchPeriod)
        {
            return _indicator.MyRetrace2TrendMA(Input, fastPeriod, playSoundLong, playSoundShort, slowPeriod, soundFile, sRPeriod, touchPeriod);
        }

        /// <summary>
        /// Signals when price retraced to trending MA.
        /// </summary>
        /// <returns></returns>
        public Indicator.MyRetrace2TrendMA MyRetrace2TrendMA(Data.IDataSeries input, int fastPeriod, bool playSoundLong, bool playSoundShort, int slowPeriod, string soundFile, int sRPeriod, int touchPeriod)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.MyRetrace2TrendMA(input, fastPeriod, playSoundLong, playSoundShort, slowPeriod, soundFile, sRPeriod, touchPeriod);
        }
    }
}
#endregion
