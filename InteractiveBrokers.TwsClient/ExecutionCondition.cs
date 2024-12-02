﻿/* Copyright (C) 2019 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */

using System.IO;

namespace IBApi
{
    /**
    * @class ExecutionCondition
    * @brief This class represents a condition requiring a specific execution event to be fulfilled.
    * Orders can be activated or canceled if a set of given conditions is met. An ExecutionCondition is met whenever a trade occurs on a certain product at the given exchange.
    */
    public class ExecutionCondition : OrderCondition
    {
        /**
        * @brief Exchange where the symbol needs to be traded.
        */
        public string Exchange { get; set; }

        /**
        * @brief Kind of instrument being monitored.
        */
        public string SecType { get; set; }

        /**
        * @brief Instrument's symbol
        */
        public string Symbol { get; set; }

        const string header = "trade occurs for ",
                     symbolSuffix = " symbol on ",
                     exchangeSuffix = " exchange for ",
                     secTypeSuffix = " security type";

        public override string ToString()
        {
            return header + Symbol + symbolSuffix + Exchange + exchangeSuffix + SecType + secTypeSuffix;
        }

        protected override bool TryParse(string cond)
        {
            if (!cond.StartsWith(header))
                return false;

            try
            {
                var parser = new StringSuffixParser(cond.Replace(header, ""));

                Symbol = parser.GetNextSuffixedValue(symbolSuffix);
                Exchange = parser.GetNextSuffixedValue(exchangeSuffix);
                SecType = parser.GetNextSuffixedValue(secTypeSuffix);

                if (!string.IsNullOrWhiteSpace(parser.Rest))
                    return base.TryParse(parser.Rest);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public override void Deserialize(IDecoder inStream)
        {
            base.Deserialize(inStream);

            SecType = inStream.ReadString();
            Exchange = inStream.ReadString();
            Symbol = inStream.ReadString();
        }

        public override void Serialize(BinaryWriter outStream)
        {
            base.Serialize(outStream);

            outStream.AddParameter(SecType);
            outStream.AddParameter(Exchange);
            outStream.AddParameter(Symbol);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ExecutionCondition;

            if (other == null)
                return false;

            return base.Equals(obj) 
                && Exchange.Equals(other.Exchange)
                && SecType.Equals(other.SecType)
                && Symbol.Equals(other.Symbol);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Exchange.GetHashCode() + SecType.GetHashCode() + Symbol.GetHashCode();
        }
    }
}
