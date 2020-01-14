// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Wrappers
{
    /// <summary>
    /// This is a wrapper for the TWS contract.
    /// It's used to overwrite the equals and get hash code of the TWS contract.
    /// </summary>
    public class TwsContract
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwsContract"/> class.
        /// </summary>
        public TwsContract()
        {
            this.ConId = 0;
            this.Symbol = string.Empty;
            this.SecType = string.Empty;
            this.LastTradeDateOrContractMonth = string.Empty;
            this.Strike = 0;
            this.Right = string.Empty;
            this.Multiplier = string.Empty;
            this.Exchange = string.Empty;
            this.Currency = string.Empty;
            this.Exchange = string.Empty;
            this.LocalSymbol = string.Empty;
            this.PrimaryExchange = string.Empty;
            this.TradingClass = string.Empty;
            this.IncludeExpired = false;
            this.SecIdType = string.Empty;
            this.SecId = string.Empty;
        }

        /// <summary>
        /// Gets or sets the con Id
        /// </summary>
        public int ConId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Symbol
        /// </summary>
        public string Symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the security type
        /// </summary>
        public string SecType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last traded date or the contract month.
        /// This is in a format like 201806
        /// </summary>
        public string LastTradeDateOrContractMonth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the strike price
        /// </summary>
        public double Strike
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the right
        /// </summary>
        public string Right
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the multiplier
        /// </summary>
        public string Multiplier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exchange
        /// </summary>
        public string Exchange
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the currency
        /// </summary>
        public string Currency
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the local symbol
        /// </summary>
        public string LocalSymbol
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the primary exchange
        /// </summary>
        public string PrimaryExchange
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the trading class
        /// </summary>
        public string TradingClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include expired contracts
        /// </summary>
        public bool IncludeExpired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the security Id type
        /// </summary>
        public string SecIdType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the security id
        /// </summary>
        public string SecId
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var contract = obj as TwsContract;
            return contract != null &&
                   this.ConId == contract.ConId &&
                   this.Symbol == contract.Symbol &&
                   this.SecType == contract.SecType &&
                   this.LastTradeDateOrContractMonth == contract.LastTradeDateOrContractMonth &&
                   this.Strike == contract.Strike &&
                   this.Right == contract.Right &&
                   this.Multiplier == contract.Multiplier &&
                   this.Exchange == contract.Exchange &&
                   this.Currency == contract.Currency &&
                   this.LocalSymbol == contract.LocalSymbol &&
                   this.PrimaryExchange == contract.PrimaryExchange &&
                   this.TradingClass == contract.TradingClass &&
                   this.IncludeExpired == contract.IncludeExpired &&
                   this.SecIdType == contract.SecIdType &&
                   this.SecId == contract.SecId;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 675893705;
            hashCode = (hashCode * -1521134295) + this.ConId.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Symbol.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.SecType.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.LastTradeDateOrContractMonth.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Strike.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Right.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Multiplier.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Exchange.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Currency.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.LocalSymbol.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.PrimaryExchange.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.TradingClass.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.IncludeExpired.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.SecIdType.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.SecId.GetHashCode();

            return hashCode;
        }
    }
}
