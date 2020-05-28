// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// This class holds the string parameters values for requesting contracts from the TWS EClientSocketWrapper.
    /// http://interactivebrokers.github.io/tws-api/classIBApi_1_1Contract.html
    /// </summary>
    public class TwsContractSecType
    {
        /// <summary>
        /// A stock contract
        /// </summary>
        public const string Stock = "STK";

        /// <summary>
        /// An option contract
        /// </summary>
        public const string Option = "OPT";

        /// <summary>
        /// A future contract
        /// </summary>
        public const string Future = "FUT";

        /// <summary>
        /// An index contract
        /// </summary>
        public const string Index = "IND";

        /// <summary>
        /// A future option contract
        /// </summary>
        public const string FutureOption = "FOP";

        /// <summary>
        /// A forex pair contract
        /// </summary>
        public const string Cash = "CASH";

        /// <summary>
        /// A combo contract
        /// </summary>
        public const string Combo = "BAG";

        /// <summary>
        /// A warrant contract
        /// </summary>
        public const string Warrant = "WAR";

        /// <summary>
        /// A bond contract
        /// </summary>
        public const string Bond = "BOND";

        /// <summary>
        /// A commodity contract
        /// </summary>
        public const string Commodity = "CMDTY";

        /// <summary>
        /// A news contract
        /// </summary>
        public const string News = "NEWS";

        /// <summary>
        /// A mutual fund contract
        /// </summary>
        public const string MutualFund = "FUND";
    }
}
