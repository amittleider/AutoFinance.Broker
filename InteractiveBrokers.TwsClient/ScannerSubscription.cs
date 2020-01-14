/* Copyright (C) 2013 Interactive Brokers LLC. All rights reserved.  This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IBApi
{
    /**
     * @class ScannerSubscription
     * @brief Defines a market scanner request
     */
    public class ScannerSubscription
    {
        private int numberOfRows = -1;
        private string instrument;
        private string locationCode;
        private string scanCode;
        private double abovePrice = Double.MaxValue;
        private double belowPrice = Double.MaxValue;        
        private int aboveVolume = Int32.MaxValue;
        private int averageOptionVolumeAbove = Int32.MaxValue;
        private double marketCapAbove = Double.MaxValue;
        private double marketCapBelow = Double.MaxValue;
        private string moodyRatingAbove;
        private string moodyRatingBelow;
        private string spRatingAbove;
        private string spRatingBelow;
        private string maturityDateAbove;
        private string maturityDateBelow;
        private double couponRateAbove = Double.MaxValue;
        private double couponRateBelow = Double.MaxValue;
        private string excludeConvertible;
        private string scannerSettingPairs;
        private string stockTypeFilter;

        /**
         * @brief The number of rows to be returned for the query
         */
        public int NumberOfRows
        {
            get { return numberOfRows; }
            set { numberOfRows = value; }
        }

        /**
         * @brief The instrument's type for the scan. I.e. STK, FUT.HK, etc.
         */
        public string Instrument
        {
            get { return instrument; }
            set { instrument = value; }
        }

        /**
         * @brief The request's location (STK.US, STK.US.MAJOR, etc). 
         */
        public string LocationCode
        {
            get { return locationCode; }
            set { locationCode = value; }
        }

        /**
         * @brief Same as TWS Market Scanner's "parameters" field, for example: TOP_PERC_GAIN
         */
        public string ScanCode
        {
            get { return scanCode; }
            set { scanCode = value; }
        }

        /**
         * @brief Filters out Contracts which price is below this value
         */
        public double AbovePrice
        {
            get { return abovePrice; }
            set { abovePrice = value; }
        }

        /**
         * @brief Filters out contracts which price is above this value.
         */
        public double BelowPrice
        {
            get { return belowPrice; }
            set { belowPrice = value; }
        }

        /**
         * @brief Filters out Contracts which volume is above this value.
         */
        public int AboveVolume
        {
            get { return aboveVolume; }
            set { aboveVolume = value; }
        }

        /**
         * @brief Filters out Contracts which option volume is above this value.
         */
        public int AverageOptionVolumeAbove
        {
            get { return averageOptionVolumeAbove; }
            set { averageOptionVolumeAbove = value; }
        }

        /**
         * @brief Filters out Contracts which market cap is above this value.
         */
        public double MarketCapAbove
        {
            get { return marketCapAbove; }
            set { marketCapAbove = value; }
        }

        /**
         * @brief Filters out Contracts which market cap is below this value.
         */
        public double MarketCapBelow
        {
            get { return marketCapBelow; }
            set { marketCapBelow = value; }
        }

        /**
         * @brief Filters out Contracts which Moody's rating is below this value.
         */
        public string MoodyRatingAbove
        {
            get { return moodyRatingAbove; }
            set { moodyRatingAbove = value; }
        }

        /**
         * @brief Filters out Contracts which Moody's rating is above this value.
         */
        public string MoodyRatingBelow
        {
            get { return moodyRatingBelow; }
            set { moodyRatingBelow = value; }
        }

        /**
         * @brief Filters out Contracts with a S&P rating below this value.
         */
        public string SpRatingAbove
        {
            get { return spRatingAbove; }
            set { spRatingAbove = value; }
        }

        /**
         * @brief Filters out Contracts with a S&P rating above this value.
         */
        public string SpRatingBelow
        {
            get { return spRatingBelow; }
            set { spRatingBelow = value; }
        }

        /**
         * @brief Filter out Contracts with a maturity date earlier than this value.
         */
        public string MaturityDateAbove
        {
            get { return maturityDateAbove; }
            set { maturityDateAbove = value; }
        }

        /**
         * @brief Filter out Contracts with a maturity date older than this value.
         */
        public string MaturityDateBelow
        {
            get { return maturityDateBelow; }
            set { maturityDateBelow = value; }
        }

        /**
         * @brief Filter out Contracts with a coupon rate lower than this value.
         */
        public double CouponRateAbove
        {
            get { return couponRateAbove; }
            set { couponRateAbove = value; }
        }

        /**
         * @brief Filter out Contracts with a coupon rate higher than this value.
         */
        public double CouponRateBelow
        {
            get { return couponRateBelow; }
            set { couponRateBelow = value; }
        }

        /**
         * @brief Filters out Convertible bonds
         */
        public string ExcludeConvertible
        {
            get { return excludeConvertible; }
            set { excludeConvertible = value; }
        }

        /**
         * @brief For example, a pairing "Annual, true" used on the "top Option Implied Vol % Gainers" scan would return annualized volatilities.
         */
        public string ScannerSettingPairs
        {
            get { return scannerSettingPairs; }
            set { scannerSettingPairs = value; }
        }

        /**
         * @brief -
         *      CORP = Corporation
         *      ADR = American Depositary Receipt
         *      ETF = Exchange Traded Fund
         *      REIT = Real Estate Investment Trust
         *      CEF = Closed End Fund
         */
        public string StockTypeFilter
        {
            get { return stockTypeFilter; }
            set { stockTypeFilter = value; }
        }
    }
}
