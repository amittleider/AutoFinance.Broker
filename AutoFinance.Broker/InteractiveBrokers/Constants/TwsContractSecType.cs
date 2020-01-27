// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// This class holds the string parameters values for requesting contracts from the TWS EClientSocketWrapper.
    /// </summary>
    public class TwsContractSecType
    {
        /// <summary>
        /// A futures contract
        /// </summary>
        public const string Future = "FUT";

        /// <summary>
        /// A stock contract
        /// </summary>
        public const string Stock = "STK";
    }
}
