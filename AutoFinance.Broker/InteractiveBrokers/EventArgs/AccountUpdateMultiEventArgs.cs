// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event args for a AccountSummaryEvent of the TwsCallbackHandler
    /// </summary>
    public class AccountUpdateMultiEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountUpdateMultiEventArgs"/> class.
        /// This class corresponds with the AccountSummary response of the TwsCallbackHandler
        /// </summary>
        /// <param name="requestId">The request ID that corresponds with this response.</param>
        /// <param name="account">The account name.</param>
        /// <param name="key">The account key to update.</param>
        /// <param name="value">The account value to update.</param>
        /// <param name="currency">The account balance currency. </param>
        /// <param name="modelCode">The model code.</param>
        public AccountUpdateMultiEventArgs(int requestId, string account, string modelCode, string key, string value, string currency)
        {
            this.RequestId = requestId;
            this.Account = account;
            this.ModelCode = modelCode;
            this.Key = key;
            this.Value = value;
            this.Currency = currency;
        }

        /// <summary>
        /// Gets the request ID that corresponds with this response.
        /// </summary>
        public int RequestId { get; private set; }

        /// <summary>
        /// Gets the account name
        /// </summary>
        public string Account { get; private set; }

        /// <summary>
        /// Gets get the Tag value
        /// </summary>
        public string ModelCode { get; private set; }

        /// <summary>
        /// Gets the key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Get the account value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Get the account currency
        /// </summary>
        public string Currency { get; private set; }
    }
}