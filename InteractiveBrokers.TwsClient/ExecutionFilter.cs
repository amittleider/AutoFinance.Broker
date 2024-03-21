/* Copyright (C) 2019 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */

using System.Collections.Generic;

namespace IBApi
{
    /**
     * @class ExecutionFilter
     * @brief when requesting executions, a filter can be specified to receive only a subset of them
     * @sa Contract, Execution, CommissionReport
     */
    public class ExecutionFilter
    {
        /**
         * @brief The API client which placed the order
         */
        public int ClientId { get; set; }

        /**
        * @brief The account to which the order was allocated to
        */
        public string AcctCode { get; set; }

        /**
         * @brief Time from which the executions will be returned yyyymmdd hh:mm:ss
         * Only those executions reported after the specified time will be returned.
         */
        public string Time { get; set; }

        /**
        * @brief The instrument's symbol
        */
        public string Symbol { get; set; }

        /**
         * @brief The Contract's security's type (i.e. STK, OPT...)
         */
        public string SecType { get; set; }

        /**
         * @brief The exchange at which the execution was produced
         */
        public string Exchange { get; set; }

        /**
        * @brief The Contract's side (BUY or SELL)
        */
        public string Side { get; set; }

        public ExecutionFilter()
        {
            ClientId = 0;
        }

        public ExecutionFilter(int clientId, string acctCode, string time,
                string symbol, string secType, string exchange, string side)
        {
            ClientId = clientId;
            AcctCode = acctCode;
            Time = time;
            Symbol = symbol;
            SecType = secType;
            Exchange = exchange;
            Side = side;
        }

        public override bool Equals(object other)
        {
            bool l_bRetVal = false;
            ExecutionFilter l_theOther = other as ExecutionFilter;

            if (l_theOther == null)
            {
                l_bRetVal = false;
            }
            else if (this == other)
            {
                l_bRetVal = true;
            }
            else
            {
                l_bRetVal = (ClientId == l_theOther.ClientId &&
                    string.Compare(AcctCode, l_theOther.AcctCode, true) == 0 &&
                    string.Compare(Time, l_theOther.Time, true) == 0 &&
                    string.Compare(Symbol, l_theOther.Symbol, true) == 0 &&
                    string.Compare(SecType, l_theOther.SecType, true) == 0 &&
                    string.Compare(Exchange, l_theOther.Exchange, true) == 0 &&
                    string.Compare(Side, l_theOther.Side, true) == 0);
            }
            return l_bRetVal;
        }

        public override int GetHashCode()
        {
            var hashCode = 82934527;
            hashCode = hashCode * -1521134295 + ClientId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AcctCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Time);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Symbol);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SecType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Exchange);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Side);
            return hashCode;
        }
    }
}
