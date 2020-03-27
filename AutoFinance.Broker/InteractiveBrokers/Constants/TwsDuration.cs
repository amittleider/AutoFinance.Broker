// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// The TWS duration. Supported strings are: integer{SPACE}unit (S|D|W|M|Y)
    /// </summary>
    public class TwsDuration : ITwsStringParameter
    {
        private static TwsDuration oneSecond = new TwsDuration("1 S");
        private static TwsDuration oneDay = new TwsDuration("1 D");
        private static TwsDuration oneWeek = new TwsDuration("1 W");
        private static TwsDuration oneMonth = new TwsDuration("1 M");
        private static TwsDuration oneYear = new TwsDuration("1 Y");

        /// <summary>
        /// The underlying string
        /// </summary>
        private readonly string type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsDuration"/> class.
        /// </summary>
        /// <param name="type">The underlying string</param>
        private TwsDuration(string type)
        {
            this.type = type;
        }

        /// <summary>
        /// Gets one second
        /// </summary>
        public static TwsDuration OneSecond { get => oneSecond; }

        /// <summary>
        /// Gets one day
        /// </summary>
        public static TwsDuration OneDay { get => oneDay; }

        /// <summary>
        /// Gets one week
        /// </summary>
        public static TwsDuration OneWeek { get => oneWeek; }

        /// <summary>
        /// Gets one month
        /// </summary>
        public static TwsDuration OneMonth { get => oneMonth; }

        /// <summary>
        /// Gets one year
        /// </summary>
        public static TwsDuration OneYear { get => oneYear; }

        /// <summary>
        /// Get the TWS parameter
        /// </summary>
        /// <returns>The TWS parameter</returns>
        public string ToTwsParameter()
        {
            return this.type;
        }
    }
}
