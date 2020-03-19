// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// Holds data returned by the News Providers TWS endpoint
    /// </summary>
    public class NewsProviderEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsProviderEventArgs"/> class.
        /// </summary>
        /// <param name="newsProviders">The news providers, given by TWS</param>
        public NewsProviderEventArgs(NewsProvider[] newsProviders)
        {
            this.NewsProviders = newsProviders;
        }

        /// <summary>
        /// Gets the news providers
        /// </summary>
        public NewsProvider[] NewsProviders
        {
            get;
            private set;
        }
    }
}
