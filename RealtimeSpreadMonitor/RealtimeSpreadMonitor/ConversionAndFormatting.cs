using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtimeSpreadMonitor
{
    class ConversionAndFormatting
    {
        public static String convertToTickMovesString(double barVal, double tickIncrement, double tickDisplay)
        {
            try
            {


                return Convert.ToString(convertToTickMovesDouble(barVal, tickIncrement, tickDisplay));
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut("ConversionAndFormatting", ex);

                return Convert.ToString(barVal);
            }
        }

        //int TMLtradingSystem.convertToTickMovesInt(double barVal, double tickDisplay)
        //{
        //	try
        //	{
        //		if(tickDisplay != 0 && tickDisplay != 1)
        //		{
        //			int intPart = (int)barVal;
        //			double decPart = barVal - intPart;
        //
        //			int converted = (int)(decPart/tickDisplay * 10);		
        //
        //			return (int)(intPart*1000 + converted);
        //		}
        //
        //		return (int)barVal;
        //	}
        //	catch (Exception ex)
        //	{
        //		Debug.WriteLine("convertToTickMoves " + ex);
        //		return (int)barVal;
        //	}
        //}

        // instrument			tick incr.			display incr	tick value		sample display		value
        // --------------------------------------------------------------------------------------------------
        // USA (30YR)			0.03125				10				31.25
        // TY (10YR)			0.015625			 5				15.625
        // FV (5YR)				0.0078125			 2.5			7.8125

        // corn				ZC	0.25				 2				12.50	

        // s&p					0.25				25				12.50		
        // dow					1					 1				5

        // euro$ front		GE	0.0025				 0.25			 6.25			9970.25
        // euro$ back		GE	0.005				 0.50			12.50			9970.5

        // brent crude								 0

        // mini crude		QM	0.025				25				12.50			71125 71150	
        // sweet crude		CL	0.01				 1				10.00			7107 7108	

        // nat gas mini		QG	0.005				 5				12.50			3400 or 3405
        // nat gas large	NG	0.001				 1				10.00			3401 3402			3.401

        // eurofx			6E	0.0001				 1				12.50			14604.0 14605.0		1.46040	
        // cdn				6C	0.0001				 1				10.00			9263 9264			0.9263
        // bp				6B	0.0001				 1				 6.25			16469 16470			1.6469
        // Jap				6J	0.000001			 1				12.50			10855 10856			0.010855
        // aus				6A	0.0001				 1				10.00			8573 8574			0.8573
        // swiss			6S	0.0001				 1				12.50			9639 9640			0.9639


        public static double convertToStrikeForTT(double barVal, double tickIncrement, double tickDisplay,
            int idInstrument)
        {
            if (tickDisplay == 0)
            {
                return barVal;
            }
            else if (idInstrument == 39 || idInstrument == 40) //GLE or HE
            {
                return barVal * tickDisplay;
            }
            else if (idInstrument == 101) //GLE or HE
            {
                return convertToTickMovesDouble(barVal, tickIncrement, tickDisplay) / 100;
            }
            else
            {
                return convertToTickMovesDouble(barVal, tickIncrement, tickDisplay);
            }
        }

        public static double convertToOrderPriceForTT(double barVal, double tickIncrement, double tickDisplay,
            int idInstrument, bool isOptionContract)
        {
            if (tickDisplay == 0)
            {
                return barVal;
            }
            else if (idInstrument == 3)
            {
                return barVal;
            }
            else if (idInstrument == 31 || idInstrument == 32) //ZSE or ZCE
            {
                return barVal * 0.01;
            }
            //else if (idInstrument == 52 && !isOptionContract) //EU6 Futures only
            //{
            //    double tempVal = convertToTickMovesDouble(barVal, tickIncrement, tickDisplay);
            //    return tempVal;
            //}
            else
            {
                return convertToTickMovesDouble(barVal, tickIncrement, tickDisplay);
            }
        }


        public static double convertToStrikeForCQGSymbol(double barVal, double tickIncrement, double tickDisplay,
            int idInstrument)
        {
            if (idInstrument == 39 || idInstrument == 40) //GLE or HE
            {
                return barVal * tickDisplay;
            }
            else if (idInstrument == 1 || idInstrument == 360)
            {
                return barVal * tickDisplay;
            }
            //else if (idInstrument == 101) //GLE or HE
            //{
            //    return convertToTickMovesDouble(barVal, tickIncrement, tickDisplay) / 100;
            //}
            else
            {
                return convertToTickMovesDouble(barVal, tickIncrement, tickDisplay);
            }
        }

        public static double convertToTickMovesDouble(double barVal, double tickIncrement, double tickDisplay)
        {
            if (tickDisplay == 0)
            {
                return barVal;
            }
            else
            {
                try
                {
                    double fuzzyZero = tickIncrement / 1000;
                    double positiveFuzzyZero = tickIncrement / 1000;

                    if (barVal < 0)
                        fuzzyZero = -tickIncrement / 1000;

                    int nTicksInUnit = (int)(1 / tickIncrement + positiveFuzzyZero);

                    if (nTicksInUnit == 0)
                        return barVal / tickIncrement * tickDisplay;

                    //nTicksInUnit = 1;

                    int intPart = (int)(barVal + fuzzyZero);
                    int nTicks = (int)((barVal + fuzzyZero) / tickIncrement + fuzzyZero);
                    //Debug.WriteLine(barVal + "  " + tickIncrement + "  " + fuzzyZero + "  " + nTicks + "  ");

                    int decPart = (int)((nTicks % nTicksInUnit) * tickDisplay + fuzzyZero);
                    double fractPart = 0;

                    // a hack for Eurodollar
                    if (tickDisplay < 1)
                        fractPart = (nTicks % nTicksInUnit) * tickDisplay - decPart;

                    int decimalBase = 1;

                    //if (tickDisplay/tickIncrement > 1)
                    //    decimalBase = (int)tickDisplay;
                    //else



                    while (((nTicksInUnit - 1) * tickDisplay / decimalBase) >= 1)
                        decimalBase *= 10;


                    //if (decimalBase == 1 && tickDisplay > 0 && tickIncrement > 0 && tickDisplay > tickIncrement)
                    //{
                    //    decimalBase = (int)(tickDisplay / tickIncrement);
                    //}


                    return intPart * decimalBase + decPart + fractPart;

                }
                catch (Exception ex)
                {
                    TSErrorCatch.errorCatchOut("ConversionAndFormatting", ex);

                    return (int)barVal;
                }
            }
        }

        /**
         * double fuzzyZero = tickIncrement / 1000;
                    double positiveFuzzyZero = tickIncrement / 1000;

                    if (barVal < 0)
                        fuzzyZero = -tickIncrement / 1000;

                    int nTicksInUnit = (int)(1 / tickIncrement + positiveFuzzyZero);

                    if (nTicksInUnit == 0)
                        return barVal / tickIncrement * tickDisplay;

                    //nTicksInUnit = 1;

                    int intPart = (int)(barVal + fuzzyZero);
                    int nTicks = (int)((barVal + fuzzyZero) / tickIncrement + fuzzyZero);
                    //Debug.WriteLine(barVal + "  " + tickIncrement + "  " + fuzzyZero + "  " + nTicks + "  ");

                    int decPart = (int)((nTicks % nTicksInUnit) * tickDisplay + fuzzyZero);
                    double fractPart = 0;

                    // a hack for Eurodollar
                    if (tickDisplay < 1)
                        fractPart = (nTicks % nTicksInUnit) * tickDisplay - decPart;

                    int decimalBase = 1;

                    //if (tickDisplay/tickIncrement > 1)
                    //    decimalBase = (int)tickDisplay;
                    //else



                    while (((nTicksInUnit - 1) * tickDisplay / decimalBase) >= 1)
                        decimalBase *= 10;


                    //if (decimalBase == 1 && tickDisplay > 0 && tickIncrement > 0 && tickDisplay > tickIncrement)
                    //{
                    //    decimalBase = (int)(tickDisplay / tickIncrement);
                    //}


                    return intPart * decimalBase + decPart + fractPart;
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * double fuzzyZero = tickIncrement / 1000;
                    double positiveFuzzyZero = tickIncrement / 1000;

                    int nTicksInUnit = (int)(1 / tickIncrement + positiveFuzzyZero);

                    double maxFractUnits = 0;

                    if (tickIncrement == 1 && tickDisplay > 0)
                    {
                        maxFractUnits = nTicksInUnit * tickDisplay;
                    }
                    else
                    {
                        maxFractUnits = (nTicksInUnit - 1) * tickDisplay;
                    }


                    int decimalBase = 1;
                    while ((maxFractUnits + positiveFuzzyZero) / decimalBase >= 1)
                        decimalBase *= 10;

                    double displayVal = Convert.ToDouble(barVal);

                    if (displayVal < 0)
                        fuzzyZero = -tickIncrement / 1000;

                    int intPart = (int)((displayVal + fuzzyZero) / decimalBase + fuzzyZero);
                    double decPart = (displayVal - intPart * decimalBase) / tickDisplay * tickIncrement;

                    double incrementFixTest = (displayVal - intPart * decimalBase) % (tickIncrement * decimalBase);

                    double incrementFix = 0;

                    if (incrementFixTest != 0)
                        incrementFix = ((tickIncrement * decimalBase) - incrementFixTest) / decimalBase;


                    return intPart + decPart + incrementFix;
         */

        public static double convertToTickMovesDouble(String barVal, double tickIncrement, double tickDisplay)
        {
            if (tickDisplay == 0)
                return Convert.ToDouble(barVal);
            try
            {
                double fuzzyZero = tickIncrement / 1000;  //0.000000000001;
                double positiveFuzzyZero = tickIncrement / 1000;  //0.000000000001;

                int nTicksInUnit = (int)(1 / tickIncrement + positiveFuzzyZero);
                double maxFractUnits = (nTicksInUnit - 1) * tickDisplay;

                if(tickIncrement == 1)
                {
                    maxFractUnits = tickDisplay;
                }

                int decimalBase = 1;
                while ((maxFractUnits + positiveFuzzyZero) / decimalBase >= 1)
                    decimalBase *= 10;

                double displayVal = Convert.ToDouble(barVal);

                if (displayVal < 0)
                    fuzzyZero = -tickIncrement / 1000;

                int intPart = (int)((displayVal + fuzzyZero) / decimalBase + fuzzyZero);
                double decPart = (displayVal - intPart * decimalBase) / tickDisplay * tickIncrement;

                double fractPart = 0;

                // a hack for Eurodollar
                //if ( tickDisplay < 1 )
                //	fractPart = (nTicks % nTicksInUnit) * tickDisplay - decPart;

                double res = intPart + decPart + fractPart;

                double incrMultiple = Math.Floor(res / tickIncrement + positiveFuzzyZero) * tickIncrement;

                if (res < incrMultiple + positiveFuzzyZero && res > incrMultiple - positiveFuzzyZero)
                {
                    return res;
                }
                else
                {
                    return incrMultiple + tickIncrement;
                }


                // make sure return value is an integral multiple of the tick increment
                //return Math.Ceiling((intPart + decPart) / tickIncrement - positiveFuzzyZero)
                //    * tickIncrement;

                //return intPart + decPart + fractPart;

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut("ConversionAndFormatting", ex);

                return Convert.ToDouble(barVal);
            }
        }

        

        public static double roundToSmallestIncrement(double inputValue, double tickIncrement, double optionTickSize,
           double secondaryOptionTickSize)
        {
            double minIncrement = tickIncrement;

            if (optionTickSize > 0 && minIncrement > optionTickSize)
            {
                minIncrement = optionTickSize;
            }

            if (secondaryOptionTickSize > 0 && minIncrement > secondaryOptionTickSize)
            {
                minIncrement = secondaryOptionTickSize;
            }

            double nTicksInUnit = 1 / minIncrement;

            int nDecPlaces = 0;
            int nFactor = 1;
            int nPower = 0;

            while (nPower < 10)   // using 10 just to avoid infinite loop in case of some stupid singularity
            {
                nPower = nPower + 1;
                nFactor = nFactor * 10;
                if (nFactor % nTicksInUnit == 0)
                {
                    nDecPlaces = nPower;
                    break;
                }
            }

            return Math.Round(inputValue, nDecPlaces);
        }



        

        

       
    }
}
