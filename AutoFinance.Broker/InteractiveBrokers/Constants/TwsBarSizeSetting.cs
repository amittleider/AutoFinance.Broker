// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// Constants to specify a TWS bar size setting for historical data requests.
    /// </summary>
    public class TwsBarSizeSetting : ITwsStringParameter
    {
        private static TwsBarSizeSetting oneSecond = new TwsBarSizeSetting("1 secs");
        private static TwsBarSizeSetting fiveSeconds = new TwsBarSizeSetting("5 secs");
        private static TwsBarSizeSetting oneMinute = new TwsBarSizeSetting("1 min");
        private static TwsBarSizeSetting fiveMinutes = new TwsBarSizeSetting("5 mins");
        private static TwsBarSizeSetting fifteenMinutes = new TwsBarSizeSetting("15 mins");
        private static TwsBarSizeSetting thirtyMinutes = new TwsBarSizeSetting("30 mins");
        private static TwsBarSizeSetting oneHour = new TwsBarSizeSetting("1 hour");
        private static TwsBarSizeSetting oneDay = new TwsBarSizeSetting("1 day");

        /// <summary>
        /// The underlying string
        /// </summary>
        private readonly string type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsBarSizeSetting"/> class.
        /// </summary>
        /// <param name="type">The underlying string</param>
        private TwsBarSizeSetting(string type)
        {
            this.type = type;
        }

        /// <summary>
        /// Gets one second
        /// </summary>
        public static TwsBarSizeSetting OneSecond { get => oneSecond; }

        /// <summary>
        /// Gets five seconds
        /// </summary>
        public static TwsBarSizeSetting FiveSeconds { get => fiveSeconds; }

        /// <summary>
        /// Gets one minute
        /// </summary>
        public static TwsBarSizeSetting OneMinute { get => oneMinute; }

        /// <summary>
        /// Gets five minutes
        /// </summary>
        public static TwsBarSizeSetting FiveMinutes { get => fiveMinutes; }

        /// <summary>
        /// Gets fifteen minutes
        /// </summary>
        public static TwsBarSizeSetting FifteenMinutes { get => fifteenMinutes; }

        /// <summary>
        /// Gets thirty minutes
        /// </summary>
        public static TwsBarSizeSetting ThirtyMinutes { get => thirtyMinutes; }

        /// <summary>
        /// Gets one hour
        /// </summary>
        public static TwsBarSizeSetting OneHour { get => oneHour; }

        /// <summary>
        /// Gets one day
        /// </summary>
        public static TwsBarSizeSetting OneDay { get => oneDay; }

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
