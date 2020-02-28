// Licensed under the Apache License, Version 2.0.

using System.Threading.Tasks;

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    public interface ITwsOrderCancelationController
    {
        Task<bool> CancelOrderAsync(int orderId);
    }
}