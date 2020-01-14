// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;

    /// <summary>
    /// This class receives messages about account updates from TWS and stores them.
    /// It provides public methods to retrieve the information on demand.
    /// </summary>
    public class TwsPositionsController
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
        /// Initializes a new instance of the <see cref="TwsPositionsController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS callback handler</param>
        public TwsPositionsController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
        }

        /// <summary>
        /// Get a list of all the positions in TWS.
        /// </summary>
        /// <returns>A list of position status events from TWS.</returns>
        public Task<List<PositionStatusEventArgs>> RequestPositions()
        {
            List<PositionStatusEventArgs> positionStatusEvents = new List<PositionStatusEventArgs>();
            var taskSource = new TaskCompletionSource<List<PositionStatusEventArgs>>();
            EventHandler<PositionStatusEventArgs> positionStatusEventHandler = null;
            EventHandler<RequestPositionsEndEventArgs> requestPositionsEndEventHandler = null;

            positionStatusEventHandler = (sender, args) =>
            {
                positionStatusEvents.Add(args);
            };

            requestPositionsEndEventHandler = (sender, args) =>
            {
                // Cleanup the event handlers when the positions end event is called.
                this.twsCallbackHandler.PositionStatusEvent -= positionStatusEventHandler;
                this.twsCallbackHandler.RequestPositionsEndEvent -= requestPositionsEndEventHandler;

                taskSource.SetResult(positionStatusEvents);
            };

            this.twsCallbackHandler.PositionStatusEvent += positionStatusEventHandler;
            this.twsCallbackHandler.RequestPositionsEndEvent += requestPositionsEndEventHandler;

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            tokenSource.Token.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.clientSocket.RequestPositions();

            return taskSource.Task;
        }
    }
}
