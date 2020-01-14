// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Wrappers
{
    using IBApi;

    /// <summary>
    /// Converts wrapped TWS contracts into the real TWS contracts used by the IB library.
    /// </summary>
    public class TwsContractConverter
    {
        /// <summary>
        /// Convert a wrapped contract to a TWS implementation of the contract
        /// </summary>
        /// <param name="twsContract">The wrapped contract</param>
        /// <returns>The TWS contract</returns>
        public static Contract ConvertToTwsContract(TwsContract twsContract)
        {
            Contract contract = new Contract()
            {
                ConId = twsContract.ConId,
                Currency = twsContract.Currency,
                Exchange = twsContract.Exchange,
                IncludeExpired = twsContract.IncludeExpired,
                LastTradeDateOrContractMonth = twsContract.LastTradeDateOrContractMonth,
                LocalSymbol = twsContract.LocalSymbol,
                Multiplier = twsContract.Multiplier,
                PrimaryExch = twsContract.PrimaryExchange,
                Right = twsContract.Right,
                SecId = twsContract.SecId,
                SecIdType = twsContract.SecIdType,
                SecType = twsContract.SecType,
                Strike = twsContract.Strike,
                Symbol = twsContract.Symbol,
                TradingClass = twsContract.TradingClass,
            };

            return contract;
        }
    }
}
