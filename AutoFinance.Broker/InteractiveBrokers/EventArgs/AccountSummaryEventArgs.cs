// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event args for a AccountSummaryEvent of the TwsCallbackHandler
    /// </summary>
    public class AccountSummaryEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountSummaryEventArgs"/> class.
        /// This class corresponds with the AccountSummary response of the TwsCallbackHandler
        /// </summary>
        /// <param name="requestId">The request ID that corresponds with this response.</param>
        /// <param name="account">The account name.</param>
        /// <param name="tag">The account tag requested.</param>
        /// <param name="value">The account balance.</param>
        /// <param name="currency">The account balance currency. </param>
        public AccountSummaryEventArgs(int requestId, string account, string tag, string value, string currency)
        {
            this.RequestId = requestId;
            this.Account = account;
            this.Tag = tag;
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
        /// Get the Tag value
        /// </summary>
        public string Tag { get; private set; }

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
