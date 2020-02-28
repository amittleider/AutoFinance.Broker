using AutoFinance.Broker.InteractiveBrokers.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    public interface ITwsOpenOrdersController
    {
        Task<List<OpenOrderEventArgs>> RequestOpenOrders();
    }
}