// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// The event arguments when a commission report is received.
    /// </summary>
    public class CommissionReportEventArgs
    {
       /// <summary>
       /// Initializes a new instance of the <see cref="CommissionReportEventArgs"/> class.
       /// </summary>
       /// <param name="commissionReport">The commission report from reqExecutions().</param>
        public CommissionReportEventArgs(CommissionReport commissionReport)
        {
            this.CommissionReport = commissionReport;
        }

        /// <summary>
        /// Gets the commission report
        /// </summary>
        public CommissionReport CommissionReport { get; private set; }
    }
}
