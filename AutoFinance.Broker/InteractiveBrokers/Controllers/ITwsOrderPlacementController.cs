// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

using IBApi;
using System.Threading.Tasks;

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    public interface ITwsOrderPlacementController
    {
        Task<bool> PlaceOrderAsync(int orderId, Contract contract, Order order);
    }
}