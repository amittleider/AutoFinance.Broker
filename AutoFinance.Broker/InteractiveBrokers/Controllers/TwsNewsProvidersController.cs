// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;

    /// <summary>
    /// Has access to news providers in TWS
    /// </summary>
    public class TwsNewsProvidersController
    {
        /// <summary>
        /// The client socket
        /// </summary>
        private ITwsClientSocket clientSocket;

        /// <summary>
        /// The TWS callback handler
        /// </summary>
        private ITwsCallbackHandler twsCallbackHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsNewsProvidersController"/> class.
        /// </summary>
        /// <param name="clientSocket">The client socket</param>
        /// <param name="twsCallbackHandler">The callback handler</param>
        public TwsNewsProvidersController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
        }

        /// <summary>
        /// Gets news providers from TWS
        /// </summary>
        /// <returns>News providers</returns>
        public Task<NewsProviderEventArgs> GetNewsProviders()
        {
            var taskSource = new TaskCompletionSource<NewsProviderEventArgs>();

            EventHandler<NewsProviderEventArgs> newsProviderCallback = null;
            newsProviderCallback = (sender, eventArgs) =>
            {
                    // Unregister the callbacks
                    this.twsCallbackHandler.NewsProviderEvent -= newsProviderCallback;
                    taskSource.TrySetResult(eventArgs);
            };

            this.twsCallbackHandler.NewsProviderEvent += newsProviderCallback;
            this.clientSocket.EClientSocket.reqNewsProviders();

            return taskSource.Task;
        }
    }
}
