// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;

    /// <summary>
    /// This class receives messages about account updates from TWS and stores them.
    /// It provides public methods to retrieve the information on demand.
    /// </summary>
    internal class TwsAccountUpdatesController
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
        /// The account updates dictionary
        /// </summary>
        private ConcurrentDictionary<string, string> accountUpdates;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsAccountUpdatesController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS callback handler</param>
        public TwsAccountUpdatesController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
            this.accountUpdates = new ConcurrentDictionary<string, string>();
            this.twsCallbackHandler.UpdateAccountValueEvent += this.OnUpdateAccountValueEvent;
        }

        /// <summary>
        /// Get an account detail
        /// </summary>
        /// <param name="accountId">The account Id</param>
        /// <returns>The account values</returns>
        public Task<ConcurrentDictionary<string, string>> GetAccountDetailsAsync(string accountId)
        {
            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            return this.GetAccountDetailsAsync(accountId, tokenSource.Token);
        }

        /// <summary>
        /// Get an account detail
        /// </summary>
        /// <param name="accountId">The account Id</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>The account values</returns>
        public Task<ConcurrentDictionary<string, string>> GetAccountDetailsAsync(string accountId, CancellationToken cancellationToken)
        {
            string value = string.Empty;

            var taskSource = new TaskCompletionSource<ConcurrentDictionary<string, string>>();
            this.twsCallbackHandler.AccountDownloadEndEvent += (sender, args) =>
            {
                taskSource.TrySetResult(this.accountUpdates);
            };

            cancellationToken.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.clientSocket.ReqAccountDetails(true, accountId);

            return taskSource.Task;
        }

        /// <summary>
        /// Run when an account value is updated
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="eventArgs">The event arguments</param>
        private void OnUpdateAccountValueEvent(object sender, UpdateAccountValueEventArgs eventArgs)
        {
            this.accountUpdates.AddOrUpdate(eventArgs.Key, eventArgs.Value, (key, value) =>
            {
                return eventArgs.Value; // Always take the most recent result
            });
        }
    }
}
