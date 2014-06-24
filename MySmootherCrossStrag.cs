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
    /// 
    /// </summary>
    [Description("")]
    public class MySmootherCrossStrag : Strategy
    {
        #region Variables
        // Wizard generated variables
        private int fastMAPeriod = 5; // Default setting for FastMAPeriod
        private int slowMAPeriod = 20; // Default setting for SlowMAPeriod
        private int fastSRPeriod = 89; // Default setting for FastSRPeriod
        private int slowSRPeriod = 89; // Default setting for SlowSRPeriod
        // User defined variables (add any user defined variables below)
        
        private DataSeries _fastMAValues;
        private DataSeries _slowMAValues;
        private DataSeries _fastSRValues;
        private DataSeries _slowSRValues;
        
        #endregion

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
            //SetProfitTarget("", CalculationMode.Ticks, 8);
            //SetTrailStop("", CalculationMode.Ticks, 8, false);
            
            _fastMAValues = new DataSeries(this);
            _slowMAValues = new DataSeries(this);
            
            _fastSRValues = new DataSeries(this);
            _slowSRValues = new DataSeries(this);
            
            CalculateOnBarClose = false;
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
                if(ToTime(Time[0]) >= 143000)
                    return;
                
            #endregion
                
            //double fastEMA = EMA(FastMAPeriod)
            _fastMAValues.Set(EMA(fastMAPeriod)[0]);
            _slowMAValues.Set(anaSuperSmootherFilter(slowMAPeriod, 2)[0]);
            
            _fastSRValues.Set(anaSuperSmootherFilter(fastSRPeriod, 2)[0]);
            _slowSRValues.Set(anaSuperSmootherFilter(slowSRPeriod, 3)[0]);
            
            if(Position.MarketPosition == MarketPosition.Long)
            {
                if (CrossBelow(_fastMAValues, _slowMAValues, 1))
                {
                    ExitLong("", "");
                }
            }
            else if(Position.MarketPosition == MarketPosition.Short)
            {
                if (CrossAbove(_fastMAValues, _slowMAValues, 1))
                {
                    ExitShort("", "");
                }
            }
                
                
                
            if(_fastSRValues[0] > _slowSRValues[0] &&
               _slowMAValues[0] > _fastSRValues[0])
            {
                // Condition set 1
                if (CrossAbove(_fastMAValues, _slowMAValues, 1))
                {
                    EnterLong(1, "");
                }
            }

            if(_fastSRValues[0] < _slowSRValues[0] &&
                _slowMAValues[0] < _fastSRValues[0])
            {    
                // Condition set 2
                if (CrossBelow(_fastMAValues, _slowMAValues, 1))
                {
                    //ExitLong("", "");
                    EnterShort(1, "");
                }
            }
        }

        #region Properties
        [Description("")]
        [GridCategory("Parameters")]
        public int FastMAPeriod
        {
            get { return fastMAPeriod; }
            set { fastMAPeriod = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int SlowMAPeriod
        {
            get { return slowMAPeriod; }
            set { slowMAPeriod = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int FastSRPeriod
        {
            get { return fastSRPeriod; }
            set { fastSRPeriod = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int SlowSRPeriod
        {
            get { return slowSRPeriod; }
            set { slowSRPeriod = Math.Max(1, value); }
        }
        #endregion
    }
}
