// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments for the account download end event
    /// </summary>
    public class AccountDownloadEndEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountDownloadEndEventArgs"/> class.
        ///
        /// </summary>
        /// <param name="account">The account values that were returned</param>
        public AccountDownloadEndEventArgs(string account)
        {
            this.Account = account;
        }

        /// <summary>
        /// Gets the account
        /// </summary>
        public string Account
        {
            get;
            private set;
        }
    }
}
