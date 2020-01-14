// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers
{
    /// <summary>
    /// Provides the event arguments for the UpdateAccountValue event
    /// </summary>
    public class UpdateAccountValueEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAccountValueEventArgs"/> class.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <param name="currency">The currency</param>
        /// <param name="accountName">The account name</param>
        public UpdateAccountValueEventArgs(string key, string value, string currency, string accountName)
        {
            this.Key = key;
            this.Value = value;
            this.Currency = currency;
            this.AccountName = accountName;
        }

        /// <summary>
        /// Gets the key of the dictionary value.
        /// These are defined in the TwsAccountUpdates enum.
        /// </summary>
        public string Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value of the corresponding key
        /// </summary>
        public string Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the currency
        /// </summary>
        public string Currency
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the account name
        /// </summary>
        public string AccountName
        {
            get;
            private set;
        }
    }
}
