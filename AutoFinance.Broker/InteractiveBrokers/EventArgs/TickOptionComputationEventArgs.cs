// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Events container for data from the tickOptionComputation TWS endpoint
    /// </summary>
    public class TickOptionComputationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickOptionComputationEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        /// <param name="field">The ticker field</param>
        /// <param name="impliedVolatility">The implied volatility calculated by the TWS option modeler, using the specified tick type value</param>
        /// <param name="delta">The option delta value</param>
        /// <param name="optPrice">The option price</param>
        /// <param name="pwDividend">The present value of dividends expected on the option's underlying</param>
        /// <param name="gamma">The option gamma value</param>
        /// <param name="vega">The option vega value</param>
        /// <param name="theta">The option theta value</param>
        /// <param name="undPrice">The price of the underlying</param>
        public TickOptionComputationEventArgs(
            int tickerId,
            int field,
            double impliedVolatility,
            double delta,
            double optPrice,
            double pwDividend,
            double gamma,
            double vega,
            double theta,
            double undPrice)
        {
            this.TickerId = tickerId;
            this.Field = field;
        }

        /// <summary>
        /// Gets the ticker id
        /// </summary>
        public int TickerId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the field
        /// </summary>
        public int Field
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the implied volatility calculated by the TWS option modeler, using the specified tick type value
        /// </summary>
        public double ImpliedVolatility
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the option delta value
        /// </summary>
        public double Delta
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the option price
        /// </summary>
        public double OptPrice
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the present value of dividends expected on the option's underlying
        /// </summary>
        public double PwDividend
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the option gamma value
        /// </summary>
        public double Gamma
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the option vega value
        /// </summary>
        public double Vega
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the option theta value
        /// </summary>
        public double Theta
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the price of the underlying
        /// </summary>
        public double UndPrice
        {
            get;
            private set;
        }
    }
}
