// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments for Error events sent from TWS.
    /// </summary>
    public class ErrorEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs"/> class.
        /// </summary>
        /// <param name="id">The error Id</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="errorMsg">The error message</param>
        public ErrorEventArgs(int id, int errorCode, string errorMsg, string advancedOrderRejectJson)
        {
            this.Id = id;
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMsg;
            AdvancedOrderRejectJson=advancedOrderRejectJson;
        }

        /// <summary>
        /// Gets the Id
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the error code
        /// </summary>
        public int ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }
        public string AdvancedOrderRejectJson { get; }
    }
}
